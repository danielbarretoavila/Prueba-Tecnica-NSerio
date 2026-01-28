using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Data;
using TeamTasksApi.DTOs;

namespace TeamTasksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly TeamTasksDbContext _context;

    public ProjectsController(TeamTasksDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/projects
    /// Returns all projects with task statistics
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var projects = await _context.Projects
            .Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                Name = p.Name,
                ClientName = p.ClientName,
                Status = p.Status,
                TotalTasks = p.Tasks.Count,
                OpenTasks = p.Tasks.Count(t => t.Status != "Completed"),
                CompletedTasks = p.Tasks.Count(t => t.Status == "Completed")
            })
            .ToListAsync();

        return Ok(projects);
    }

    /// <summary>
    /// GET /api/projects/{id}/tasks
    /// Returns tasks for a specific project with filtering and pagination
    /// </summary>
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetProjectTasks(
        int id,
        [FromQuery] string? status = null,
        [FromQuery] int? assigneeId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Validate project exists
        var projectExists = await _context.Projects.AnyAsync(p => p.ProjectId == id);
        if (!projectExists)
        {
            return NotFound(new { message = $"Project with ID {id} not found." });
        }

        // Build query
        var query = _context.Tasks
            .Where(t => t.ProjectId == id)
            .Include(t => t.Assignee)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(t => t.Status == status);
        }

        if (assigneeId.HasValue)
        {
            query = query.Where(t => t.AssigneeId == assigneeId.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var tasks = await query
            .OrderBy(t => t.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                Title = t.Title,
                Description = t.Description,
                AssigneeId = t.AssigneeId,
                AssigneeName = t.Assignee.FirstName + " " + t.Assignee.LastName,
                Status = t.Status,
                Priority = t.Priority,
                EstimatedComplexity = t.EstimatedComplexity,
                DueDate = t.DueDate,
                CompletionDate = t.CompletionDate,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        var result = new PagedResult<TaskDto>
        {
            Items = tasks,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return Ok(result);
    }
}

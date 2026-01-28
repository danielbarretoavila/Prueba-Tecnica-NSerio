using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Data;
using TeamTasksApi.DTOs;

namespace TeamTasksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TeamTasksDbContext _context;

    public TasksController(TeamTasksDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// POST /api/tasks
    /// Creates a new task with validation
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(createTaskDto.Title))
        {
            return BadRequest(new { message = "El título de la tarea es requerido." });
        }

        // Validate ProjectId exists
        var projectExists = await _context.Projects.AnyAsync(p => p.ProjectId == createTaskDto.ProjectId);
        if (!projectExists)
        {
            return BadRequest(new { message = "El ProjectId especificado no existe." });
        }

        // Validate AssigneeId exists and is active
        var assigneeExists = await _context.Developers
            .AnyAsync(d => d.DeveloperId == createTaskDto.AssigneeId && d.IsActive);
        if (!assigneeExists)
        {
            return BadRequest(new { message = "El AssigneeId especificado no existe o el desarrollador no está activo." });
        }

        // Validate Status
        var validStatuses = new[] { "ToDo", "InProgress", "Blocked", "Completed" };
        if (!validStatuses.Contains(createTaskDto.Status))
        {
            return BadRequest(new { message = "Status debe ser: ToDo, InProgress, Blocked, o Completed." });
        }

        // Validate Priority
        var validPriorities = new[] { "Low", "Medium", "High" };
        if (!validPriorities.Contains(createTaskDto.Priority))
        {
            return BadRequest(new { message = "Priority debe ser: Low, Medium, o High." });
        }

        // Validate EstimatedComplexity
        if (createTaskDto.EstimatedComplexity < 1 || createTaskDto.EstimatedComplexity > 5)
        {
            return BadRequest(new { message = "EstimatedComplexity debe estar entre 1 y 5." });
        }

        // Create the task
        var task = new Models.Task
        {
            ProjectId = createTaskDto.ProjectId,
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            AssigneeId = createTaskDto.AssigneeId,
            Status = createTaskDto.Status,
            Priority = createTaskDto.Priority,
            EstimatedComplexity = createTaskDto.EstimatedComplexity,
            DueDate = createTaskDto.DueDate,
            CompletionDate = createTaskDto.CompletionDate,
            CreatedAt = DateTime.Now
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Load the assignee for the response
        await _context.Entry(task).Reference(t => t.Assignee).LoadAsync();

        var taskDto = new TaskDto
        {
            TaskId = task.TaskId,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            AssigneeId = task.AssigneeId,
            AssigneeName = $"{task.Assignee.FirstName} {task.Assignee.LastName}",
            Status = task.Status,
            Priority = task.Priority,
            EstimatedComplexity = task.EstimatedComplexity,
            DueDate = task.DueDate,
            CompletionDate = task.CompletionDate,
            CreatedAt = task.CreatedAt
        };

        return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, taskDto);
    }

    /// <summary>
    /// GET /api/tasks/{id}
    /// Gets a specific task by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Assignee)
            .Where(t => t.TaskId == id)
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
            .FirstOrDefaultAsync();

        if (task == null)
        {
            return NotFound(new { message = $"Task with ID {id} not found." });
        }

        return Ok(task);
    }

    /// <summary>
    /// PUT /api/tasks/{id}/status
    /// Updates the status of a task (and optionally priority and complexity)
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<TaskDto>> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto updateDto)
    {
        var task = await _context.Tasks
            .Include(t => t.Assignee)
            .FirstOrDefaultAsync(t => t.TaskId == id);

        if (task == null)
        {
            return NotFound(new { message = $"Task with ID {id} not found." });
        }

        // Validate Status
        var validStatuses = new[] { "ToDo", "InProgress", "Blocked", "Completed" };
        if (!validStatuses.Contains(updateDto.Status))
        {
            return BadRequest(new { message = "Status debe ser: ToDo, InProgress, Blocked, o Completed." });
        }

        task.Status = updateDto.Status;

        // Update completion date if status is Completed
        if (updateDto.Status == "Completed" && !task.CompletionDate.HasValue)
        {
            task.CompletionDate = DateTime.Now;
        }
        else if (updateDto.Status != "Completed")
        {
            task.CompletionDate = null;
        }

        // Update priority if provided
        if (!string.IsNullOrEmpty(updateDto.Priority))
        {
            var validPriorities = new[] { "Low", "Medium", "High" };
            if (!validPriorities.Contains(updateDto.Priority))
            {
                return BadRequest(new { message = "Priority debe ser: Low, Medium, o High." });
            }
            task.Priority = updateDto.Priority;
        }

        // Update complexity if provided
        if (updateDto.EstimatedComplexity.HasValue)
        {
            if (updateDto.EstimatedComplexity.Value < 1 || updateDto.EstimatedComplexity.Value > 5)
            {
                return BadRequest(new { message = "EstimatedComplexity debe estar entre 1 y 5." });
            }
            task.EstimatedComplexity = updateDto.EstimatedComplexity.Value;
        }

        await _context.SaveChangesAsync();

        var taskDto = new TaskDto
        {
            TaskId = task.TaskId,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            AssigneeId = task.AssigneeId,
            AssigneeName = $"{task.Assignee.FirstName} {task.Assignee.LastName}",
            Status = task.Status,
            Priority = task.Priority,
            EstimatedComplexity = task.EstimatedComplexity,
            DueDate = task.DueDate,
            CompletionDate = task.CompletionDate,
            CreatedAt = task.CreatedAt
        };

        return Ok(taskDto);
    }
}

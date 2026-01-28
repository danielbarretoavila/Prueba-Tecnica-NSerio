using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Data;
using TeamTasksApi.DTOs;

namespace TeamTasksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly TeamTasksDbContext _context;

    public DashboardController(TeamTasksDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/dashboard/developer-workload
    /// Returns workload summary per active developer
    /// </summary>
    [HttpGet("developer-workload")]
    public async Task<ActionResult<IEnumerable<DeveloperWorkloadDto>>> GetDeveloperWorkload()
    {
        var workload = await _context.Developers
            .Where(d => d.IsActive)
            .Select(d => new DeveloperWorkloadDto
            {
                DeveloperId = d.DeveloperId,
                DeveloperName = d.FirstName + " " + d.LastName,
                OpenTasksCount = d.Tasks.Count(t => t.Status != "Completed"),
                AverageEstimatedComplexity = d.Tasks
                    .Where(t => t.Status != "Completed")
                    .Average(t => (decimal?)t.EstimatedComplexity) ?? 0
            })
            .ToListAsync();

        return Ok(workload);
    }

    /// <summary>
    /// GET /api/dashboard/project-health
    /// Returns health status summary per project
    /// </summary>
    [HttpGet("project-health")]
    public async Task<ActionResult<IEnumerable<ProjectHealthDto>>> GetProjectHealth()
    {
        var health = await _context.Projects
            .Select(p => new ProjectHealthDto
            {
                ProjectId = p.ProjectId,
                ProjectName = p.Name,
                ClientName = p.ClientName,
                ProjectStatus = p.Status,
                TotalTasks = p.Tasks.Count,
                OpenTasks = p.Tasks.Count(t => t.Status != "Completed"),
                CompletedTasks = p.Tasks.Count(t => t.Status == "Completed")
            })
            .ToListAsync();

        return Ok(health);
    }

    /// <summary>
    /// GET /api/dashboard/developer-delay-risk
    /// Returns delay risk prediction per active developer
    /// </summary>
    [HttpGet("developer-delay-risk")]
    public async Task<ActionResult<IEnumerable<DeveloperDelayRiskDto>>> GetDeveloperDelayRisk()
    {
        var developers = await _context.Developers
            .Where(d => d.IsActive)
            .Include(d => d.Tasks)
            .ToListAsync();

        var riskData = developers
            .Select(d =>
            {
                var openTasks = d.Tasks.Where(t => t.Status != "Completed").ToList();
                var completedTasks = d.Tasks.Where(t => t.Status == "Completed" && t.CompletionDate.HasValue).ToList();

                if (!openTasks.Any())
                    return null;

                // Calculate average delay from completed tasks
                var avgDelayDays = completedTasks.Any()
                    ? (decimal)completedTasks.Average(t =>
                    {
                        var delay = (t.CompletionDate!.Value.Date - t.DueDate.Date).Days;
                        return delay > 0 ? delay : 0;
                    })
                    : 0;

                var nearestDueDate = openTasks.Min(t => t.DueDate);
                var latestDueDate = openTasks.Max(t => t.DueDate);
                var predictedCompletionDate = latestDueDate.AddDays((double)avgDelayDays);

                var highRiskFlag = (predictedCompletionDate > latestDueDate || avgDelayDays > 3) ? 1 : 0;

                return new DeveloperDelayRiskDto
                {
                    DeveloperId = d.DeveloperId,
                    DeveloperName = $"{d.FirstName} {d.LastName}",
                    OpenTasksCount = openTasks.Count,
                    AvgDelayDays = avgDelayDays,
                    NearestDueDate = nearestDueDate,
                    LatestDueDate = latestDueDate,
                    PredictedCompletionDate = predictedCompletionDate,
                    HighRiskFlag = highRiskFlag
                };
            })
            .Where(r => r != null)
            .Cast<DeveloperDelayRiskDto>()
            .ToList();

        return Ok(riskData);
    }
}

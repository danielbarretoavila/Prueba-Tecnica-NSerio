namespace TeamTasksApi.DTOs;

public class DeveloperWorkloadDto
{
    public int DeveloperId { get; set; }
    public string DeveloperName { get; set; } = string.Empty;
    public int OpenTasksCount { get; set; }
    public decimal AverageEstimatedComplexity { get; set; }
}

public class ProjectHealthDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ProjectStatus { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int OpenTasks { get; set; }
    public int CompletedTasks { get; set; }
}

public class DeveloperDelayRiskDto
{
    public int DeveloperId { get; set; }
    public string DeveloperName { get; set; } = string.Empty;
    public int OpenTasksCount { get; set; }
    public decimal AvgDelayDays { get; set; }
    public DateTime? NearestDueDate { get; set; }
    public DateTime? LatestDueDate { get; set; }
    public DateTime? PredictedCompletionDate { get; set; }
    public int HighRiskFlag { get; set; }
}

public class ProjectDto
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int OpenTasks { get; set; }
    public int CompletedTasks { get; set; }
}

public class TaskDto
{
    public int TaskId { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AssigneeId { get; set; }
    public string AssigneeName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int EstimatedComplexity { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskDto
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AssigneeId { get; set; }
    public string Status { get; set; } = "ToDo";
    public string Priority { get; set; } = "Medium";
    public int EstimatedComplexity { get; set; } = 3;
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class UpdateTaskStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string? Priority { get; set; }
    public int? EstimatedComplexity { get; set; }
}

public class DeveloperDto
{
    public int DeveloperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

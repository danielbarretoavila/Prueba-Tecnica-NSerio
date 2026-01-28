namespace TeamTasksApi.Models;

public class Task
{
    public int TaskId { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AssigneeId { get; set; }
    public string Status { get; set; } = string.Empty; // ToDo, InProgress, Blocked, Completed
    public string Priority { get; set; } = string.Empty; // Low, Medium, High
    public int EstimatedComplexity { get; set; } // 1-5
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public Developer Assignee { get; set; } = null!;
}

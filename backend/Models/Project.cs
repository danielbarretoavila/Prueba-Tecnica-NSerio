namespace TeamTasksApi.Models;

public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty; // Planned, InProgress, Completed
    public DateTime CreatedAt { get; set; }

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}

namespace TeamTasksApi.Models;

public class Developer
{
    public int DeveloperId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}

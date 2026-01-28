using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Data;
using TeamTasksApi.DTOs;

namespace TeamTasksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevelopersController : ControllerBase
{
    private readonly TeamTasksDbContext _context;

    public DevelopersController(TeamTasksDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/developers
    /// Returns all active developers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetDevelopers()
    {
        var developers = await _context.Developers
            .Where(d => d.IsActive)
            .Select(d => new DeveloperDto
            {
                DeveloperId = d.DeveloperId,
                FullName = d.FirstName + " " + d.LastName,
                Email = d.Email
            })
            .ToListAsync();

        return Ok(developers);
    }
}

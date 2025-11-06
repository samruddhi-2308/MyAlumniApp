using Microsoft.AspNetCore.Mvc;
using MyAlumniApp.Data;
using System.Linq;

namespace MyAlumniApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult Search(
            string? keyword,
            string? year,
            string? organization,
            string? achievement,
            string? category)
        {
            var query = _context.People.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(p => p.Name.Contains(keyword) ||
                                         p.Description.Contains(keyword) ||
                                         p.Domain.Contains(keyword));

            if (!string.IsNullOrEmpty(year))
                query = query.Where(p => p.Batch_Year == year);

            if (!string.IsNullOrEmpty(organization))
                query = query.Where(p => p.Organization == organization);

            if (!string.IsNullOrEmpty(achievement))
                query = query.Where(p => p.Award_Or_Achievement == achievement);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Domain == category);

            return Ok(query.ToList());
        }
    }
}

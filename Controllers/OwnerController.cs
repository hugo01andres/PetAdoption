using demowebapi.DataAccessLayer;
using demowebapi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demowebapi.Controllers
{
    public class OwnerController : Controller
    {
        private readonly WpmDBContext dbContext;
        public OwnerController(WpmDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("pets/{id}/owners")]
        public async Task<ActionResult<IEnumerable<Owner>>> GetPetOwner(int id)
        {
            var petOwner = await dbContext
                                  .Pets
                                  .Include(p => p.Owners)
                                  .Where(p => p.Id == id)
                                  .SelectMany(p => p.Owners)
                                  .Select(p => new { p.Id, p.Name })
                                  .ToListAsync();
            return petOwner != null ? Ok(petOwner) : NotFound();
        }
    }
}

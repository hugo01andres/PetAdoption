using demowebapi.DataAccessLayer;
using demowebapi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demowebapi.Controllers
{
    public class PetController : Controller
    {
        private readonly WpmDBContext dbContext;
        public PetController(WpmDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("pets")]
        public async Task<ActionResult<IEnumerable<PetViewModel>>> GetAllPets()
        {
            var allPets = await dbContext.Pets
                                .Include(p => p.Breed)
                                .Select(p => new PetViewModel(p.Id, p.Name, p.Age, p.Weight, p.Breed.Name))
                                .ToListAsync();
            return Ok(allPets);
        }

        [HttpGet("breeds/{id}/pets")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetAllPetsByBreed(int id)
        {
            var allPets = await dbContext.Pets.Where(b => b.BreedId == id).ToListAsync();
            return allPets.Any() ? Ok(allPets) : NotFound();
        }
    }

    public record PetViewModel(int Id, string Name, int? Age, decimal? Weight, string BreedName);
}

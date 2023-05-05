using demowebapi.DataAccessLayer;
using demowebapi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demowebapi.Controllers
{
    public class BreedController : Controller
    {
        private readonly WpmDBContext dbContext;
        public BreedController(WpmDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("species/{id}/breeds)")]
        public async Task<ActionResult<IEnumerable<BreedViewModel>>> GetBreedBySpecies(int id)
        {
            var allBreeds = await dbContext
                .Breeds
                .Include(b => b.Species)
                .Where(b => b.SpeciesId == id)
                .Select(b => new BreedViewModel(b.Id, b.Name, b.Species.Name))
                .ToListAsync();
            return allBreeds.Any() ? Ok(allBreeds) : NotFound();
        }

        [HttpGet("breeds")]
        public async Task<ActionResult<IEnumerable<Breed>>> GetAllBreeds()
        {
            var allBreeds = await dbContext.Breeds.ToListAsync();
            return Ok(allBreeds);
        }

        [HttpPost("species/{speciesId}/breeds")]
        public async Task<IActionResult> CreateBreed(int speciesId, BreedModel breedModel)
        {
            var newBreed = new Domain.Breed()
            {
                Name = breedModel.Name,
                SpeciesId = speciesId,
                IdealMaxWeight = breedModel.IdealMaxWeight
            };
            dbContext.Breeds.Add(newBreed);
            var result = await dbContext.SaveChangesAsync();
            return result == 1 ? Ok(newBreed.Id) : BadRequest();
        }

        [HttpPut("breeds/{id}")]
        public async Task<IActionResult> UpdateBreed(int id, BreedModel breedModel)
        {
            var breed = dbContext.Breeds.First(b => b.Id == id);
            breed.Name = breedModel.Name;
            breed.IdealMaxWeight = breedModel.IdealMaxWeight;
            var result = await dbContext.SaveChangesAsync();
            return result == 1 ? Ok() : BadRequest();
        }
    }

    public record BreedViewModel(int Id, string BreedName, string SpeciesName);
    public record BreedModel(string Name, decimal IdealMaxWeight);
}

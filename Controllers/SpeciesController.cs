using Microsoft.AspNetCore.Mvc;
using demowebapi.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using demowebapi.Domain;

namespace demowebapi.Controllers
{
    [ApiController]
    [Route("api")]
    public class SpeciesController : ControllerBase
    {
        private readonly WpmDBContext dbContext;
        public SpeciesController(WpmDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet("species")] //api/species
        public async Task<ActionResult<IEnumerable<SpeciesViewModel>>> GetAllSpecies()
        {
            var allSpecies = await dbContext.
                                    Species.
                                    Select(s => new SpeciesViewModel(s.Id, s.Name)).
                                    ToListAsync();
            return Ok(allSpecies);
        }

       
        
        
        
    }

    public record SpeciesViewModel(int Id, string Name);
 

    

}
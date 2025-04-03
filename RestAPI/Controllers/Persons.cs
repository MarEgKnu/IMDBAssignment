using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReadFromDatabase;
using RestAPI.Models;
using System.Net;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Persons : ControllerBase
    {
        private ReadClass _repo;

        public Persons(ReadClass repo)
        {
            _repo = repo; 
        }

        [HttpGet()]
        [Route("Persons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery] SearchDTO dto)
        {
            
            try
            {
                if (dto == null)
                {
                    return Ok(_repo.ReadPersonsWithTitles());
                }


                return Ok(_repo.ReadPersonsWithTitles(dto.search, dto.offset, dto.rows, dto.ascending));
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
            
            
        }
    }
}

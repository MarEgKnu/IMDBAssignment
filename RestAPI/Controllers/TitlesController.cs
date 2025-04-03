using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFromDatabase;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private ReadClass _repo;

        public TitlesController(ReadClass repo)
        {
            _repo = repo;
        }

        [HttpGet()]
        [Route("Titles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetTitles([FromQuery] SearchDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return Ok(_repo.GetTitles());
                }
                return Ok(_repo.GetTitles(dto.search, dto.offset, dto.rows, dto.ascending));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

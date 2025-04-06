using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ReadFromDatabase;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("[controller]")]
    [EnableCors("def")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private ITitleRepository _repo;

        public TitlesController(ITitleRepository repo)
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
                    return Ok(_repo.GetTitlesBasic());
                }
                return Ok(_repo.GetTitlesBasic(dto.search, dto.offset, dto.rows, dto.ascending));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost()]
        [Route("Titles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult PostTitle([FromBody] TitleBasicDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return StatusCode(406);
                }
                TitleWithGenres title = DTOConverter.ConvertTitlesBasicDTO(dto);
                title = _repo.AddTitleBasic(title);
                return Created($"Titles/{title.ID}" , title);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

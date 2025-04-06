using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ReadFromDatabase;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [ApiController]
    [EnableCors("def")]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private IPersonRepository _repo;

        public PersonsController(IPersonRepository repo)
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
                    return Ok(_repo.ReadPersonsBasic());
                }


                return Ok(_repo.ReadPersonsBasic(dto.search, dto.offset, dto.rows, dto.ascending));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost()]
        [Route("Persons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult PostPerson([FromBody] PersonBasicDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return StatusCode(406);
                }
                PersonWithTitles person = DTOConverter.ConvertPersonBasicDTO(dto);
                person = _repo.AddPersonBasic(person);
                return Created($"Persons/{person.Id}", person);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

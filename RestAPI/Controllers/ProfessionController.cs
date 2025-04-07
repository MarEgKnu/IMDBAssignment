using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ReadFromDatabase;

namespace RestAPI.Controllers
{
    [ApiController]
    [EnableCors("def")]
    [Route("[controller]")]
    public class ProfessionController : ControllerBase
    {
        private IProfessionRepository _repo;

        public ProfessionController(IProfessionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(byte id)
        {

            try
            {
                Profession? result = _repo.GetByID(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {

            try
            {
                List<Profession> result = _repo.GetAll();
                if (result.Count == 0)
                {
                    return NoContent();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}

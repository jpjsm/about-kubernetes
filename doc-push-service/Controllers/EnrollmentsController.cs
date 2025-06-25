using Microsoft.AspNetCore.Mvc;
using doc_push_service.DbCtx;
using doc_push_service.models;

namespace doc_push_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        // GET: Urls
        [HttpGet]
        public IEnumerable<string> GetEnrollments()
        {

            return EnrollmentsDb.List();
        }

        // POST: Urls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] String new_enrollment)
        {
            Enrollment enrollment = new Enrollment("http://localhost"); // this is a do nothing assignment, to avoid compiler complains
            try
            {
                enrollment = new Enrollment(new_enrollment);
            }
            catch (Exception)
            {

                return BadRequest("Invalid URL.");
            }

            bool result = await EnrollmentsDb.InsertAsync(enrollment.AbsoluteUri);
            if (result) 
            {
                return Created("added", enrollment.AbsoluteUri);
            }

            return Ok(enrollment.AbsoluteUri);
        }

        // DELETE: Urls/5
        [HttpDelete("{existing_enrollment}")]
        public async Task<IActionResult> DeleteUrl(string existing_enrollment)
        {
            Enrollment enrollment = new Enrollment(existing_enrollment);

            bool result = await EnrollmentsDb.DeleteAsync(enrollment.AbsoluteUri);

            if (result)
            {
                return NoContent();
            }

            return NotFound(enrollment.AbsoluteUri);
        }
    }
}

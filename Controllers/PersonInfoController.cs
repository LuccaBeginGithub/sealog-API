using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.DataAccess;

namespace WebAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PersonInfoController : Controller
    {
       private readonly MongoDBDataAccess _mongoDBDataAccess;

       public PersonInfoController(MongoDBDataAccess mongoDBDataAccess)
        {
            _mongoDBDataAccess = mongoDBDataAccess;
        }

        [HttpGet]
        public async Task<List<PersonModel>> Get() { 
            return await _mongoDBDataAccess.GetAsync();
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetPersonByEmail([FromQuery] string email)
        {
            var person = await _mongoDBDataAccess.GetPersonByEmail(email);

            if(person == null)
            {
                return NotFound("Email not found");
            }
            return Json(person);
        }


        [HttpPost]
        public async Task<IActionResult> AddToPersonList([FromBody] PersonModel personModel)
        {
            if (await _mongoDBDataAccess.EmailExistsAsync(personModel.EmailAddress))
            {
                // Email already exists, return an error response
                return BadRequest("Email address already in use");
            }
            await _mongoDBDataAccess.CreateAsync(personModel);
            return CreatedAtAction(nameof(Get), new { id = personModel.Id }, personModel);

        }

        [HttpPut("{email}")]
        public async Task<IActionResult> AddBooking(string email, [FromBody] HotelModel hotelModel) {
            await _mongoDBDataAccess.AddBooking(email, hotelModel);
            var person = await _mongoDBDataAccess.GetPersonByEmail(email);
            return Json(person);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) { 
            await _mongoDBDataAccess.DeleteAsync(id);
            return NoContent();
        }
    }
}

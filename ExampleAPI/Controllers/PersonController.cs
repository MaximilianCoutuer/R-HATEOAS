namespace ExampleAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ExampleAPI.Models;
    using ExampleAPI.PropertySets;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RDHATEOAS.Filters;
    using RDHATEOAS.Rulesets;

    /// <summary>
    /// This is an example controller that offers basic functionality.
    /// It supports basic CRUD operations.
    /// </summary>
    /// <remarks>
    /// The HATEOAS package supports any kind of controller method.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        #region fields

        private readonly ExampleDbContext _context;

        #endregion

        #region constructors

        public PersonController(ExampleDbContext context)
        {
            _context = context;
        }

        #endregion

        #region methods

        // GET api/person
        [HttpGet]
        [AddHateoasLinks(new[] {
            typeof(ExampleHateoasPropertySetPerson),
            //typeof(ExampleHateoasPropertySetCountry),
        })]
        public async Task<ActionResult<List<Person>>> GetAllPersons()
        {
            IEnumerable<Person> persons = await _context.Persons.Include(p => p.Country).ToListAsync();
            if (persons.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(persons);
            }
        }

        // GET api/person (paginated)
        [HttpGet("{skip, take}")]
        [AddHateoasLinks(new[] {
            typeof(ExampleHateoasPropertySetPerson),
            typeof(ExampleHateoasPropertySetCountry),
        })]
        public async Task<ActionResult<Person>> GetPaginatedList(int skip, int take)
        {
            IEnumerable<Person> persons = await _context.Persons.Skip(skip * take).Take(take).Include(p => p.Country).ToListAsync();
            if (persons.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(persons);
            }
        }

        // GET api/person/5
        [HttpGet("{id}")]
        [AddHateoasLinks(new[] {
            typeof(ExampleHateoasPropertySetPerson),
            typeof(ExampleHateoasPropertySetCountry),
        })]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.FindAsync<Person>(id);

            if (person == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(person);
            }
        }

        // POST api/person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            // returns 201 Created with location header containing GET link to newly created object in DB
            _context.Add<Person>(person);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPerson), new { person.Id }, person);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> PutPerson(int id, Person person)
        {
            // returns 404 Not Found if ID invalid
            // (may also return 400 Bad Request)
            if (id != person.Id)
            {
                return NotFound();
            }

            if (_context.Find<Person>(id) != null)
            {
                // returns 204 No Content
                // (may also return 200 OK)
                _context.Update<Person>(person);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                // returns 201 Created
                _context.Add<Person>(person);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPerson), new { person.Id }, person);
            }
        }

        // DELETE api/person/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            // returns 404 Not Found if ID invalid
            // returns 200 OK
            // (may also return 202 Accepted or 204 No Content)
            Person person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion
    }
}

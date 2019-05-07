using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDHATEOAS.Filters;
using RDHATEOAS.Rulesets;
using RDHATEOAS.Services;

namespace ExampleAPI.Controllers
{
    /// <summary>
    /// This is an example controller that offers basic functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PeopleContext _context;

        public PersonController(PeopleContext context)
        {
            _context = context;

            //// Seed database with some items
            //if (_context.Persons.Count() == 0)
            //{
            //    _context.Add(new Person()
            //    {
            //        FirstName = "Maximilian",
            //        LastName = "Coutuer",
            //        Age = 35,
            //        Country = new Country() {
            //            Name = "Belgium",
            //            Capital = "Brussels"
            //        },
            //    });
            //    _context.Add(new Person()
            //    {
            //        FirstName = "Nicolas",
            //        LastName = "Zawada",
            //        Age = 40,
            //        Country = new Country()
            //        {
            //            Name = "Belgium",
            //            Capital = "Brussels"
            //        },
            //    });
            //    _context.Add(new Person()
            //    {
            //        FirstName = "Wim",
            //        LastName = "Bellemans",
            //        Age = 45,
            //        Country = new Country()
            //        {
            //            Name = "Belgium",
            //            Capital = "Brussels"
            //        },
            //    });
            //    _context.SaveChanges();
            //}
        }

        // GET api/person
        [HttpGet]
        //[TypeFilter(typeof(AddHateoasLinksAttribute))]
        [AddHateoasLinks(null, new[] { typeof(DemoRulesetFullLinks) })]
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
        //[TypeFilter(typeof(AddHateoasLinksAttribute))]
        [AddHateoasLinks(new[] { "skip", "take" }, new[] { typeof(DemoRulesetFullLinks) })]
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
        [AddHateoasLinks(new[] { "Id" }, new[] { typeof(DemoRulesetFullLinks) })]
        public async Task<ActionResult<Person>> GetPerson(int Id)
        {
            var person = await _context.FindAsync<Person>(Id);

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
            // returns 201 with location header containing GET link to newly created object in DB
            _context.Add<Person>(person);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPerson), new { Id = person.Id }, person);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> PutPerson(int id, Person person)
        {
            // returns 404 Not Found if ID invalid
            // (may also return 400 Bad Request)
            if (id != person.Id) return NotFound();

            if (_context.Find<Person>(id) != null)
            {
                // returns 204 No Content
                // (may also return 200 OK)
                _context.Update<Person>(person);
                await _context.SaveChangesAsync();
                return NoContent();
                // TODO: return 409 Conflict or 415 Unsupported Media Type if representation inconsistent
            }
            else
            {
                // returns 201 Created
                _context.Add<Person>(person);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPerson), new { Id = person.Id }, person);
            }
        }

        // DELETE api/person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
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
            return NoContent();
        }

        // PATCH api/person/5
        public async Task<ActionResult<Person>> PatchPerson(int id, Person person)
        {
            throw new NotImplementedException();
        }
    }
}

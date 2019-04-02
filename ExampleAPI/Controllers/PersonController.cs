using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RDHATEOAS.Controllers;
using RDHATEOAS.Filters;
using RDHATEOAS.Services;

// an example API that returns person data. Each person comes with a country.
namespace ExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly ILinkService _linkService;

        public PersonController(PersonContext context)
        {
            _context = context;
            //_linkService = linkService; // ILinkService

            // seed database with one item so we don't need to start off by creating items
            if (_context.Persons.Count() == 0)
            {
                _context.Add(new Person()
                {
                    FirstName = "Maximilian",
                    LastName = "Coutuer",
                    Age = 35,
                    Country = new Country() { Name = "Belgium", Capital = "Brussels" },
                });
                _context.SaveChanges();
            }
        }

        // GET api/person
        [HttpGet]
        public async Task<ActionResult> GetAllPersons()
        {
            IEnumerable<Person> persons = await _context.Persons.ToListAsync();
            if (persons.Count() == 0)
            {
                return NotFound();
            } else
            {
                var testobject = new { Name = "Donald", Link = "Link" };
                var testobject2 = new ExpandoObject();



                var testobject3 = JsonConvert.SerializeObject(persons);


                dynamic testobject4 = persons.ElementAt(0);


                JObject testobject5 = JObject.FromObject(persons.ElementAt(0));
                testobject5.Add("rofl", "lmao");

                return Ok(testobject5);
            }

    //        "_link": [
    //    {
    //        "rel": "customers",
    //        "method": "GET",
    //        "href": "https://"
    //    }
    //]
        }

        // GET api/person/5
        [HttpGet("{id}")]
        [LinkEnabled("Test")]
        [HATEOASLinks("test25")]
        public async Task<ActionResult<Person>> GetPerson(int Id)
        {
            //var test = Attribute.GetCustomAttributes(typeof(RequireHttpsAttribute), true);

            var person = await _context.FindAsync<Person>(Id);

            //var duck = _linkService.AddLinksToOutput(ref person);


            if (person == null)
            {
                return NotFound();
            } else
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
                //_context.Entry(person).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
                _context.Update<Person>(person);
                await _context.SaveChangesAsync();
                return NoContent();
                // TODO: return 409 Conflict or 415 Unsupported Media Type if representation inconsistent
            } else
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
    }
}

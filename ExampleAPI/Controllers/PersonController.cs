namespace ExampleAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ExampleAPI.Models;
    using ExampleAPI.PropertySets;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Rhateoas.Filters;

    /// <summary>
    /// This is an example controller that offers basic functionality.
    /// It supports basic CRUD operations.
    /// </summary>
    /// <remarks>
    /// The HATEOAS package does support any kind of controller method.
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
            this.SeedDatabase();
        }

        #endregion

        #region methods

        /// <summary>
        /// GET api/person
        /// Example API method that returns a list of all Person entities in the database.
        /// </summary>
        /// <returns>
        /// 200 OK unless there are no Person entries, in which case 404 Not Found.
        /// </returns>
        [HttpGet]
        [AddHateoasLinks(new[] {
            typeof(ExamplePropertySetPerson),
            typeof(ExamplePropertySetCountry),
            typeof(ExamplePropertySetCity),
            typeof(ExamplePropertySetInvalid),
            typeof(ExamplePropertySetSelf),
        })]
        public async Task<ActionResult<List<Person>>> GetAllPersons()
        {
            IEnumerable<Person> persons = await _context.Persons
                .Include(p => p.Country)
                .Include(p => p.Country.Capital)
                .ToListAsync();
            if (persons.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(persons);
            }
        }

        /// <summary>
        /// GET api/person?skip=X&take=Y
        /// Example API method that returns a paginated list of a subset of Person entities in the database.
        /// </summary>
        /// <param name="skip">Number of Person entries to skip.</param>
        /// <param name="take">Number of Person entries to take.</param>
        /// <returns>
        /// 200 OK unless there are no valid Person entries, in which case 404 Not Found.
        /// </returns>
        [HttpGet("{skip, take}")]
        [AddHateoasLinks(typeof(ExamplePropertySetPerson))]
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

        /// <summary>
        /// GET api/person/5
        /// Example API method that returns a single Person entity based on its ID.
        /// </summary>
        /// <param name="id">The person ID in the database.</param>
        /// <returns>
        /// 200 OK unless there is no Person entry meeting the ID, in which case 404 Not Found.
        /// </returns>
        [HttpGet("{id}")]
        [AddHateoasLinks(new[] {
            typeof(ExamplePropertySetSelf),
            typeof(ExamplePropertySetPerson),
            typeof(ExamplePropertySetCountry),
            typeof(ExamplePropertySetCity),
            typeof(ExamplePropertySetInvalid),
        })]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context
                .Persons
                .Include(p => p.Country)
                .Include(p => p.Country.Capital)
                .FirstOrDefaultAsync<Person>( p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(person);
            }
        }

        /// <summary>
        /// POST api/person
        /// Example API method that allows posting a Person entity into the database.
        /// </summary>
        /// <param name="person">A valid Person record.</param>
        /// <returns>
        /// 201 Created with location header containing GET link to newly created object in database.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Add<Person>(person);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPerson), new { person.Id }, person);
        }

        /// <summary>
        /// PUT api/person/5
        /// Example API method that allows putting a Person entity into the database.
        /// </summary>
        /// <param name="id">The ID of the Person record to update.</param>
        /// <param name="person">The Person record to replace it with.</param>
        /// <returns>
        /// 204 No Content, or 201 Created if the ID doesn't already exist, or 404 Not Found if ID mismatch.
        /// </returns>
        /// <remarks>
        /// In other systems, may return 200 OK, or 400 Bad Request if ID is invalid.
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (_context.Find<Person>(id) != null)
            {
                _context.Update<Person>(person);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                _context.Add<Person>(person);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPerson), new { person.Id }, person);
            }
        }

        /// <summary>
        /// DELETE api/person/5
        /// Example API method that allows deleting a Person entity based on its ID.
        /// </summary>
        /// <param name="id">ID of the Person to delete.</param>
        /// <returns>
        /// 200 OK, or 404 Not Found if ID doesn't exist in the database.
        /// </returns>
        /// <remarks>
        /// In other systems, may return 202 Accepted or 204 No Content.
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            Person person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Seeds the database with Person entities at various stages of data integrity
        /// </summary>
        private void SeedDatabase()
        {
            if (_context.Persons.Count() == 0)
            {
                _context.Add(new Person()
                {
                    FirstName = "Maximilian",
                    LastName = "Coutuer",
                    Age = 35,
                    Country = new Country()
                    {
                        Name = "Belgium",
                        Capital = new City()
                        {
                            Name = "Brussels",
                            Population = 1199000,
                            HistoricName = "Bruoxala"
                        },
                        Population = 11350000,
                    },
                });
                _context.Add(new Person()
                {
                    FirstName = "Guðni",
                    LastName = "Jóhannesson",
                    Age = 51,
                    Country = new Country()
                    {
                        Name = "Iceland",
                        Capital = new City()
                        {
                            Name = "Reykjavik",
                            Population = 112853,
                        },
                        Population = 338349,
                    },
                });
                _context.Add(new Person()
                {
                    FirstName = "Xi",
                    LastName = "Jinping",
                    Country = null,
                });
                _context.Add(new Person()
                {
                    FirstName = "Halimah",
                    LastName = "Yacob",
                    Country = new Country()
                    {
                        Name = "Singapore",
                        Population = 5612000,
                        Capital = null,
                    },
                });
                _context.SaveChanges();
            }
        }

        #endregion
    }
}

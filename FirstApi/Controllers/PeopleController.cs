using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private static readonly List<Person> _people = new();
    private readonly GenderizeService _genderizeService;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(ILogger<PeopleController> logger, GenderizeService genderizeService)
    {
        _logger = logger;
        _genderizeService = genderizeService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_people);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.Name)) return BadRequest("Name cannot be empty");

        var gender = await _genderizeService.GetGender(person.Name);

        if (gender == null) return BadRequest("Failed to determine gender");

        person.Gender = gender;
        person.Id = _people.Count + 1;
        _people.Add(person);

        return Ok(person);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Person updatedPerson)
    {
        var person = _people.FirstOrDefault(p => p.Id == id);

        if (person == null) return NotFound();

        if (string.IsNullOrWhiteSpace(updatedPerson.Name)) return BadRequest("Name cannot be empty");

        var gender = await _genderizeService.GetGender(updatedPerson.Name);

        if (gender == null) return BadRequest("Failed to determine gender");

        person.Name = updatedPerson.Name;
        person.Gender = gender;

        return Ok(person);
    }
}
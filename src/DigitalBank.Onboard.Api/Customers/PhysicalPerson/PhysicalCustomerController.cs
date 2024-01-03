using Microsoft.AspNetCore.Mvc;
using Customers.PhysicalPerson;

namespace digitalbank.onboard.api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("onboard/customers/physical-person")]
public class PhysicalCustomerController : ControllerBase
{
    private readonly ILogger<PhysicalCustomerController> _logger;

    public PhysicalCustomerController(ILogger<PhysicalCustomerController> logger)
    {
        _logger = logger; 
    }

    public static List<PhysicalCustomerResponse> customers = new List<PhysicalCustomerResponse>()
    {
            new PhysicalCustomerResponse("John", "Doe", "03855312747", new DateTime(1980, 1, 1), "johndoe@gmail.com", "123456789", "Rua 1", "São Paulo", "SP", "123456", "Brasil"),
            new PhysicalCustomerResponse("Mary", "Doe", "03855313747", new DateTime(1980, 1, 1), "marydoe@gmail.com", "123456789", "Rua 1", "São Paulo", "SP", "123456", "Brasil")
    };

    [HttpGet]
    [Route("{cpf}")]    
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    public ActionResult<PhysicalCustomerResponse> Get(string cpf)
    {    
        var customer = customers.FirstOrDefault(x => x.Document == cpf);
        if (customer != null)
        {
            return customer;
        }
        else
        {
            return NotFound("Customer not found by document provided.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]   
    public IActionResult Post([FromBody] PhysicalCustomerRequest customerRequest)
    {
        if (!ModelState.IsValid || customerRequest == null)
        {
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Get), new { cpf = 12345678 }, customerRequest);
    }

    [HttpPut]
    [Route("{cpf}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Put(string cpf, [FromBody] PhysicalCustomerRequest customerRequest)
    {
        return NoContent();
    }
}


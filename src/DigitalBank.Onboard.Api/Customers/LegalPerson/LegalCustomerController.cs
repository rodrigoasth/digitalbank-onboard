using Microsoft.AspNetCore.Mvc;
using Customers.LegalPerson;

namespace digitalbank.onboard.api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("onboard/customers/legal-person")]
public class LegalCustomerController : ControllerBase
{
    private readonly ILogger<LegalCustomerController> _logger;

    public LegalCustomerController(ILogger<LegalCustomerController> logger)
    {
        _logger = logger;
    }

    public static List<LegalCustomerResponse> customers = new List<LegalCustomerResponse>()
    {        
            new LegalCustomerResponse("Americanas", "00776574000156", "johndoe@gmail.com", "123456789", "Rua 1", "São Paulo", "SP", "123456", "Brasil"),
            new LegalCustomerResponse("Rede Economia", " 33762676000171", "marydoe@gmail.com", "123456789", "Rua 1", "São Paulo", "SP", "123456", "Brasil")
    };

    [HttpGet]
    [Route("{cnpj}")]    
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    public ActionResult<LegalCustomerResponse> Get(string cnpj)
    {    
        var customer = customers.FirstOrDefault(x => x.Document == cnpj);
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
    public IActionResult Post([FromBody] LegalCustomerRequest customerRequest)
    {
        return CreatedAtAction(nameof(Get), new { cpf = 12345678 }, customerRequest);
    }

    [HttpPut]
    [Route("{cnpj}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(string cnpj, [FromBody] LegalCustomerRequest customerRequest)
    {
        return NoContent();
    }
    
}


using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
    
    [ApiController]
    [Route("ecommerce/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService service, ILogger<CompanyController> logger)
        {
            _service = service;
            _logger= logger;
        }

        // GET: ecommerce/Company/allcompany?pageNumber=1&pageSize=10
        [Authorize]
        [HttpGet("allcompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = _service.GetAll(pageNumber, pageSize);
            _logger.LogInformation("Successfully retrieved companies for pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
            return Ok(result);
            
        }
        // GET: ecommerce/company/search?searchTerm=apple&pageNumber=1&pageSize=10
        [Authorize]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SearchCompanies([FromQuery] string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
                
                    var result = _service.Search(searchTerm, pageNumber, pageSize);
                    _logger.LogInformation("Successfully retrieved companies matching search term: SearchTerm='{SearchTerm}', PageNumber={PageNumber}, PageSize={PageSize}", searchTerm, pageNumber, pageSize);
                    return Ok(result);
               
        }
        // GET: ecommerce/company/searchbylocation?location=NYC&pageNumber=1&pageSize=10
        [Authorize]
        [HttpGet("searchbylocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SearchByLocation([FromQuery] string location, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
                var result = _service.SearchByLocation(location, pageNumber, pageSize);
                _logger.LogInformation("Successfully retrieved companies by location: Location='{Location}', PageNumber={PageNumber}, PageSize={PageSize}", location, pageNumber, pageSize);
                return Ok(result);
                
        }

        // GET: ecommerce/company/bycompanyid/{id}
        [Authorize]
        [HttpGet("bycompanyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CompanyDTO> GetCompanyById(int id)
        {
                var result = _service.GetById(id);
                _logger.LogInformation("Successfully retrieved company by id: Id={Id}", id);
                return Ok(result);
            
        }
        // GET: ecommerce/company/bycompanyname/{name}
        [Authorize]
        [HttpGet("bycompanyname/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CompanyDTO> GetCompanyByName(string name)
        {
            
                var company = _service.GetByName(name);
                _logger.LogInformation("Successfully retrieved company by name: Name='{Name}'", name);
                return Ok(company);
            
        }

        // POST: ecommerce/company/createcompany
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createcompany")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<Company> PostCompany(Company company)
        {
            
                var c = _service.Create(company);
                _logger.LogInformation("Successfully created company: Id={Id}, Name='{Name}'", c.CompanyId, c.CompanyName);
                return CreatedAtAction(nameof(GetCompanyById),new { id = c.CompanyId },
c);

        }

        // PATCH: ecommerce/company/updatecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<CompanyPatchDTO> PatchCompany(int id, CompanyPatchDTO company)
        {

            var existingCompany = _service.Patch(id, company);
            _logger.LogInformation("Successfully patched company: Id={Id}", id);
            return Ok(existingCompany);
        }
        //Put: ecommerce/company/companyupdate/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPut("companyupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<Company> PutCompany(int id, Company company)
        {
                var existingCompany =_service.Update(id,company);
               
                _logger.LogInformation("Successfully updated company: Id={Id}", id);
                return Ok(existingCompany);
            
        }
        //DELETE: ecommerce/company/deletecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deletecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult DeleteCompany(int id)
        {
                
                _logger.LogInformation("Successfully deleted company: Id={Id}", id);
                _service.Delete(id);

                return NoContent();
            
        }


    }
}


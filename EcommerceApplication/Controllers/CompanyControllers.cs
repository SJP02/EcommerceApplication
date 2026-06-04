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
            try
            {
                _logger.LogInformation("Getting all companies with pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
                var result = _service.GetAll(pageNumber, pageSize);
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No companies found for pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
                    return NotFound("No Companies to show");
                }
                _logger.LogInformation("Successfully retrieved companies for pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting companies for pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
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
                try
                {
                    _logger.LogInformation("Searching companies with search term: SearchTerm='{SearchTerm}', PageNumber={PageNumber}, PageSize={PageSize}", searchTerm, pageNumber, pageSize);
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        _logger.LogWarning("Search term cannot be empty for company search: SearchTerm='{SearchTerm}'", searchTerm);
                        return BadRequest("Search term cannot be empty.");
                    }
                       
                    var result = _service.Search(searchTerm, pageNumber, pageSize);
                    if (result.Count == 0) {
                        _logger.LogInformation("No companies found matching search term: SearchTerm='{SearchTerm}'", searchTerm);
                        return NotFound($"No companies found matching '{searchTerm}'");
                    }
                     _logger.LogInformation("Successfully retrieved companies matching search term: SearchTerm='{SearchTerm}', PageNumber={PageNumber}, PageSize={PageSize}", searchTerm, pageNumber, pageSize);
                    return Ok(result);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while searching companies: SearchTerm='{SearchTerm}', PageNumber={PageNumber}, PageSize={PageSize}", searchTerm, pageNumber, pageSize);
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                }
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
                try
                {
                    if (string.IsNullOrEmpty(location))
                    {
                        _logger.LogWarning("Location cannot be empty for company search by location: Location='{Location}'", location);
                        return BadRequest("Location cannot be empty.");
                    }

                    var result = _service.SearchByLocation(location, pageNumber, pageSize);
                    if (result.Count == 0)
                        return NotFound($"No companies found in '{location}'");

                    _logger.LogInformation("Successfully retrieved companies by location: Location='{Location}', PageNumber={PageNumber}, PageSize={PageSize}", location, pageNumber, pageSize);
                    return Ok(result);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while searching companies by location: Location='{Location}', PageNumber={PageNumber}, PageSize={PageSize}", location, pageNumber, pageSize);
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
                }
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
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid company id provided: Id={Id}", id);
                    return BadRequest("Id cannot be less than or equal to 0");
                }
                var result = _service.GetById(id);
                if (result == null)
                {
                    _logger.LogWarning("No company found with id: Id={Id}", id);
                    return NotFound($"No company with id {id} found");
                }
                _logger.LogInformation("Successfully retrieved company by id: Id={Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting company by id: Id={Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
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
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning("Company name cannot be null or empty: Name='{Name}'", name);
                    return BadRequest("Name cannot be null or empty.");
                }
                var company = _service.GetByName(name);

                if (company == null)
                {
                    _logger.LogWarning("No company found with name: Name='{Name}'", name);
                    return NotFound($"Company {name} cannot found");
                }
                _logger.LogInformation("Successfully retrieved company by name: Name='{Name}'", name);
                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting company by name: Name='{Name}'", name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // POST: ecommerce/company/createcompany
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createcompany")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Company> PostCompany(Company company)
        {
            try
            {

                if (company == null)
                {
                    _logger.LogWarning("Company object cannot be null for creation.");
                    return BadRequest("Product cannot be null.");
                }
                var c = _service.Create(company);
                _logger.LogInformation("Successfully created company: Id={Id}, Name='{Name}'", c.CompanyId, c.CompanyName);
                return Ok(c);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a company.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // PATCH: ecommerce/company/updatecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CompanyPatchDTO> PatchCompany(int id, CompanyPatchDTO company)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning("Invalid company id provided for patching: Id={Id}", id);
                    return BadRequest();
                }
                var existingCompany = _service.Patch(id, company);
                _logger.LogInformation("Successfully patched company: Id={Id}", id);
                return Ok(existingCompany);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while patching company: Id={Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }
        //Put: ecommerce/company/companyupdate/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPut("companyupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Company> PutCompany(int id, Company company)
        {
            try
            { 
                if ( id < 0)
                {
                        _logger.LogWarning("Invalid company id provided for updating: Id={Id}", id);
                        return BadRequest();
                }
                var existingCompany =_service.Update(id,company);
                if (existingCompany == null)
                {
                        _logger.LogWarning("No company found with id for updating: Id={Id}", id);
                        return NotFound();
                }
                _logger.LogInformation("Successfully updated company: Id={Id}", id);
                return Ok(existingCompany);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating company: Id={Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            
        }
        //DELETE: ecommerce/company/deletecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deletecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCompany(int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning("Invalid company id provided for deletion: Id={Id}", id);
                    return BadRequest("Id cannot be 0.");
                }
                var company = _service.GetById(id);
                if (company == null)
                {
                    _logger.LogWarning("No company found with id for deletion: Id={Id}", id);
                    return NotFound();
                }
                _logger.LogInformation("Successfully deleted company: Id={Id}", id);
                _service.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting company: Id={Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


    }
}


using EcommerceApplication.Data;
using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ecommerce/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        // GET: ecommerce/Company/allcompany?pageNumber=1&pageSize=10
        [AllowAnonymous]
        [HttpGet("allcompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = _service.GetAll(pageNumber,pageSize);
            if (result == null)
                return NotFound("No Companies to show");
            return Ok(result);
        }
        // GET: ecommerce/company/search?searchTerm=apple&pageNumber=1&pageSize=10
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchCompanies([FromQuery] string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("Search term cannot be empty.");

            var result = _service.Search(searchTerm, pageNumber, pageSize);
            if (result.Count == 0)
                return NotFound($"No companies found matching '{searchTerm}'");

            return Ok(result);
        }
        // GET: ecommerce/company/searchbylocation?location=NYC&pageNumber=1&pageSize=10
        [AllowAnonymous]
        [HttpGet("searchbylocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchByLocation([FromQuery] string location, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(location))
                return BadRequest("Location cannot be empty.");

            var result = _service.SearchByLocation(location, pageNumber, pageSize);
            if (result.Count == 0)
                return NotFound($"No companies found in '{location}'");

            return Ok(result);
        }

        // GET: ecommerce/company/bycompanyid/{id}
        [AllowAnonymous]
        [HttpGet("bycompanyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CompanyDTO> GetCompanyById(int id)
        {
            if (id <= 0)
                return BadRequest("Id cannot be less than or equal to 0");
            var result= _service.GetById(id);
            if (result == null)
                return NotFound($"No company with id {id} found");
            return Ok(result);
        }
        // GET: ecommerce/company/bycompanyname/{name}
        [AllowAnonymous]
        [HttpGet("bycompanyname/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CompanyDTO> GetCompanyByName(string name)
        {
            if (name.Length == 0)
            {
                return BadRequest("Name cannot be null.");
            }
            var company = _service.GetByName(name);

            if (company == null)
            {
                return NotFound($"Company {name} cannot found");
            }
            return Ok(company);
        }

        // POST: ecommerce/company/createcompany
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createcompany")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Company> PostCompany(Company company)
        {
            if (company == null)
            {
                return BadRequest("Product cannot be null.");
            }
            var c = _service.Create(company);

            return Ok(c);
        }

        // PATCH: ecommerce/company/updatecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CompanyPatchDTO> PatchCompany(int id, CompanyPatchDTO company)
        {
            if ( id < 0)
            {
                return BadRequest();
            }
            var existingCompany = _service.Patch(id,company);
            return Ok(existingCompany);
        }
        //Put: ecommerce/company/companyupdate/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPut("companyupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Company> PutCompany(int id, Company company)
        {
            if ( id < 0)
            {
                return BadRequest();
            }
            var existingCompany =_service.Update(id,company);
            if (existingCompany == null)
            {
                return NotFound();
            }
            
            return Ok(existingCompany);
        }
        //DELETE: ecommerce/company/deletecompany/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deletecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteCompany(int id)
        {
            if (id < 0)
            {
                return BadRequest("Id cannot be 0.");
            }
             var company=_service.GetById(id);
            if (company == null)
            {
                return NotFound();
            }

           _service.Delete(id);

            return NoContent();
        }


    }
}


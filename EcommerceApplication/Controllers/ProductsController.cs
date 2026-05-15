using EcommerceApplication.Data;
using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ecommerce/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: ecommerce/products/allproduct?pageNumber=1&pageSize=10
        [AllowAnonymous]
        [HttpGet("allproduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProducts([FromQuery] ProductFilterDTO filter)
        {
            var result = _service.GetAll(filter);
            if (result == null||!result.Any())
                return NotFound("No Products to show");
            return Ok(result);
        }



        // GET: ecommerce/products/byproductid/5
        [AllowAnonymous]
        [HttpGet("byproductid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> GetProductById(int id)
        {
          
            if (id <= 0)
            {
                return BadRequest("Id cannot be 0.");
            }
            var product =  _service.GetById(id);

            if (product == null)
            {
                return NotFound($"Product with id {id} not found");
            }
            return Ok(product);
        }
        // GET: ecommerce/products/byproductname/{name} 
        [AllowAnonymous]
        [HttpGet("byproductname/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> GetProductByName(string name)
        {
            if (name.Length==0)
            {
                return BadRequest("Name cannot be null.");
            }
            var product = _service.GetByName(name);

            if (product == null)
            {
                return NotFound($"Product {name} cannot found");
            }
            return Ok(product);
        }

        // POST: ecommerce/products/createproduct
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createproduct")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PostProduct(Product product)
        {
            
            if (product == null||product.CompanyId ==0)
            {
                return BadRequest("Product cannot be null.");
            }
            
            var p = _service.Create(product);
            if (p!=null)
            {
                return Ok(p);
            }
            else
                return BadRequest("Product not present");
        }


        // PATCH: ecommerce/products/updatecompany/5
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> PatchProduct(int id, ProductPatchDTO product)
        {
            if ( id < 0)
            {
                return BadRequest();
            }
            var existingProduct = _service.Patch(id,product);
            if(existingProduct == null) 
            {
                return NotFound();
            }
            
            return Ok(existingProduct);
        }
        //Put: ecommerce/products/productupdate/5
        [Authorize(Roles = "ADMIN")]
        [HttpPut("productupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> PutProduct(int id, ProductDTO product)
        {
            if ( id < 0)
            {
                return BadRequest();
            }
            var existingProduct = _service.Update(id, product);
            if(existingProduct == null)
            {
                return NotFound();
            }
            return Ok(existingProduct);
        }
        //Get: ecommerce/products/sortedbycompany/5
        [Authorize(Roles = "CUSTOMER")]
        [HttpGet("sortedbycompany/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> GetProductsByCompany(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var existingProduct = _service.GetAllProductsByCompanyId(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            return Ok(existingProduct);
        }
        //DELETE: ecommerce/products/5
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deleteproduct/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteProduct(int id)
        {
            if (id < 0)
            {
                return BadRequest("Id cannot be 0.");
            }
            var product = _service.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

           _service.Delete(id);

            return NoContent();
        }

        
    }
}

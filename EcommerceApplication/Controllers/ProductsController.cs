using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
   
    [ApiController]
    [Route("ecommerce/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: ecommerce/products/allproduct?pageNumber=1&pageSize=10
        [Authorize]
        [HttpGet("allproduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProducts([FromQuery] ProductFilterDTO filter)
        {
                 var result = _service.GetAll(filter);
                _logger.LogInformation("Products retrieved successfully with the given filter criteria.");
                return Ok(result);
        }



        // GET: ecommerce/products/byproductid/5
        [Authorize]
        [HttpGet("byproductid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ProductDTO> GetProductById(int id)
        {
            
                var product = _service.GetById(id);
                _logger.LogInformation("Product with id {ProductId} retrieved successfully.", id);
                return Ok(product);
            
        }
        // GET: ecommerce/products/byproductname/{name} 
        [Authorize]
        [HttpGet("byproductname/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ProductDTO> GetProductByName(string name)
        {
            
                var product = _service.GetByName(name);
                _logger.LogInformation("Product with name {ProductName} retrieved successfully.", name);
                return Ok(product);
            
        }

        // POST: ecommerce/products/createproduct
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createproduct")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<Product> PostProduct(Product product)
        {
           
                var p = _service.Create(product);
                _logger.LogInformation("Product created successfully with id {ProductId}.", p.Id);
                return Ok(p);
                
        }


        // PATCH: ecommerce/products/updatecompany/5
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ProductDTO> PatchProduct(int id, ProductPatchDTO product)
        {
            
                var existingProduct = _service.Patch(id, product);
                _logger.LogInformation("Product with id {ProductId} patched successfully.", id);
                return Ok(existingProduct);
            
        }
        //Put: ecommerce/products/productupdate/5
        [Authorize(Roles = "ADMIN")]
        [HttpPut("productupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<ProductDTO> PutProduct(int id, ProductDTO product)
        {
           
                var existingProduct = _service.Update(id, product);
                _logger.LogInformation("Product with id {ProductId} updated successfully.", id);
                return Ok(existingProduct);
        }
        //Get: ecommerce/products/sortedbycompany/5
        [Authorize]
        [HttpGet("sortedbycompany/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ProductDTO> GetProductsByCompany(int id)
        {
            
                var existingProduct = _service.GetAllProductsByCompanyId(id);
                _logger.LogInformation("Products for company with id {CompanyId} retrieved successfully.", id);
                return Ok(existingProduct);
            
        }
        //DELETE: ecommerce/products/5
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deleteproduct/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult DeleteProduct(int id)
        {
           
                
                _logger.LogInformation("Product with id {ProductId} found for deletion.", id);
                _service.Delete(id);
                return NoContent();
            
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("upload-image/{productId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UploadImage(int productId, IFormFile file)
        {
            
                var result = await _service.UploadProductImage(productId, file);
                return Ok(new
                {
                    message = "Image uploaded successfully",
                    imageUrl = result
                });
            
        }

    }
}

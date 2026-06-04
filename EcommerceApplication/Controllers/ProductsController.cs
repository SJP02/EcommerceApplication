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
            try
            {
                var result = _service.GetAll(filter);
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No products found with the given filter criteria.");
                    return NotFound("No Products to show");
                }
                _logger.LogInformation("Products retrieved successfully with the given filter criteria.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products with the given filter criteria.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
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
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid product id: {ProductId}. Id must be greater than 0.", id);
                    return BadRequest("Id cannot be 0.");
                }
                var product = _service.GetById(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with id {ProductId} not found.", id);
                    return NotFound($"Product with id {id} not found");
                }
                _logger.LogInformation("Product with id {ProductId} retrieved successfully.", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product with id {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
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
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning("Product name cannot be null or empty.");
                    return BadRequest("Name cannot be null.");
                }
                var product = _service.GetByName(name);

                if (product == null)
                {
                    _logger.LogWarning("Product with name {ProductName} not found.", name);
                    return NotFound($"Product {name} cannot found");
                }
                _logger.LogInformation("Product with name {ProductName} retrieved successfully.", name);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product with name {ProductName}.", name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // POST: ecommerce/products/createproduct
        [Authorize(Roles = "ADMIN")]
        [HttpPost("createproduct")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Product> PostProduct(Product product)
        {
            try
            {
                if (product == null || product.CompanyId == 0)
                {
                    _logger.LogWarning("Product cannot be null and CompanyId must be provided.");
                    return BadRequest("Product cannot be null.");
                }

                var p = _service.Create(product);
                if (p != null)
                {
                    _logger.LogInformation("Product created successfully with id {ProductId}.", p.Id);
                    return Ok(p);
                }
                else
                {
                    _logger.LogWarning("Failed to create product. Please check the provided data.");
                    return BadRequest("Product couldnot be created.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        // PATCH: ecommerce/products/updatecompany/5
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("updatecompany/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> PatchProduct(int id, ProductPatchDTO product)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid product id: {ProductId}. Id must be greater than 0.", id);
                    return BadRequest();
                }
                var existingProduct = _service.Patch(id, product);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with id {ProductId} not found for patching.", id);
                    return NotFound();
                }
                _logger.LogInformation("Product with id {ProductId} patched successfully.", id);
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while patching product with id {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }
        //Put: ecommerce/products/productupdate/5
        [Authorize(Roles = "ADMIN")]
        [HttpPut("productupdate/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> PutProduct(int id, ProductDTO product)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid product id: {ProductId}. Id must be greater than 0.", id);
                    return BadRequest();
                }
                var existingProduct = _service.Update(id, product);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with id {ProductId} not found for updating.", id);
                    return NotFound();
                }
                _logger.LogInformation("Product with id {ProductId} updated successfully.", id);
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with id {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
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
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid company id: {CompanyId}. Id must be greater than 0.", id);
                    return BadRequest();
                }
                var existingProduct = _service.GetAllProductsByCompanyId(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("No products found for company with id {CompanyId}.", id);
                    return NotFound();
                }
                _logger.LogInformation("Products for company with id {CompanyId} retrieved successfully.", id);
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products for company with id {CompanyId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        //DELETE: ecommerce/products/5
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deleteproduct/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid product id: {ProductId}. Id must be greater than 0.", id);
                    return BadRequest("Id cannot be 0.");
                }
                var product = _service.GetById(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with id {ProductId} not found for deletion.", id);
                    return NotFound();
                }
                _logger.LogInformation("Product with id {ProductId} found for deletion.", id);
                _service.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with id {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");

            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("upload-image/{productId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage(int productId, IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    _logger.LogWarning("No file provided for product {ProductId}", productId);
                    return BadRequest("File is required");
                }

                var result = await _service.UploadProductImage(productId, file);

                if (result == null)
                {
                    return BadRequest("Upload failed");
                }

                return Ok(new
                {
                    message = "Image uploaded successfully",
                    imageUrl = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for product {ProductId}", productId);
                return StatusCode(500, ex.ToString());
            }
        }

    }
}

using EcommerceApplication.Controllers;
using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;//used as APIController, ControllerBase classes are used in main project
using Moq;
using Xunit;
namespace EcommerceTest.ControllerTest
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _ProductsController;

        public ProductsControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _ProductsController = new ProductsController(_mockProductService.Object);

        }
        [Fact]
        public void GetProduct_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var ProductDto = new ProductDTO();
            var pagedProductDto = new PagedList<ProductDTO>(
                 new List<ProductDTO> { ProductDto },
                 1,
                 1,
                 10
            );
            var filter = new ProductFilterDTO
            {
                Page = 1,
                PageSize = 10
            };
            _mockProductService.Setup(x => x.GetAll(filter))
                        .Returns(pagedProductDto);
            // Act
            var result = _ProductsController.GetProducts(filter);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public void GetProduct_ReturnsNotFound_WhenNoProductExists()
        {
            var filter = new ProductFilterDTO
            {
                Page = 1,
                PageSize = 10
            };

            _mockProductService.Setup(x => x.GetAll(filter))
                        .Returns((PagedList<ProductDTO>)null);
            // Act
            var result = _ProductsController.GetProducts(filter);
            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public void GetProductById_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var ProductDto = new ProductDTO();

            _mockProductService.Setup(x => x.GetById(1))
                        .Returns(ProductDto);

            // Act
            var result = _ProductsController.GetProductById(1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetProductById_ReturnsNotFound_WhenProductDoesNotExists()
        {

            _mockProductService.Setup(x => x.GetById(152))
                        .Returns((ProductDTO)null);

            // Act
            var result = _ProductsController.GetProductById(152);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void GetProductById_ReturnsBadRequest_WhenIdIsInvalid()
        {

            // Act
            var result = _ProductsController.GetProductById(0);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public void GetProductByName_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var ProductDto = new ProductDTO();

            _mockProductService.Setup(x => x.GetByName("Monitors"))
                        .Returns(ProductDto);

            // Act
            var result = _ProductsController.GetProductByName("Monitors");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();//to check if the OK() object is returned 
        }
        [Fact]
        public void GetProductByName_ReturnsNotFound_WhenProductDoesNotExists()
        {

            _mockProductService.Setup(x => x.GetByName("Apple"))
                        .Returns((ProductDTO)null);

            // Act
            var result = _ProductsController.GetProductByName("Apple");

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public void GetProductByName_ReturnsBadRequest_WhenProductNameNotProvided()
        {

            _mockProductService.Setup(x => x.GetByName(""))
                        .Returns((ProductDTO)null);

            // Act
            var result = _ProductsController.GetProductByName("");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public void DeleteProduct_ReturnsOk_WhenProductExists()
        {
            var ProductDto = new ProductDTO();
            _mockProductService.Setup(x => x.GetById(1))
                        .Returns(ProductDto);

            // Act
            var result = _ProductsController.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]//to indicate that this is a test method and should be executed by the test runner
        public void DeleteProduct_ReturnsNotFound_WhenProductDoesNotExists()
        {

            _mockProductService.Setup(x => x.GetById(1))
                        .Returns((ProductDTO)null);

            // Act
            var result = _ProductsController.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void DeleteProduct_ReturnBadRequest_ProductIdIsNull()
        {
            var ProductDto = new ProductDTO();
            var result = _ProductsController.DeleteProduct(-1);
            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public void GetProductsByCompany_ReturnOk_ProductExists()
        {
            var product = new ProductDTO();
            _mockProductService.Setup(x => x.GetAllProductsByCompanyId(1))
                        .Returns(new List<ProductDTO> { product } );
            var result = _ProductsController.GetProductsByCompany(1);
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public void GetProductsByCompany_ReturnsBadRequest_CompanyIdIsNull()
        {
            var result = _ProductsController.GetProductsByCompany(-1);
            result.Result.Should().BeOfType<BadRequestResult>();
        }
        [Fact]
        public void GetProductsByCompany_ReturnsNotFound_NoProductsExists()
        {
            var products = new List<ProductDTO>();
            _mockProductService.Setup(x => x.GetAllProductsByCompanyId(1))
            .Returns((List<ProductDTO>)null);
            var result = _ProductsController.GetProductsByCompany(1);
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void PutProduct_ReturnOk_ProductExists()
        {
            var product = new ProductDTO();
            _mockProductService.Setup(x => x.Update(1, product))
                        .Returns(new Product());
            var result = _ProductsController.PutProduct(1, product);
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public void PutProduct_ReturnNotFound_ProductDoesNotExists()
        {
            var Product = new ProductDTO();
            _mockProductService.Setup(x => x.Update(1, Product))
                        .Returns((Product)null);
            var result = _ProductsController.PutProduct(1, Product);
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void PutProduct_ReturnBadRequest_ProductIdIsNull()
        {
            var Product = new ProductDTO();
            var result = _ProductsController.PutProduct(-1, Product);
            result.Result.Should().BeOfType<BadRequestResult>();

        }
        [Fact]
        public void PatchProduct_ReturnOk_ProductExists()
        {
            var ProductpatchDto = new ProductPatchDTO();
            _mockProductService.Setup(x => x.Patch(1, ProductpatchDto))
                        .Returns(new Product());
            var result = _ProductsController.PatchProduct(1, ProductpatchDto);
            result.Result.Should().BeOfType<OkObjectResult>();

        }
        [Fact]
        public void PatchProduct_ReturnBadRequest_ProductIdInvalid()
        {
            var result = _ProductsController.PatchProduct(-1, new ProductPatchDTO());
            result.Result.Should().BeOfType<BadRequestResult>();
        }
            [Fact]
            public void PostProduct_ReturnCreated_ProductCreated()
            {
                var product = new Product
                {
                    Name = "Test",
                    Price = 100,
                    CompanyId = 1,
                    Description = "Test Product",
                    CategoryId=1,
                    Id=1,
                    Quantity=100
                };
            ;
            _mockProductService.Setup(x => x.Create(It.IsAny<Product>()))
                            .Returns(product);
           var result = _ProductsController.PostProduct(product);
            _mockProductService.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);

            result.Result.Should().BeOfType<OkObjectResult>();
            }
        
        [Fact]
        public void PostProduct_ReturnsBadRequest_WhenProductIsNull()
        {
            // Act
            var result = _ProductsController.PostProduct(null);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

    }
}
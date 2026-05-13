using AutoMapper;
using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Repository.Interfaces;
using EcommerceApplication.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace EcommerceApplication.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();

            _service = new ProductService(
                _mockRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void Create_ReturnsTrue_WhenProductAdded()
        {
            // Arrange
            var product = new Product();

            _mockRepository
                .Setup(x => x.Add(product))
                .Returns(product);

            // Act
            var result = _service.Create(product);

            // Assert
            result.Should().BeOfType<Product>();
        }

        [Fact]
        public void Delete_ReturnsFalse_WhenProductNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns((Product)null);

            // Act
            var result = _service.Delete(1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Delete_ReturnsTrue_WhenProductExists()
        {
            // Arrange
            var product = new Product();

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns(product);

            // Act
            var result = _service.Delete(1);

            // Assert
            result.Should().BeTrue();

            _mockRepository.Verify(x => x.Delete(product), Times.Once);//to verify that the delete method  was called exactly once
        }

        [Fact]
        public void GetById_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns((Product)null);

            // Act
            var result = _service.GetById(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetById_ReturnsProductDTO_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Laptop"
            };

            var productDto = new ProductDTO
            {
                Name = "Laptop"
            };

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns(product);

            _mockMapper
                .Setup(x => x.Map<ProductDTO>(product))
                .Returns(productDto);

            // Act
            var result = _service.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Laptop");//! is to tell compiler that the reult will not be null, its like giving a guaratee 
        }
        [Fact]
        public void GetByName_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(x => x.GetByName("Phone"))
                .Returns((Product)null);

            // Act
            var result = _service.GetByName("Phone");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByName_ReturnsProductDTO_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Phone"
            };

            var productDto = new ProductDTO
            {
                Name = "Phone"
            };

            _mockRepository
                .Setup(x => x.GetByName("Phone"))
                .Returns(product);

            _mockMapper
                .Setup(x => x.Map<ProductDTO>(product))
                .Returns(productDto);

            // Act
            var result = _service.GetByName("Phone");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Phone");
        }

        [Fact]
        public void GetAllProductsByCompanyId_ReturnsEmptyList_WhenNoProducts()
        {
            // Arrange
            _mockRepository
                .Setup(x => x.GetProductByCompanyId(1))
                .Returns(new List<Product>());

            // Act
            var result = _service.GetAllProductsByCompanyId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAllProductsByCompanyId_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop" }
            };

            var productDtos = new List<ProductDTO>
            {
                new ProductDTO {  Name = "Laptop" }
            };

            _mockRepository
                .Setup(x => x.GetProductByCompanyId(1))
                .Returns(products);

            _mockMapper
                .Setup(x => x.Map<List<ProductDTO>>(products))
                .Returns(productDtos);

            // Act
            var result = _service.GetAllProductsByCompanyId(1);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void Patch_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            var patchDto = new ProductPatchDTO();

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns((Product)null);

            // Act
            var result = _service.Patch(1, patchDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Patch_UpdatesProduct_WhenProductExists()
        {
            // Arrange
            var existing = new Product
            {
                Id = 1,
                Name = "Old",
                Price = 100
            };

            var patchDto = new ProductPatchDTO
            {
                Name = "New",
                Price = 200
            };

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns(existing);

            // Act
            var result = _service.Patch(1, patchDto);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("New");
            result.Price.Should().Be(200);

            _mockRepository.Verify(x => x.Update(existing), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            var productDto = new ProductDTO();

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns((Product)null);

            // Act
            var result = _service.Update(1, productDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Update_UpdatesProduct_WhenProductExists()
        {
            // Arrange
            var existing = new Product
            {
                Id = 1,
                Name = "Old"
            };

            var dto = new ProductDTO
            {
                Name = "Updated",
                Description = "Updated Desc",
                Price = 500
            };

            _mockRepository
                .Setup(x => x.GetById(1))
                .Returns(existing);

            // Act
            var result = _service.Update(1, dto);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Updated");
            result.Description.Should().Be("Updated Desc");
            result.Price.Should().Be(500);

            _mockRepository.Verify(x => x.Update(existing), Times.Once);
        }

        [Fact]
        public void GetAll_ReturnsPagedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "Gaming Laptop",
                    Price = 1000,
                    Quantity = 5
                }
            }.AsQueryable();

            var productDtos = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Name = "Laptop"
                }
            };

            _mockRepository
                .Setup(x => x.GetAllProducts())
                .Returns(products);

            _mockMapper
                .Setup(x => x.Map<List<ProductDTO>>(It.IsAny<List<Product>>()))//It is a helper class present in Moq,a nd it is used to matchany value of type List<Product>
                .Returns(productDtos);

            var filter = new ProductFilterDTO
            {
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = _service.GetAll(filter);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
        }
    }
}
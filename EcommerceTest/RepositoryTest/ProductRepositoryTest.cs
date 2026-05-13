using EcommerceApplication.Data;
using EcommerceApplication.Models;
using EcommerceApplication.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;
using FluentAssertions;

public class ProductRepositoryTests
{
    private EcommerceContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<EcommerceContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;//to specify to use a fake db with unique new name each time the test runs

        return new EcommerceContext(options);
    }
    [Fact]
    public void Add_ShouldInsertProduct()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Categories.AddRange(
           new Category
           {
               CategoryId = 1,
               CategoryName = "Test Category"
           });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            });
        context.SaveChanges();
        var product = new Product
        {
            Id = 1,
            Name = "Bottle",
            Description = "it is a sample bottle",
            Price=150,
            Quantity=100,
            CategoryId=1,
            CompanyId=1
        };

        var result=repo.Add(product);   
        result.Should().BeOfType<Product>();
        context.Products.Count().Should().Be(1);
    }
    [Fact]
    public void Delete_ShouldDeleteProduct()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Categories.AddRange(
           new Category
           {
               CategoryId = 1,
               CategoryName = "Test Category"
           },
           new Category
           {
               CategoryId = 2,
               CategoryName = "Test Category 2"
           });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            },
            new Company
            {
                CompanyId = 2,
                CompanyName = "Test Company 2",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "mumbai",
                EstablishedYear = 1994
            });
        var product = new Product
        {
            Id = 1,
            Name = "Bottle",
            Description = "it is a sample bottle",
            Price = 150,
            Quantity = 100,
            CategoryId = 1,
            CompanyId = 1
        };
        context.Products.Add(product);
        context.SaveChanges();

        repo.Delete(product);

        context.Products.Count().Should().Be(0);
    }
    [Fact]
    public void GetAllProducts_ShouldReturnProducts()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            },
            new Company
            {
                CompanyId = 2,
                CompanyName = "Test Company 2",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "Mumbai",
                EstablishedYear = 1994
            });
        context.Categories.AddRange(
            new Category
            {
                CategoryId = 1,
                CategoryName = "Test Category"
            },
            new Category
            {
                CategoryId = 2,
                CategoryName = "Test Category 2"
            });
        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Bottle",
                Description = "it is a sample bottle",
                Price = 150,
                Quantity = 100,
                CategoryId = 1,
                CompanyId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Phones",
                Description = "it is a sample phone",
                Price = 15000,
                Quantity = 20,
                CategoryId = 2,
                CompanyId = 2
            }
        );
        context.SaveChanges();

        var result = repo.GetAllProducts();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<Product>();
    }
    [Fact]
    public void GetByName_ShouldReturnMatchinProducts()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Categories.AddRange(
           new Category
           {
               CategoryId = 1,
               CategoryName = "Test Category"
           },
           new Category
           {
               CategoryId = 2,
               CategoryName = "Test Category 2"
           });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            },
            new Company
            {
                CompanyId = 2,
                CompanyName = "Test Company 2",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "Mumbai",
                EstablishedYear = 1994
            });

        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Bottle",
                Description = "it is a sample bottle",
                Price = 150,
                Quantity = 100,
                CategoryId = 1,
                CompanyId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Phones",
                Description = "it is a sample phone",
                Price = 15000,
                Quantity = 20,
                CategoryId = 2,
                CompanyId = 2
            }
        );
        context.SaveChanges();

        var result = repo.GetByName("Phones");

        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
    }
    [Fact]
    public void GetById_ShouldReturnMatchingProducts()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
         
        context.Categories.AddRange(
            new Category
            {
                CategoryId = 1,
                CategoryName = "Test Category"
            },
            new Category
            {
                CategoryId = 2,
                CategoryName = "Test Category 2"
            });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            },
            new Company
            {
                CompanyId = 2,
                CompanyName = "Test Company 2",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "Mumbai",
                EstablishedYear=1994

            });

        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Bottle",
                Description = "it is a sample bottle",
                Price = 150,
                Quantity = 100,
                CategoryId = 1,
                CompanyId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Phones",
                Description = "it is a sample phone",
                Price = 15000,
                Quantity = 20,
                CategoryId = 2,
                CompanyId = 2
            }
        );
        context.SaveChanges();

        var result = repo.GetById(1);

        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Bottle");
    }
    [Fact]
    public void GetProductByCompanyId_ShouldReturnMatchingProducts()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Categories.AddRange(
           new Category
           {
               CategoryId = 1,
               CategoryName = "Test Category"
           },
           new Category
           {
               CategoryId = 2,
               CategoryName = "Test Category 2"
           });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 1",
                Email = "testcompany1@gmail.com",
                Location = "Delhi",
                EstablishedYear = 1995
            },
            new Company
            {
                CompanyId = 2,
                CompanyName = "Test Company 2",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "Mumbai",
                EstablishedYear = 1994
            });
        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Bottle",
                Description = "it is a sample bottle",
                Price = 150,
                Quantity = 100,
                CategoryId = 1,
                CompanyId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Phones",
                Description = "it is a sample phone",
                Price = 15000,
                Quantity = 20,
                CategoryId = 2,
                CompanyId = 2
            }
        );
        context.SaveChanges();

        var result = repo.GetProductByCompanyId(1);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeOfType<List<Product>>();
        result.First().CompanyId.Should().Be(1);
        result.First().Name.Should().Be("Bottle");
    }
    [Fact]
    public void Update_ShouldUpdateProduct()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        context.Categories.AddRange(
           new Category
           {
               CategoryId = 1,
               CategoryName = "Test Category"
           });
        context.Companys.AddRange(
            new Company
            {
                CompanyId = 1,
                CompanyName = "Test Company",
                Description = "it is a sample Test Company 2",
                Email = "testcompany2@gmail.com",
                Location = "Mumbai",
                EstablishedYear = 1994
            });
        context.Products.AddRange(
            new Product
            {
                Id = 1,
                Name = "Bottle",
                Description = "it is a sample bottle",
                Price = 150,
                Quantity = 100,
                CategoryId = 1,
                CompanyId = 1
            }
        );
        context.SaveChanges();
        var product = context.Products.First(x => x.Id == 1);

        product.Name = "Steel Bottle";
        product.Description = "it is a sample steel bottle ";
        repo.Update
            (product);

        var result = context.Products.First(x=>x.Id==1);

        result.Name.Should().Be("Steel Bottle");
        result.Description.Should().Be("it is a sample steel bottle ");
    }

}

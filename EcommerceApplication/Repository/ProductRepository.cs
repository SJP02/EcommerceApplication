using EcommerceApplication.Data;
using EcommerceApplication.Models;
using EcommerceApplication.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceContext _context;

        public ProductRepository(EcommerceContext context)
        {
            _context = context;
        }
        public Product Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }


        public IQueryable<Product> GetAllProducts()//Iqueryabe used so that the caller can further filter or paginate the results without executing the query immediately
        {
            return _context.Products
                .Include(p => p.ProductCompany)
                .Include(p => p.ProductCategory)
                .AsQueryable();
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        //public PagedList<Product> GetProducts(RequestParameters parameters)
        //{
        //    var products = _context.Products
        //        .Include(p => p.ProductCompany)
        //        .Include(p => p.ProductCategory)
        //        .AsQueryable();
        //    return PagedList<Product>.ToPagedList(products, parameters.PageNumber, parameters.PageSize);
        //}

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.ProductCompany)
                .Include(p => p.ProductCategory)
                .FirstOrDefault(c => c.Id == id);
        }

        public Product GetByName(string name)
        {
            return _context.Products
                .Include(p => p.ProductCompany)
                .Include(p => p.ProductCategory)
                .FirstOrDefault(c => c.Name == name);
        }

        public List<Product> GetProductByCompanyId(int companyId)
        {
            return _context.Products
                .Include(p => p.ProductCompany)
                .Include(p => p.ProductCategory)
                .Where(c => c.CompanyId == companyId)
                .ToList();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}

using EcommerceApplication.Models;

namespace EcommerceApplication.Repository.Interfaces
{
    public interface IProductRepository
    {

        IQueryable<Product> GetAllProducts();
        //PagedList<Product> GetProducts(RequestParameters parameters); 
        Product GetById(int id);
        Product GetByName(string name);
        Product Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetProductByCompanyId(int companyId);
    }
}

using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Pagination;

namespace EcommerceApplication.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        PagedList<Company> GetAll(RequestParameters parameters);
        Company GetById(int id);
        Company GetByName(string name);
        PagedList<Company> Search(string searchTerm, RequestParameters parameters);
        PagedList<Company> SearchByLocation(string location, RequestParameters parameters);
        void Delete(Company company);
        void Update(Company company);
        void Add(Company company);

    }
}

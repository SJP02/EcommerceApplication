using EcommerceApplication.DTO;
using EcommerceApplication.Models;

namespace EcommerceApplication.Services.Interfaces
{
    public interface ICompanyService
    {

        PagedList<CompanyDTO> GetAll(int pageNumber, int pageSize);
        CompanyDTO? GetById(int id);
        CompanyDTO? GetByName(string name);
        PagedList<CompanyDTO> Search(string searchTerm, int pageNumber, int pageSize);
        PagedList<CompanyDTO> SearchByLocation(string location, int pageNumber, int pageSize);
        Company Create(Company company);
        Company? Update(int id, Company company);
        Company? Patch(int id, CompanyPatchDTO company);
        bool Delete(int id);
    }
}

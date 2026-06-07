using EcommerceApplication.Repository.Interfaces;
using EcommerceApplication.Data;
using EcommerceApplication.Models;
using Microsoft.EntityFrameworkCore;
using EcommerceApplication.DTO;
using EcommerceApplication.Pagination;

namespace EcommerceApplication.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        public readonly EcommerceContext _context;
        public CompanyRepository(EcommerceContext context)
        {
            _context = context;
        }

        public void Add(Company company)
        {
            _context.Companys.Add(company);
            _context.SaveChanges();
        }
        public PagedList<Company> Search(string searchTerm, RequestParameters parameters)
        {
            var companies = _context.Companys.Include(c => c.ProductList).ThenInclude(p => p.ProductCategory).Where(c => c.CompanyName.Contains(searchTerm)).AsQueryable();
            return PagedList<Company>.ToPagedList(companies, parameters.PageNumber, parameters.PageSize);
        }
        public PagedList<Company> SearchByLocation(string location, RequestParameters parameters)
        {
            var companies = _context.Companys.Include(c => c.ProductList).ThenInclude(p => p.ProductCategory).Where(c => c.Location.Contains(location)).AsQueryable();
            return PagedList<Company>.ToPagedList(companies, parameters.PageNumber, parameters.PageSize);
        }
        public void Delete(Company company)
        {
            _context.Companys.Remove(company);
            _context.SaveChanges();
        }

        public PagedList<Company> GetAll(RequestParameters parameters)
        {
            var companies = _context.Companys.Include(c=>c.ProductList).ThenInclude(p=>p.ProductCategory).AsQueryable();
            return PagedList<Company>.ToPagedList(companies, parameters.PageNumber, parameters.PageSize);
        }

        public Company GetById(int id)
        {
            return _context.Companys.Include(c => c.ProductList).FirstOrDefault(c=>c.CompanyId==id);
        }

        public Company GetByName(string name)
        {
            return _context.Companys.Include(c => c.ProductList).FirstOrDefault(c=>c.CompanyName==name);
        }

        public void Update(Company company)
        {
            _context.Companys.Update(company);
            _context.SaveChanges();

        }
    }
}

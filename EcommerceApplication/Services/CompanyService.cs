using AutoMapper;
using EcommerceApplication.DTO;
using EcommerceApplication.Exceptions;
using EcommerceApplication.Models;
using EcommerceApplication.Pagination;
using EcommerceApplication.Repository.Interfaces;
using EcommerceApplication.Services.Interfaces;

namespace EcommerceApplication.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(
            ICompanyRepository repository,
            IMapper mapper,
            ILogger<CompanyService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public Company Create(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.CompanyName))
            {
                throw new ArgumentException("Company name is required.");
            }

            _repository.Add(company);

            _logger.LogInformation(
                "Company created: {CompanyName}",
                company.CompanyName);

            return company;
        }

        public void Delete(int id)
        {
            var company = _repository.GetById(id);

            if (company == null)
            {
                _logger.LogWarning(
                    "Delete failed. Company not found. Id={Id}",
                    id);

                throw new NotFoundException(
                    $"Company with id {id} was not found.");
            }

            _repository.Delete(company);

            _logger.LogInformation(
                "Company deleted: {CompanyName}",
                company.CompanyName);
        }

        public PagedList<CompanyDTO> GetAll(
            int pageNumber,
            int pageSize)
        {
            var parameters = new RequestParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var companies = _repository.GetAll(parameters);

            var companiesDTO =
                _mapper.Map<List<CompanyDTO>>(companies);

            return new PagedList<CompanyDTO>(
                companiesDTO,
                companies.TotalCount,
                companies.CurrentPage,
                companies.PageSize);
        }

        public CompanyDTO GetById(int id)
        {
            var company = _repository.GetById(id);

            if (company == null)
            {
                _logger.LogWarning(
                    "Company not found. Id={Id}",
                    id);

                throw new NotFoundException(
                    $"Company with id {id} was not found.");
            }

            return _mapper.Map<CompanyDTO>(company);
        }

        public CompanyDTO GetByName(string name)
        {
            var company = _repository.GetByName(name);

            if (company == null)
            {
                _logger.LogWarning(
                    "Company not found. Name={Name}",
                    name);

                throw new NotFoundException(
                    $"Company '{name}' was not found.");
            }

            return _mapper.Map<CompanyDTO>(company);
        }

        public Company Patch(int id, CompanyPatchDTO company)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
            {
                _logger.LogWarning(
                    "Patch failed. Company not found. Id={Id}",
                    id);

                throw new NotFoundException(
                    $"Company with id {id} was not found.");
            }

            if (!string.IsNullOrWhiteSpace(company.CompanyName))
                existing.CompanyName = company.CompanyName;

            if (!string.IsNullOrWhiteSpace(company.Location))
                existing.Location = company.Location;

            if (!string.IsNullOrWhiteSpace(company.Description))
                existing.Description = company.Description;

            if (company.PhoneNumber.HasValue)
                existing.PhoneNumber = company.PhoneNumber.Value;

            if (!string.IsNullOrWhiteSpace(company.Email))
                existing.Email = company.Email;

            _repository.Update(existing);

            _logger.LogInformation(
                "Company patched. Id={Id}",
                id);

            return existing;
        }

        public PagedList<CompanyDTO> Search(
            string searchTerm,
            int pageNumber,
            int pageSize)
        {
            var parameters = new RequestParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var companies =
                _repository.Search(searchTerm, parameters);

            var companiesDTO =
                _mapper.Map<List<CompanyDTO>>(companies);

            return new PagedList<CompanyDTO>(
                companiesDTO,
                companies.TotalCount,
                companies.CurrentPage,
                companies.PageSize);
        }

        public PagedList<CompanyDTO> SearchByLocation(
            string location,
            int pageNumber,
            int pageSize)
        {
            var parameters = new RequestParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var companies =
                _repository.SearchByLocation(location, parameters);

            var companiesDTO =
                _mapper.Map<List<CompanyDTO>>(companies);

            return new PagedList<CompanyDTO>(
                companiesDTO,
                companies.TotalCount,
                companies.CurrentPage,
                companies.PageSize);
        }

        public Company Update(int id, Company company)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
            {
                _logger.LogWarning(
                    "Update failed. Company not found. Id={Id}",
                    id);

                throw new NotFoundException(
                    $"Company with id {id} was not found.");
            }

            existing.CompanyName = company.CompanyName;
            existing.Description = company.Description;
            existing.Email = company.Email;
            existing.Location = company.Location;
            existing.PhoneNumber = company.PhoneNumber;
            existing.EstablishedYear = company.EstablishedYear;
            existing.ProductList = company.ProductList;

            _repository.Update(existing);

            _logger.LogInformation(
                "Company updated. Id={Id}",
                id);

            return existing;
        }
    }
}
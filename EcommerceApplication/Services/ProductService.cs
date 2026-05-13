using AutoMapper;
using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Repository.Interfaces;
using EcommerceApplication.Services.Interfaces;

namespace EcommerceApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Product Create(Product product)
        {
            _repository.Add(product);
            return product;
        }

        public bool Delete(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return false;

            _repository.Delete(product);
            return true;
        }

        public PagedList<ProductDTO> GetAll(ProductFilterDTO filter)
        {
            if (filter == null)
                filter = new ProductFilterDTO();

            if (filter.Page <= 0)
                filter.Page = 1;

            if (filter.PageSize <= 0)
                filter.PageSize = 10;
            else if (filter.PageSize > 100)
                filter.PageSize = 10;
            
            var query = _repository.GetAllProducts();

            // Apply search filter (by name or description)
            if (!string.IsNullOrEmpty(filter.Search))   
                query = query.Where(p => p.Name.Contains(filter.Search) || p.Description.Contains(filter.Search));

            if (!string.IsNullOrEmpty(filter.CategoryName))
                query = query.Where(p => p.ProductCategory != null &&
                                         p.ProductCategory.CategoryName.Contains(filter.CategoryName));

            // Apply price filters
            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            // Apply quantity filters
            if (filter.MinQuantity.HasValue)
                query = query.Where(p => p.Quantity >= filter.MinQuantity.Value);

            if (filter.MaxQuantity.HasValue)
                query = query.Where(p => p.Quantity <= filter.MaxQuantity.Value);

            // Apply sorting
            var sortedQuery = filter.SortBy?.ToLower() switch
            {
                "price" => filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),

                "name" => filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                "quantity" => filter.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Quantity)
                    : query.OrderBy(p => p.Quantity),

                _ => query.OrderBy(p => p.Id)
            };

            // Get total count before pagination
            var totalCount = sortedQuery.Count();

            
            var products = sortedQuery
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Map to DTOs
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);

            return new PagedList<ProductDTO>(
                productsDTO,
                totalCount,
                filter.Page,
                filter.PageSize
            );
        }

        public List<ProductDTO> GetAllProductsByCompanyId(int id)
        {
            var products = _repository.GetProductByCompanyId(id);
            if (products == null || !products.Any())
                return new List<ProductDTO>();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public ProductDTO? GetById(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product);
        }

        public ProductDTO? GetByName(string name)
        {
            var product = _repository.GetByName(name);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product);
        }

        public Product? Patch(int id, ProductPatchDTO product)
        {
            var existing = _repository.GetById(id);
            if (existing == null) 
                return null;

            if (!string.IsNullOrEmpty(product.Name))
                existing.Name = product.Name;

            if (product.Price.HasValue && product.Price.Value > 0)
                existing.Price = product.Price.Value;

            if (!string.IsNullOrEmpty(product.Description))
                existing.Description = product.Description;

            _repository.Update(existing);
            return existing;
        }

        public Product? Update(int id, ProductDTO product)
        {
            var existing = _repository.GetById(id);
            if (existing == null) 
                return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;

            _repository.Update(existing);
            return existing;
        }
    }
}

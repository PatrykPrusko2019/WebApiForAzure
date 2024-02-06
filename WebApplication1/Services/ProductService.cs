using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicStoreApi.Exceptions;
using System.Linq.Expressions;
using System.Net;
using WebApplication1.DownloadDataFromCSV;
using WebApplication1.DownloadFileCSV;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Models.ShowAll;
using WebApplication1.SetNewRecordsToDatabase;

namespace WebApplication1.Services
{
    public interface IProductService
    {
        void GetCVS(File products);
        List<ProductDto> GetData(string product);
        DetailsProductDto GetDetailsProduct(string SKU);
        void SetDataInDatabase(List<ProductDto> products);
        PageResult<AllProductDto> GetAll(ProductQuery productQuery);

    }
    public class ProductService : IProductService
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;

        public ProductService(ProductsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;

        }

        public void GetCVS(File products)
        {
           Download.DownloadFile(products);
        }

        public List<ProductDto> GetData(string product)
        {
            var results = DownloadData.GetDataFromProductsCSV(product);
            return results;
        }

        public DetailsProductDto GetDetailsProduct(string SKU)
        {
            var records = WebApplication1.File.Products.FirstOrDefault(p => p.SKU == SKU);
            var detailsRecords = mapper.Map<DetailsProductDto>(records);
            return detailsRecords;

        }

        public void SetDataInDatabase(List<ProductDto> products)
        {
            var recordsProducts = mapper.Map<List<Product>>(products);

            IEnumerable<Product> records = recordsProducts.ToList();
            DataSeeder dataSeeder = new DataSeeder(dbContext);
            dataSeeder.SeedProduct(recordsProducts);
        }

        public PageResult<AllProductDto> GetAll(ProductQuery searchQuery)
        {
            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Products == null || dbContext.Products.Count() == 0) throw new BadRequestException("There are no records in the Products table, use the api/file action to set the records in the Products table!!!");
            }
            else
            {
                throw new BadRequestException("No database connection !!!");
            }

            // .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1)) -> 5 * (2 - 1) = 10 -> skip 10 items
            var baseQuery = dbContext.Products
                .Where(p => searchQuery.SearchWord == null || (p.Id.ToString().Contains(searchQuery.SearchWord)));

            if (!string.IsNullOrEmpty(searchQuery.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Product, object>>>
                {
                    { nameof(Product.Id), p => p.Id }
                };

                var selectedColumn = columnsSelectors[searchQuery.SortBy];

                switch (searchQuery.SortDirection)
                {
                    case SortDirection.ASC:
                        baseQuery = baseQuery.OrderBy(selectedColumn);
                        break;
                    case SortDirection.DESC:
                        baseQuery = baseQuery.OrderByDescending(selectedColumn);
                        break;
                }
            }
            
            var products = baseQuery
                .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1))
                .Take(searchQuery.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

           
            if (totalItemsCount <= searchQuery.PageSize * (searchQuery.PageNumber - 1))
            {

                 throw new BadRequestException($"search result items of Products: {totalItemsCount} is too small or equal, because the number of skip: {searchQuery.PageSize * (searchQuery.PageNumber - 1)} "
                                                  + ", change the values in 'PageSize = 5, PageNumber = 1' , to see the result");
            }

            if (products is null || products.Count == 0) throw new NotFoundException(searchQuery.SearchWord == null ? "list of products is empty" : $"searchWord not found: {searchQuery.SearchWord}");
            
            var productsDtos = mapper.Map<List<AllProductDto>>(products);

            var result = new PageResult<AllProductDto>(productsDtos, totalItemsCount, searchQuery.PageSize, searchQuery.PageNumber);

            return result;
        }
    }


}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStoreApi.Exceptions;
using System.Linq.Expressions;
using WebApplication1.DownloadDataFromCSV;
using WebApplication1.DownloadFileCSV;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Models.ShowAll;
using WebApplication1.SetNewRecordsToDatabase;

namespace WebApplication1.Services
{

    public interface IPriceService
    {
        void GetCVS(File prices);
        List<PriceDto> GetData(string price);
        DetailsProductDto GetDetailsPrice(DetailsProductDto productDetails);
        void SetDataInDatabase(List<PriceDto> prices);
        PageResult<AllPriceDto> GetAll(PriceQuery priceQuery);
    }
    public class PriceService : IPriceService
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;

        public PriceService(ProductsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public void GetCVS(File prices)
        {
           Download.DownloadFile(prices);
        }

        public List<PriceDto> GetData(string price)
        {
            var results = DownloadData.GetDataFromPricesCSV(price);
            return results;
        }

        public DetailsProductDto GetDetailsPrice(DetailsProductDto productDetails)
        {
            if (WebApplication1.File.Prices == null || WebApplication1.File.Prices.Count() == 0) return productDetails;
            var priceRecord = WebApplication1.File.Prices.FirstOrDefault(i => i.SKU == productDetails.SKU);
            productDetails.NettProductPrice = (double)priceRecord.NettProductPrice;
            productDetails.NettProductPriceAfterDiscountForProductLogisticUnit = (double)priceRecord.NettProductPriceAfterDiscountForProductLogisticUnit;

            return productDetails;
        }

        public void SetDataInDatabase(List<PriceDto> prices)
        {
            var recordsPrices = mapper.Map<List<Price>>(prices);
            DataSeeder dataSeeder = new DataSeeder(dbContext);
            dataSeeder.SeedPrice(recordsPrices);
        }

        public PageResult<AllPriceDto> GetAll(PriceQuery searchQuery)
        {
            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Prices == null || dbContext.Prices.Count() == 0) throw new BadRequestException("There are no records in the Prices table, use the api/file action to set the records in the Prices table!!!");
            }
            else
            {
                throw new BadRequestException("No database connection !!!");
            }
            
            // .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1)) -> 5 * (2 - 1) = 10 -> skip 10 items
            var baseQuery = dbContext.Prices
                .Where(p => searchQuery.SearchWord == null || (p.Id.ToString().Contains(searchQuery.SearchWord)));

            if (!string.IsNullOrEmpty(searchQuery.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Price, object>>>
                {
                    { nameof(Price.Id), p => p.Id }
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

            var prices = baseQuery
                .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1))
                .Take(searchQuery.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();


            if (totalItemsCount <= searchQuery.PageSize * (searchQuery.PageNumber - 1))
            {

                throw new BadRequestException($"search result items of Prices: {totalItemsCount} is too small or equal, because the number of skip: {searchQuery.PageSize * (searchQuery.PageNumber - 1)} "
                                                 + ", change the values in 'PageSize = 5, PageNumber = 1' , to see the result");
            }

            if (prices is null || prices.Count == 0) throw new NotFoundException(searchQuery.SearchWord == null ? "list of prices is empty" : $"searchWord not found: {searchQuery.SearchWord}");

            var pricesDtos = mapper.Map<List<AllPriceDto>>(prices);

            var result = new PageResult<AllPriceDto>(pricesDtos, totalItemsCount, searchQuery.PageSize, searchQuery.PageNumber);

            return result;
        }
    }
}

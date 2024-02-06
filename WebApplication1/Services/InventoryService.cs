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
    public interface IInventoryService
    {
        void GetCVS(File inventories);
        List<InventoryDto> GetData(string name);
        DetailsProductDto GetDetailsInventory(DetailsProductDto productDetails);
        void SetDataInDatabase(List<InventoryDto> inventories);
        PageResult<AllInventoryDto> GetAll(InventoryQuery inventoryQuery);
    }
    public class InventoryService : IInventoryService
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;

        public InventoryService(ProductsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public void GetCVS(File inventories)
        {
            Download.DownloadFile(inventories);
        }

        public List<InventoryDto> GetData(string name)
        {

            var results = DownloadData.GetDataFromInventoriesCSV(name);
            return results;
        }

        public DetailsProductDto GetDetailsInventory(DetailsProductDto productDetails)
        {
            if (WebApplication1.File.Inventories == null || WebApplication1.File.Inventories.Count() == 0) return productDetails;
            var shippingCost = WebApplication1.File.Inventories.FirstOrDefault(i => i.SKU == productDetails.SKU && i.ProductId == productDetails.ProductId).ShippingCost;
            productDetails.ShippingCost = shippingCost;
            return productDetails;
        }

        public void SetDataInDatabase(List<InventoryDto> inventories)
        {
            var recordsInventories = mapper.Map<List<Inventory>>(inventories);
            DataSeeder dataSeeder = new DataSeeder(dbContext);
            dataSeeder.SeedInventory(recordsInventories);
        }

        public PageResult<AllInventoryDto> GetAll(InventoryQuery searchQuery)
        {
            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Inventories == null || dbContext.Inventories.Count() == 0) throw new BadRequestException("There are no records in the Inventories table, use the api/file action to set the records in the Inventories table!!!");
            }
            else
            {
                throw new BadRequestException("No database connection !!!");
            }

            // .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1)) -> 5 * (2 - 1) = 10 -> skip 10 items
            var baseQuery = dbContext.Inventories
                .Where(i => searchQuery.SearchWord == null || (i.Id.ToString().Contains(searchQuery.SearchWord)));

            if (!string.IsNullOrEmpty(searchQuery.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Inventory, object>>>
                {
                    { nameof(Inventory.Id), i => i.Id }
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

            var inventories = baseQuery
                .Skip(searchQuery.PageSize * (searchQuery.PageNumber - 1))
                .Take(searchQuery.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();


            if (totalItemsCount <= searchQuery.PageSize * (searchQuery.PageNumber - 1))
            {

                throw new BadRequestException($"search result items of Inventories: {totalItemsCount} is too small or equal, because the number of skip: {searchQuery.PageSize * (searchQuery.PageNumber - 1)} "
                                                 + ", change the values in 'PageSize = 5, PageNumber = 1' , to see the result");
            }

            if (inventories is null || inventories.Count == 0) throw new NotFoundException(searchQuery.SearchWord == null ? "list of inventories is empty" : $"searchWord not found: {searchQuery.SearchWord}");

            var inventoriesDtos = mapper.Map<List<AllInventoryDto>>(inventories);

            var result = new PageResult<AllInventoryDto>(inventoriesDtos, totalItemsCount, searchQuery.PageSize, searchQuery.PageNumber);

            return result;
        }
    }


}

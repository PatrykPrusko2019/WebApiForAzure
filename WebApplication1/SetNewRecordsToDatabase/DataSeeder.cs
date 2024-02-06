using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.SetNewRecordsToDatabase
{
    public class DataSeeder
    {
        private readonly ProductsDbContext dbContext;

        public DataSeeder(ProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedProduct(List<Product> products)
        {
            IEnumerable<Product> records = products.ToList();
            ConnectDatabase();

            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(records);
                dbContext.SaveChanges();
            }
        }


        public void SeedInventory(List<Inventory> inventories)
        {
            IEnumerable<Inventory> records = inventories.ToList();
            ConnectDatabase();

            if (!dbContext.Inventories.Any())
            {
                dbContext.Inventories.AddRange(records);
                dbContext.SaveChanges();
            }
        }

        public void SeedPrice(List<Price> prices)
        {
            IEnumerable<Price> records = prices.ToList();
            ConnectDatabase();

            if (!dbContext.Prices.Any())
            {
                dbContext.Prices.AddRange(records);
                dbContext.SaveChanges();
            }
        }

        public void SeedUser(User user)
        {
            dbContext.Database.Migrate();

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        private void ConnectDatabase()
        {
            if (!dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        public void Seed()
        {
            ConnectDatabase();
        }
    }
}

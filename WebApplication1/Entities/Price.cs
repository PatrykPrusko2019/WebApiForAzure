using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace WebApplication1.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public double? NettProductPrice { get; set; }
        public double? NettProductPriceAfterDiscountForProductLogisticUnit { get; set; }
    }
}

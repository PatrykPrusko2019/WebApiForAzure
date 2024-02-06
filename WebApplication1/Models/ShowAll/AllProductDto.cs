namespace WebApplication1.Models.ShowAll
{
    public class AllProductDto
    {
        public int Id { get; set; }
        public string Shipping { get; set; }
        public string IsWire { get; set; } // if equals 0 -> this is no wire
    }
}

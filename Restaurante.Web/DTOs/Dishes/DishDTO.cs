namespace Restaurante.Web.DTOs.Dishes
{
    public class DishDTO
    {
        public Guid IdDish { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool State { get; set; }
        public Guid IdCategories { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

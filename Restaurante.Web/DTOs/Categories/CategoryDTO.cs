namespace Restaurante.Web.DTOs.Categories
{
    public class CategoryDTO
    {
        public Guid IdCategories { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool State { get; set; }
    }
}

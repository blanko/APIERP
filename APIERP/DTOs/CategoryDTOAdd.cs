namespace APIERP.DTOs
{
    public class CategoryDTOAdd
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

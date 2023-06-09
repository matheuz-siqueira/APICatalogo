namespace APICatalogo.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<ProductDto> Products { get; set; }
}
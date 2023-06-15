using APICatalogo.Models;

namespace APICatalogo.DTOs.Category;

public class ResponseCategoryProductsJson 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<ResponseProductJson> Products { get; set; }
}

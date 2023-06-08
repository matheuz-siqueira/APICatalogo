using System.Data;
using APICatalogo.Data;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly Context _context;
    public CategoriesController([FromServices] Context context)
    {
        _context = context; 
    }

    [HttpGet] 
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        try 
        {
            return Ok (_context.Categories.AsNoTracking().ToList());
        }
        catch
        {
            return BadRequest("Erro ao tratar requisição."); 
        }
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        try
        {
            return _context.Categories.AsNoTracking().Include(c => c.Products).ToList();
        }
        catch
        {
            return BadRequest("Erro ao tratar requisição"); 
        }
         
    }

    [HttpGet("{id:int:min(1)}", Name="GetCategory")]
    public ActionResult<Category> GetById([FromRoute] int id)
    {
        try 
        {
            var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id); 
            if(category is null)
            {
                return NotFound("Categoria não encontrada.");
            }
            return category;
        }
        catch
        {
            return BadRequest("Erro ao tratar requisição");
        }
        
    }

    [HttpPost]
    public ActionResult PostCategory([FromBody] Category category)
    {
        
        if(category is null)
        {
            return BadRequest(); 
        }
        _context.Categories.Add(category); 
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetCategory", new {id = category.Id}, category);
        
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<Category> PutCategory([FromRoute] int id, [FromBody] Category category)
    {
        if(id != category.Id)
        {
            return BadRequest();
        }
        _context.Entry(category).State = EntityState.Modified; 
        _context.SaveChanges(); 
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategory([FromRoute] int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id); 
        if(category is null)
        {
            return NotFound("Categoria não encontrada.");
        }
        _context.Remove(category);
        _context.SaveChanges(); 
        return NoContent(); 
    }
}

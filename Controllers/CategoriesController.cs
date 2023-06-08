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
    public CategoriesController(Context context)
    {
        _context = context; 
    }

    [HttpGet] 
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok (_context.Categories.ToList()); 
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        return _context.Categories.Include(c => c.Products).ToList(); 
    }

    [HttpGet("{id:int}", Name="GetCategory")]
    public ActionResult<Category> GetById([FromRoute] int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id); 
        if(category is null)
        {
            return NotFound("Category not found.");
        }
        return category;
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

    [HttpPut("{id:int}")]
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
            return NotFound("Category not found.");
        }
        _context.Remove(category);
        _context.SaveChanges(); 
        return NoContent(); 
    }
}

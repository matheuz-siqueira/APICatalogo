using System.Data;
using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IUnityOfWork _uof;
    public CategoriesController([FromServices] IUnityOfWork uof)
    {
        _uof = uof;  
    }

    [HttpGet] 
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return _uof.CategoryRepository.Get().ToList(); 
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        return _uof.CategoryRepository.GetCategoriesProducts().ToList();
    }

    [HttpGet("{id:int}", Name="GetCategory")]
    public ActionResult<Category> GetById([FromRoute] int id)
    {
        var category = _uof.CategoryRepository.GetById(c => c.Id == id); 
        if(category is null)
            return NotFound("Categoria não encotrada.");

        return Ok(category);
    }

    [HttpPost]
    public ActionResult PostCategory([FromBody] Category category)
    {
        if(category is null)
            return BadRequest(); 

        _uof.CategoryRepository.Add(category); 
        _uof.Commit(); 

        return new CreatedAtRouteResult("GetCategory", new {id = category.Id}, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Category> PutCategory([FromRoute] int id, [FromBody] Category category)
    {
        if(id != category.Id)
            return NotFound("Categoria não encontrada.");

        _uof.CategoryRepository.Update(category);
        _uof.Commit();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategory([FromRoute] int id)
    {
        var category = _uof.CategoryRepository.GetById(c => c.Id == id);  
        if(category is null)
            return NotFound("Categoria não encontrada.");
        
        _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        return NoContent(); 
    }
}

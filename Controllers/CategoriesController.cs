using System.Data;
using APICatalogo.Data;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;
    public CategoriesController([FromServices] IUnityOfWork uof, [FromServices] IMapper mapper)
    {
        _uof = uof;  
        _mapper = mapper;
    }

    [HttpGet] 
    public ActionResult<IEnumerable<CategoryDto>> GetCategories()
    {
        var categories =  _uof.CategoryRepository.Get().ToList(); 
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<CategoryDto>> GetCategoriesWithProducts()
    {
        var categories = _uof.CategoryRepository.GetCategoriesProducts().ToList();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    [HttpGet("{id:int}", Name="GetCategory")]
    public ActionResult<Category> GetById([FromRoute] int id)
    {
        var category = _uof.CategoryRepository.GetById(c => c.Id == id); 
        if(category is null)
            return NotFound("Categoria não encotrada.");

        var response = _mapper.Map<CategoryDto>(category);
        return Ok(response);
    }

    [HttpPost]
    public ActionResult<CategoryDto> PostCategory([FromBody] CategoryDto request)
    {
        if(request is null)
            return BadRequest(); 

        var category = _mapper.Map<Category>(request);
        _uof.CategoryRepository.Add(category); 
        _uof.Commit(); 
        
        var response = _mapper.Map<CategoryDto>(category);
        return new CreatedAtRouteResult("GetCategory", new {id = category.Id}, response);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoryDto> PutCategory(
            [FromRoute] int id, [FromBody] CategoryDto request)
    {
        if(id != request.Id)
            return NotFound("Categoria não encontrada.");

        var category = _mapper.Map<Category>(request);
        _uof.CategoryRepository.Update(category);
        _uof.Commit();
        var response = _mapper.Map<CategoryDto>(category);
        return Ok(response);
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

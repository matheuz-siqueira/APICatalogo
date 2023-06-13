using System.Data;
using System.Text.Json;
using APICatalogo.Data;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;


[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/categories")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        [FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories =  await _uof.CategoryRepository.GetCategories(categoriesParameters); 

        var metadata = new 
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious
        };

        Response.Headers.Add("X-Paginantion", JsonSerializer.Serialize(metadata));
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesWithProducts()
    {
        var categories = await _uof.CategoryRepository.GetCategoriesProducts();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    [HttpGet("{id:int}", Name="GetCategory")]
    public async Task<ActionResult<Category>> GetById([FromRoute] int id)
    {
        var category = await _uof.CategoryRepository.GetById(c => c.Id == id); 
        if(category is null)
            return NotFound("Categoria não encotrada.");

        var response =  _mapper.Map<CategoryDto>(category);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory([FromBody] CategoryDto request)
    {
        if(request is null)
            return BadRequest(); 

        var category = _mapper.Map<Category>(request);
        _uof.CategoryRepository.Add(category); 
        await _uof.Commit(); 
        
        var response = _mapper.Map<CategoryDto>(category);
        return new CreatedAtRouteResult("GetCategory", new {id = category.Id}, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> PutCategory(
            [FromRoute] int id, [FromBody] CategoryDto request)
    {
        if(id != request.Id)
            return NotFound("Categoria não encontrada.");

        var category = _mapper.Map<Category>(request);
        _uof.CategoryRepository.Update(category);
        await _uof.Commit();
        var response = _mapper.Map<CategoryDto>(category);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory([FromRoute] int id)
    {
        var category = await _uof.CategoryRepository.GetById(c => c.Id == id);  
        if(category is null)
            return NotFound("Categoria não encontrada.");
        
        _uof.CategoryRepository.Delete(category);
        await _uof.Commit();
        return NoContent(); 
    }
}

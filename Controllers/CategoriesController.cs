using System.Text.Json;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Produces("application/json")]
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
    /// <summary> 
    /// Obter caregorias
    /// </summary>
    /// <returns>Relação de categorias</returns>
    /// <response code="200">Sucesso</response> 
    [HttpGet] 
    [ProducesResponseType(StatusCodes.Status200OK)]
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
        var response =  _mapper.Map<List<CategoryDto>>(categories);
        return Ok(response); 
    }

    /// <summary> 
    /// Obter produtos elencados por categoria
    /// </summary>
    /// <returns>Relação de categorias com seus produtos</returns>
    /// <response code="200">Sucesso</response> 
    [HttpGet("products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesWithProducts()
    {
        var categories = await _uof.CategoryRepository.GetCategoriesProducts();
        var response = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(response); 
    }

    /// <summary> 
    /// Obter caregoria por ID
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Categoria correspondente ao ID</returns>
    /// <response code="200">Sucesso</response> 
    /// <response code="404">Não encontrado</response>
    [HttpGet("{id:int}", Name="GetCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Category>> GetById([FromRoute] int id)
    {
        var category = await _uof.CategoryRepository.GetById(c => c.Id == id); 
        if(category is null)
            return NotFound("Categoria não encotrada.");

        var response =  _mapper.Map<CategoryDto>(category);
        return Ok(response);
    }

    /// <summary> 
    /// Cadastrar categoria
    /// </summary>
    /// <remarks>
    /// {"id":0,"name":"string","imageUrl":"string"}
    /// </remarks>
    /// <returns>Categoria recém cadastrada</returns>
    /// <response code="201">Sucesso</response> 
    /// <response code="400">Erro de requisição</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary> 
    /// Atualizar categoria
    /// </summary>
    /// <remarks>
    /// {"id":0,"name":"string","imageUrl":"string"}
    /// </remarks>
    /// <param name="id">ID da categoria</param>
    /// <returns>Categoria recém cadastrada</returns>
    /// <response code="200">Sucesso</response> 
    /// <response code="404">Não encontrado</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary> 
    /// Deletar categoria
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Nada</returns>
    /// <response code="204">Sucesso</response> 
    /// <response code="404">Não encontrado</response> 
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

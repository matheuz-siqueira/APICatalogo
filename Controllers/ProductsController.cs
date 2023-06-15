using APICatalogo.DTOs;
using APICatalogo.DTOs.Product;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalogo.Controllers;

[Produces("application/json")]
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/products")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductsController : ControllerBase
{   
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper; 
    public ProductsController([FromServices] IUnityOfWork uof, [FromServices] IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper; 
    }


    /// <summary>
    /// Obter produtos ordenados por preço
    /// </summary>
    /// <returns>Relação de produtos</returns>
    /// <response code="200">Sucesso</response> 
    [HttpGet("byprice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ResponseProductJson>>> GetByPrice()
    {
        var products = await _uof.ProductRepository.GetProductsByPrice();
        var respones = _mapper.Map<List<ResponseProductJson>>(products); 
        return Ok(respones);
    }

    /// <summary> 
    /// Obter todos os produtos
    /// </summary>
    /// <returns>Relação de produtos</returns>
    /// <response code="200">Sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ResponseProductJson>>> GetAll(
        [FromQuery] ProductsParameters productsParameters)
    {   
        var products = await _uof.ProductRepository.GetProducts(productsParameters);
        var metadata = new 
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

        var response = _mapper.Map<List<ResponseProductJson>>(products); 
        return Ok(response); 
    }
    
    /// <summary> 
    /// Obter produto pelo ID
    /// </summary>
    /// <returns>Retorna objeto correspondente ao ID</returns>
    /// <param name="id">Identificador do produto</param>
    /// <response code="200">Sucesso</response>
    /// <response code="400">Não encontrado</response> 
    [HttpGet("{id:int}", Name="GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ResponseProductJson>> GetById([FromRoute] int id)
    {
        var product = await _uof.ProductRepository.GetById(p => p.Id == id); 
        if(product is null)
            return NotFound();
        

        var response = _mapper.Map<ResponseProductJson>(product);    
        return Ok(response);
    }   

    /// <summary> 
    /// Cadastrar um produto
    /// </summary>
    /// <remarks>
    /// {"name":"string","description":"string","price":0,"imageUrl":"string","categoryId":0}
    /// </remarks>
    /// <param name="request">Dados do produto</param>
    /// <returns>Produto cadastrado</returns>
    /// <response code="201">Sucesso</response> 
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostProduct([FromBody] RequestCreateProductJson request)
    {
        var product = _mapper.Map<Product>(request);
        _uof.ProductRepository.Add(product);
        await _uof.Commit();
        var response = _mapper.Map<ResponseProductJson>(product); 
        return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, response); 
    }

    /// <summary> 
    /// Atualizar um produto
    /// </summary>
    /// <remarks>
    /// {"id":0,"name":"string","description":"string","price":0,"imageUrl":"string","categoryId":0}
    /// </remarks>
    /// <param name="id">Id do produto</param>
    /// <param name="request">Dados do produto</param>
    /// <returns>Produto cadastrado</returns>
    /// <response code="404">Erro</response>
    /// <response code="200">Sucesso</response> 
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> PutProduct([FromRoute] int id, [FromBody] RequestUpdateProductJson request)
    {
        if(id != request.Id)
            return NotFound();
        
        var product = _mapper.Map<Product>(request); 
        _uof.ProductRepository.Update(product);
        await _uof.Commit();
        var response = _mapper.Map<ResponseProductJson>(product); 
        return Ok(response);
    }

    /// <summary>
    /// Deletar um produtor
    /// </summary> 
    /// <param name="id">ID do produto</param>
    /// <returns>Sem retorno</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Não encontrado</response> 
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProduct([FromRoute] int id) 
    {
        var product = await _uof.ProductRepository.GetById(p => p.Id == id);
        if(product is null)
            return NotFound();

        _uof.ProductRepository.Delete(product);
        await _uof.Commit();
        return NoContent(); 
    }
}

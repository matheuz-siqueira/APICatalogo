using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{   
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper; 
    public ProductsController([FromServices] IUnityOfWork uof, [FromServices] IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper; 
    }

    [HttpGet("byprice")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetByPrice()
    {
        var products = await _uof.ProductRepository.GetProductsByPrice();
        return _mapper.Map<List<ProductDto>>(products); 
        
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
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

        return _mapper.Map<List<ProductDto>>(products); 
    }

    [HttpGet("{id:int}", Name="GetProduct")]
    public async Task<ActionResult<ProductDto>> GetById([FromRoute] int id)
    {
        var product = await _uof.ProductRepository.GetById(p => p.Id == id); 
        if(product is null)
            return NotFound();
        
        return _mapper.Map<ProductDto>(product);    
    } 

    [HttpPost]
    public async Task<ActionResult> PostProduct([FromBody] ProductDto request)
    {
        var product = _mapper.Map<Product>(request);
        _uof.ProductRepository.Add(product);
        await _uof.Commit();
        var response = _mapper.Map<ProductDto>(product); 
        return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, response); 
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutProduct([FromRoute] int id, [FromBody] ProductDto request)
    {
        if(id != request.Id)
            return BadRequest();
        
        var product = _mapper.Map<Product>(request); 
        _uof.ProductRepository.Update(product);
        await _uof.Commit();
        var response = _mapper.Map<ProductDto>(product); 
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
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

using APICatalogo.Data;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public ActionResult<IEnumerable<ProductDto>> GetByPrice()
    {
        var products = _uof.ProductRepository.GetProductsByPrice().ToList();
        return _mapper.Map<List<ProductDto>>(products); 
        
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll()
    {   
        var products = _uof.ProductRepository.Get().ToList();
        return _mapper.Map<List<ProductDto>>(products); 
    }

    [HttpGet("{id:int}", Name="GetProduct")]
    public ActionResult<ProductDto> GetById([FromRoute] int id)
    {
        var product = _uof.ProductRepository.GetById(p => p.Id == id); 
        if(product is null)
            return NotFound();
        
        return _mapper.Map<ProductDto>(product);    
    } 

    [HttpPost]
    public ActionResult PostProduct([FromBody] ProductDto request)
    {
        var product = _mapper.Map<Product>(request);
        _uof.ProductRepository.Add(product);
        _uof.Commit();
        var response = _mapper.Map<ProductDto>(product); 
        return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, response); 
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduct([FromRoute] int id, [FromBody] ProductDto request)
    {
        if(id != request.Id)
            return BadRequest();
        
        var product = _mapper.Map<Product>(request); 
        _uof.ProductRepository.Update(product);
        _uof.Commit();
        var response = _mapper.Map<ProductDto>(product); 
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduct([FromRoute] int id) 
    {
        var product = _uof.ProductRepository.GetById(p => p.Id == id);
        if(product is null)
            return NotFound();

        _uof.ProductRepository.Delete(product);
        _uof.Commit();
        return NoContent(); 
    }
}

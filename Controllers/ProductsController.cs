using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{   
    private readonly IUnityOfWork _uof;
    public ProductsController([FromServices] IUnityOfWork uof)
    {
        _uof = uof;
    }

    [HttpGet("byprice")]
    public ActionResult<IEnumerable<Product>> GetByPrice()
    {
        return _uof.ProductRepository.GetProductsByPrice().ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {   
        return _uof.ProductRepository.Get().ToList();
    }

    [HttpGet("{id:int}", Name="GetProduct")]
    public ActionResult<Product> GetById([FromRoute] int id)
    {
        var product = _uof.ProductRepository.GetById(p => p.Id == id); 
        if(product is null)
            return NotFound();
        
        return product;    
    } 

    [HttpPost]
    public ActionResult PostProduct([FromBody] Product product)
    {
        _uof.ProductRepository.Add(product);
        _uof.Commit();
        return new CreatedAtRouteResult("GetProduct", new { id = product.Id }, product); 
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduct([FromRoute] int id, [FromBody] Product product)
    {
        if(id != product.Id)
            return BadRequest();
        
        _uof.ProductRepository.Update(product);
        _uof.Commit();
        return Ok(product);
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

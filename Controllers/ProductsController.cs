using System;
using System.Runtime.InteropServices;
using APICatalogo.Data;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{   
    private readonly Context _context;
    public ProductsController(Context context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProdutcts()
    {   
        try 
        {
            return Ok(_context.Products.ToList());
        } 
        catch
        {
            return NotFound(); 
        }
    }

    [HttpGet("{id:int}", Name="GetProduct")]
    public ActionResult<Product> GetById([FromRoute] int id)
    {
        var response = _context.Products.FirstOrDefault(p => p.Id == id); 
        if(response is null)
        {
            return NotFound("Product not found.");
        }
        return response;    
    } 

    [HttpPost]
    public ActionResult PostProduct([FromBody] Product product)
    {
        if(product is null)
        {
            return BadRequest();
        }
        _context.Products.Add(product);
        _context.SaveChanges(); 
        return new CreatedAtRouteResult("GetProduct", new {id = product.Id}, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduct([FromRoute] int id, [FromBody] Product product)
    {
        if(id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduct([FromRoute] int id) 
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id); 
        if(product is null)
        {
            return NotFound("Product not found."); 
        }
        _context.Remove(product); 
        _context.SaveChanges(); 
        return Ok(); 
    }
}

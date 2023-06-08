using APICatalogo.Data;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{   
    private readonly Context _context;
    public ProductsController([FromServices] Context context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProdutcts()
    {   
        try 
        {
            return Ok(_context.Products.AsNoTracking().ToList());
        } 
        catch
        {
            return BadRequest("Erro ao tratar requisição."); 
        }
    }

    [HttpGet("{id:int:min(1)}", Name="GetProduct")]
    public ActionResult<Product> GetById([FromRoute] int id)
    {
        try
        {
            var response = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id); 
            if(response is null)
            {
                return NotFound("Produto não encontrado.");
            } 
            return response; 
        }
        catch
        {
            return BadRequest("Erro ao tratar requisição.");
        }
          
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

    [HttpPut("{id:int:min(1)}")]
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

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult DeleteProduct([FromRoute] int id) 
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id); 
        if(product is null)
        {
            return NotFound("Produto não encontrado."); 
        }
        _context.Remove(product); 
        _context.SaveChanges(); 
        return Ok(); 
    }
}

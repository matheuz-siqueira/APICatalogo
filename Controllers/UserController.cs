using APICatalogo.DTOs.User;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper; 
    public UserController([FromServices] IUnityOfWork uof, [FromServices] IMapper mapper)
    {
        _uof = uof; 
        _mapper = mapper; 
    }

    [HttpPost]
    public async Task<ActionResult<ResponseCreateUserJson>> PostUser([FromBody] RequestCreateUserJson request)
    { 

        var query = await _uof.UserRepository.GetByEmail(request.Email); 
        if(query is not null)
            return BadRequest("Email alredy exists"); 
        
        var user = _mapper.Map<Models.User>(request);  
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        _uof.UserRepository.Add(user); 
        await _uof.Commit();
        
        var response = _mapper.Map<ResponseCreateUserJson>(user);
        return StatusCode(201, response); 
    }

}

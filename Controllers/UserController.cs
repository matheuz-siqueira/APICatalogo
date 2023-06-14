using APICatalogo.DTOs.User;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Produces("application/json")]
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/users")]
public class UserController : ControllerBase
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper; 
    public UserController([FromServices] IUnityOfWork uof, [FromServices] IMapper mapper)
    {
        _uof = uof; 
        _mapper = mapper; 
    }

    /// <summary>
    /// Cadastrar usuário
    /// </summary> 
    /// <remarks>
    /// {"name":"string","email":"string","password":"string"}
    /// </remarks>
    /// <returns>Usuário cadastrado</returns> 
    /// <response code="201">Sucesso</response>
    /// <response code="400">Erro na requisição</response>      
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

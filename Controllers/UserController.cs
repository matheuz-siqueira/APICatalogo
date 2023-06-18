using System.Security.Claims;
using APICatalogo.DTOs.User;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [HttpPost("register")]
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


    /// <summary>
    /// Obter dados do usuário logado
    /// </summary> 
    /// <returns>Usuário logado</returns> 
    /// <response code="200">Sucesso</response>
    /// <response code="401">Não autenticado</response> 
    [Authorize]
    [HttpGet("myprofile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ResponseUserProfileJson>> GetProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var logged = await _uof.UserRepository.GetProfile(userId); 
        var response = _mapper.Map<ResponseUserProfileJson>(logged); 
        return Ok(response); 
    }


    /// <summary>
    /// Alterar senha do usuário logado
    /// </summary> 
    /// <remarks>
    /// {"currentPassword":"string","newPassword":"string"}
    /// </remarks>
    /// <returns>Sem retorno</returns> 
    /// <response code="204">Sucesso</response>
    /// <response code="400">Erro na requisição</response> 
    [Authorize]
    [HttpPut("update-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePassword([FromBody] RequestUpdatePasswordJson request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var logged = await _uof.UserRepository.GetProfile(userId);
        if(!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, logged.Password))
        {
            return BadRequest("Senha incorreta");
        }
        logged.Password = request.NewPassword; 
        logged.Password = BCrypt.Net.BCrypt.HashPassword(logged.Password);
        _uof.UserRepository.Update(logged); 
        await _uof.Commit(); 
        return NoContent(); 
    }
}

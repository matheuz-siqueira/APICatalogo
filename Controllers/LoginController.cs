using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APICatalogo.DTOs.User;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APICatalogo.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/login")]
public class LoginController : ControllerBase
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public LoginController(
        [FromServices] IUnityOfWork uof, 
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration)
    {
        _uof = uof;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseLoginJson>> Login([FromBody] RequestLoginJson request)
    {
        var user = await _uof.UserRepository.GetByEmail(request.Email); 
        if( (user is null) || (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)))
        {
            return BadRequest("Usuário ou senha incorretos");
        }

        var tokenJWT = GenerateJWT(user); 
        var login = _mapper.Map<ResponseLoginJson>(user); 
        login.Token = tokenJWT; 
        return login; 
        
    }

    private string GenerateJWT(Models.User user)
    {
        //Pegando a chave JWT
        var JWTKey = Encoding.ASCII.GetBytes(_configuration["JWTKey"]);

        //Criando as credenciais
        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(JWTKey),
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Name));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        //Criando o token
        var tokenJWT = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credenciais,
            claims: claims 
        );

        //Escrevendo o token e retornando
        return new JwtSecurityTokenHandler().WriteToken(tokenJWT);
    }
}

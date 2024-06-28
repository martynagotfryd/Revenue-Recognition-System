using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project.Models;
using project.Repositories;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDbService _dbService;
    private readonly IConfiguration config;
    
    public AuthController(IDbService dbService, IConfiguration configg)
    {
        _dbService = dbService;
        config = configg;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string login, string password, string role)
    {
        return Ok(await _dbService.RegisterUser(login, password, role));
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login(string login, string password)
    {

        var employee = await _dbService.ValidateUser(login, password);

        if (employee == null)
        {
            return BadRequest("Login or Password is incorrect.");
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, employee.Role)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(15),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescription);
        var stringToken = tokenHandler.WriteToken(token);

        var refTokenDescription = new SecurityTokenDescriptor
        {
            Issuer = config["JWT:RefIssuer"],
            Audience = config["JWT:RefAudience"],
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:RefKey"]!)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var refToken = tokenHandler.CreateToken(refTokenDescription);
        var stringRefToken = tokenHandler.WriteToken(refToken);
        return Ok(new LoginResponseModel
        {
            Token = stringToken,
            RefreshToken = stringRefToken
        });
    }
}
public class LoginResponseModel
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}


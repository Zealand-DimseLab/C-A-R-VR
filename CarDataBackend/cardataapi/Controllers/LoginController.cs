using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace cardataapi.Controllers;
[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase{
    private readonly LoginService service;

    public LoginController(LoginService service){
        this.service = service;
    }
    [HttpPost]
    [Route("v")]
    public async Task<IActionResult> Login([FromBody] LoginUser tologin){
        if(tologin == null){
            return BadRequest();
        }
        LoginUser? validatedUser = service.CheckLogin(tologin.Id, tologin.Password);
        if(validatedUser == null){
            return Unauthorized();
        }
        var claims = new List<Claim>{
            new(ClaimTypes.NameIdentifier, validatedUser.Id.ToString()),
            new(ClaimTypes.Role, "admin")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return Ok(new {id = validatedUser.Id, role = "admin" });
    }
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new
                {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                role = User.FindFirstValue(ClaimTypes.Role)
                });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}
public class LoginService{
    // private const int workFactor = 12;
    private string connectionString;
    public LoginService(string connectionString){
        this.connectionString = connectionString;
    }
    public LoginUser CheckLogin(int idtovali, string passwordtocheck){
        if(string.IsNullOrEmpty(passwordtocheck))
            return null;
        LoginUser? tovalidate = GetLogin(idtovali);
        if(tovalidate == null)
            return null;
        return ValidatePassword(passwordtocheck, tovalidate.Password) ? tovalidate : null;
    }
    private bool ValidatePassword(string password, string hashedpassword){
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedpassword);
    }
    private LoginUser GetLogin(int id){
        SqlConnection connection = new SqlConnection(connectionString);
        using(connection){
            connection.Open();
            string sql = "SELECT id, password from LoginUser where (id = @id);";

            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();

            if(reader.Read()){
                return new LoginUser(reader.GetInt32(0), reader.GetString(1));
            }
        }
        return null;
    }
}
public class LoginUser{
    public int Id {get; set;}
    public string Password {get; set;}
    public LoginUser(int id, string password){
        Id = id;
        Password = password;
    }
}

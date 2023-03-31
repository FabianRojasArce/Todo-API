using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Security.Claims;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public AuthController(
        IConfiguration configuration,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AuthController> logger
    )
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterForm form)
    {
        string userName = form.Email.Split('@')[0];
        var user = new IdentityUser { Email = form.Email, UserName = userName };
        var result = await _userManager.CreateAsync(user, form.Password);
        if (result.Succeeded)
        {
            return Ok();
        }
        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Description);
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginForm form)
    {
        var user = await _userManager.FindByEmailAsync(form.Email);
        if (user == null){
            return Results.BadRequest();
        }
        var password = await _userManager.CheckPasswordAsync(user, form.Password);

        if (password) {
            if (user.UserName != null){
                var result = await _signInManager.PasswordSignInAsync(user.UserName, form.Password, true, false);
                if (result.Succeeded) {
                    return Results.Ok();
                }else{
                    return Results.BadRequest();
                }
            }

        }
        return Results.BadRequest();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    public class LoginForm {
        public string Email {get; set; }
        public string Password {get; set; }
    }

    public class RegisterForm {
        public string Email {get; set; }
        public string Password {get; set; }
        public string ConfirmPassword {get; set; }
    }


    // [HttpPost]
    // [Route("login")]
    // public async Task<IActionResult> Login(string email, string username, string password)
    // {
    //     var result = await _signInManager.PasswordSignInAsync(
    //         username,
    //         password,
    //         false,
    //         lockoutOnFailure: false
    //     );
    //     if (result.Succeeded)
    //     {
    //         var user = await _userManager.FindByNameAsync(username);
    //         if (user != null) {
    //             var token = await _userManager.GenerateUserTokenAsync(user, "Default", "Login");
    //             return Ok(token);
    //         }else{
    //             return BadRequest("Usuario no encontrado");
    //         }
    //     }
    //     else
    //     {
    //         return BadRequest("falla el login");
    //     }
    //     // ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    // }


}
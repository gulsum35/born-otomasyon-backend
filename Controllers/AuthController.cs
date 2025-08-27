using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BornOtomasyonApi.Models;
using BornOtomasyonApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;

namespace BornOtomasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              IConfiguration configuration,
                              IEmailService emailService,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Konsola token yazdır (mail gelmezse buradan token ile gösterilir.)
            Console.WriteLine($"Email confirmation token for {user.Email}: {token}");

            var encodedToken = HttpUtility.UrlEncode(token);
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:4200";
            var confirmationLink = $"{frontendUrl}/confirm-email?email={user.Email}&token={encodedToken}";

            await _emailService.SendEmailAsync(user.Email, "Email Doğrulama",
                $"Lütfen email adresinizi doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a>.");

            return Ok("Kayıt başarılı! Emailinizi doğrulayın.");
        }

        // ================= CONFIRM EMAIL =================
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);// token tamsa email doğrulama tamamlanır 
            if (user == null) return BadRequest("Kullanıcı bulunamadı.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Ok("Email doğrulandı.") : BadRequest("Email doğrulama başarısız.");
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email); // mail ile kullanıcı bulunur.
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Email veya şifre hatalı.");

            if (!user.EmailConfirmed)
                return BadRequest("Email doğrulanmamış.");

            var token = _tokenService.CreateToken(user);// doğruysa token oluşturulur.

            return Ok(new { token });
        }

        // ================= FORGOT PASSWORD =================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?email={dto.Email}&token={HttpUtility.UrlEncode(token)}";// frontend reset linki oluşturulur.
             
            await _emailService.SendEmailAsync(dto.Email, "Şifre Sıfırlama",
                $"Yeni şifre oluşturmak için <a href='{resetLink}'>buraya tıklayın</a>.");

            return Ok("Şifre sıfırlama maili gönderildi.");
        }

        // ================= RESET PASSWORD =================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Şifre başarıyla değiştirildi.");
        }
    }

    // ================= dto backend ve frontend arası vei taşır.===========
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}

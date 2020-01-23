using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

using BeerStore.Services;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

namespace BeerStore.Controllers.Api
{
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/auth")]
	public class AuthController : Controller
    {
        private readonly StoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthController(
            StoreContext context, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IEmailSender emailSender,
            ILogger<AccountController> logger, 
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _localizer = localizer;
        }

        private async Task<bool> DoLogin(LoginViewModel creds)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(creds.Name);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, creds.Password, false, false);
                return result.Succeeded;
            }
            return false;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel creds)
        {
            if (ModelState.IsValid && await DoLogin(creds))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Name);

            if (user != null) { 
                //return BadRequest("A user with that e-mail address already exists!");
                return BadRequest(_localizer["UserExist"].Value);
            }
            
            user = new ApplicationUser
            {
                Email = model.Name,
                UserName = model.Name,
                EmailConfirmed = true,                
                LockoutEnabled = true,
                FirstName = model.FirstName,
                LastName = model.LastName                
            };

            var registerResult = await _userManager.CreateAsync(user, model.Password);

            if (registerResult.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _userManager.AddToRoleAsync(user, "Users");

                var shoppingArea = new ShoppingArea
                {
                    Descr = "Site " + model.LastName + " " + model.FirstName,
                    Code = "",
                    Version = "",
                    IsMark = false
                };

                var userMeta = new UserMeta
                {
                    User = user,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ShoppingArea = shoppingArea
                };

                _context.ShoppingArea.Add(shoppingArea);
                _context.UserMeta.Add(userMeta);
                await _context.SaveChangesAsync();
                
                user.UserMetaId = userMeta.Id;
                await _userManager.UpdateAsync(user);


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(model.Name, callbackUrl);

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created a new account with password.");

            }
            else
            {
                return BadRequest(registerResult.Errors);
            }           

            return Ok();
        }
        
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)            
                return BadRequest();            

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return BadRequest();
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ApiResetPasswordCallbackLink(user.Id, code, Request.Scheme, model.Host);
            //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
            //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            //EmailSender emailSender = new EmailSender();
            //await emailSender.SendEmailAsync(model.Email, _localizer["PasswordReset"], $"{_localizer["PasswordPleaseReset"]}: <a href='{callbackUrl}'>link</a>");

            await _emailSender.SendEmailAsync(model.Email, _localizer["PasswordReset"],
               $"{_localizer["PasswordPleaseReset"]}: <a href='{callbackUrl}'>link</a>");

            return Ok();
        }

        [HttpPost("reset")]        
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();            

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                //return RedirectToAction(nameof(ResetPasswordConfirmation));
                return Ok();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                //return RedirectToAction(nameof(ResetPasswordConfirmation));
                return Ok();
            }
            //AddErrors(result);
            //return View();

            return BadRequest(result.Errors);
        }
        
    }

    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(70)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(70)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PasswordErrorMessage", MinimumLength = 6)] //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "ConfirmPasswordErrorMessage")]  //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]        
        public string ConfirmPassword { get; set; }        
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]        
        public string Host { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}

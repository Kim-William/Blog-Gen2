
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Wkkim.Blog.Web.Models.ViewModels;

namespace Wkkim.Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };



            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                // assign this user the "User" role

                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityResult.Succeeded)
                {
                    // Show success notification
                    return RedirectToAction("Register");
                }

            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            Console.WriteLine(loginViewModel.RememberMe);

            var signInResult = await signInManager.PasswordSignInAsync(
                loginViewModel.UserName,
                loginViewModel.Password,
                loginViewModel.RememberMe,
                lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                var user = await userManager.FindByNameAsync(loginViewModel.UserName);
                var claims = new List<Claim>    
                {
                    new Claim(ClaimTypes.Name, user.UserName),     
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

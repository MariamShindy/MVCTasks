using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskThree.PL.Models;
using TaskThree.PL.ViewModels.Account;

namespace TaskThree.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel) 
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(signUpViewModel.Email);
				if (user is null)
				{
					user = new ApplicationUser()
					{
						UserName = signUpViewModel.UserName,
						Email = signUpViewModel.Email,
						FName = signUpViewModel.FirstName,
						LName = signUpViewModel.LastName,
						IsAgree = signUpViewModel.IsAgree,
					};
					var result = await userManager.CreateAsync(user, signUpViewModel.Password);
					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				ModelState.AddModelError(string.Empty, "This UserName Is Already In Use for another account");

			}
			return View(signUpViewModel);
		}
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,false);
						if (result.IsLockedOut)
						{
							ModelState.AddModelError(string.Empty, "Your account is locked"); 
						}
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index),"Home");
						}
						if (result.IsNotAllowed)
						{
							ModelState.AddModelError(string.Empty, "Your account is not confirmed yet"); ;
						}

					}
				}
				ModelState.AddModelError(string.Empty, "Invalid login");
			}
			return View(model);

		}
		public async new Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
	}
}

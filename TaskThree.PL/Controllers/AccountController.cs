using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskThree.PL.Models;
using TaskThree.PL.ViewModels.User;

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
	}
}

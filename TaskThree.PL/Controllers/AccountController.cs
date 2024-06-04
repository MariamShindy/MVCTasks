using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TaskThree.PL.Models;
using TaskThree.PL.Services.EmailSender;
using TaskThree.PL.ViewModels.Account;

namespace TaskThree.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly IEmailSender emailSender;
		private readonly IConfiguration configuration;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController (IEmailSender emailSender,IConfiguration configuration,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.emailSender = emailSender;
			this.configuration = configuration;
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
		public IActionResult ForgetPassword()
		{
			 return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);

				if (user is not null) 
				{
					var passwordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, "localhost:5001");
					//send email
					await emailSender.SendAsync(
						from: configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
						subject: "reset your password",
						body: passwordUrl
						) ; 
					return Redirect(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "There is not account with this email");
			}
			return View(model);
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		[HttpGet]
		public IActionResult ResetPassword(string email , string token)
		{
			TempData["Email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var email = TempData["Email"] as string;
				var token = TempData["token"] as string;
				var user = await userManager.FindByNameAsync(email);
                if (user is not null)
                {
					userManager.ResetPasswordAsync(user,token,model.NewPassword);
					return RedirectToAction(nameof(SignIn));
                }
				ModelState.AddModelError(string.Empty, "Url is not valid");
			}
			return View(model);
		}
	}
}

using System.ComponentModel.DataAnnotations;

namespace TaskThree.PL.ViewModels.User
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "UserName is required")]
		public string UserName { get; set; }
		[Required(ErrorMessage="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "FName is required")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "LName is required")]
		[Display(Name ="Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage ="Password is required")]
		[MinLength(5,ErrorMessage ="Min length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password),ErrorMessage ="Confirmed Password doesn't not match with Password")]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }
	}
}

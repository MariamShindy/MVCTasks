using System.ComponentModel.DataAnnotations;

namespace TaskThree.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "New Password is required")]
		[MinLength(5, ErrorMessage = "Min length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirmed Password doesn't not match with Password")]
		public string ConfirmPassword { get; set; }
 
    }
}

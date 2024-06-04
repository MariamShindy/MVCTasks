using System.Globalization;
using System.Threading.Tasks;

namespace TaskThree.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(string from ,  string recipients , string subject , string body );
	}
}

using System.Globalization;
using System.Threading.Tasks;

namespace TaskThree.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(StringInfo from ,  StringInfo recipients , string subject , string body );
	}
}

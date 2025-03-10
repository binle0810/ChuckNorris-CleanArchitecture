using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string userid);
        //     Task SendEmailAsync(string userid, string toEmail, string subject, string body);
        Task SendFavoritesEmailJob();
    }
}

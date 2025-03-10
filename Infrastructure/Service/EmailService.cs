using Application.Abstractions;
using Application.Email.Command;
using Domain.Entities;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly ISender _sender;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, ApplicationDbContext context, UserManager<AppUser> user, ISender sender)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _context = context;
            _usermanager = user;
            _sender = sender;
        }

        /* public async Task SendEmailAsync(string userid,string toEmail, string subject, string body)
         {
             try
             {
                 var email = new MimeMessage();
                 email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                 email.To.Add(MailboxAddress.Parse(toEmail));
                 email.Subject = subject;

                 var bodyBuilder = new BodyBuilder { HtmlBody = body };
                 email.Body = bodyBuilder.ToMessageBody();

                 using var smtp = new SmtpClient();
                 await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                 await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                 await smtp.SendAsync(email);
                 await smtp.DisconnectAsync(true);

                 _logger.LogInformation("Email sent to {Email}", toEmail);
             }
             catch (Exception ex)
             {
                 _logger.LogError("Error sending email to {Email}: {Message}", toEmail, ex.Message);
             }
         }*/
        public async Task SendFavoritesEmailJob()
        {
            await _sender.Send(new SendFavoriteQuotesEmailCommand(null));
        }
        public async Task SendEmailAsync(string userid)
        {
            if(userid == null)
            {
                var userIds = _context.UserLikeJokes
                      .Select(ulj => ulj.UserId)
                      .Distinct() // Nếu muốn loại bỏ trùng lặp
                      .ToList();
                foreach( var id in userIds)
                {
                    var data = await _context.UserLikeJokes.Include(x => x.Joke).Where(u => u.UserId == id)
                  .Select(x => x.Joke.Id).ToListAsync();
                    var user = await _usermanager.FindByIdAsync(id);
                    //  
                    _logger.LogWarning($"email:" + user.Email);
                    SendEmail(user.Email, "Câu nói yêu thích", GenerateEmailContent(data));
                }
            }
            else
            {
                var data = await _context.UserLikeJokes.Include(x => x.Joke).Where(u => u.UserId == userid)
                    .Select(x => x.Joke.Id).ToListAsync();
                var user = await _usermanager.FindByIdAsync(userid);
                //  

                SendEmail(user.Email, "Câu nói yêu thích", GenerateEmailContent(data));
                
            }
          
        }
        private async void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email sent to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending email to {Email}: {Message}", toEmail, ex.Message);
            }
        }
        private string GenerateEmailContent(IEnumerable<string> quotes)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Danh sách các câu nói yêu thích của bạn:</h2><ul>");
            foreach (var quote in quotes)
            {
                sb.AppendLine($"<li>{quote}</li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}

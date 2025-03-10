using Application.Abstractions;
using Application.Email.Command;
using Hangfire;
using Infrastructure.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public sealed class EmailController : ApiController
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [Authorize]

        [HttpPost("send-favorites-all")]
        public async Task<IActionResult> SendFavoritesEmail()
        {

            RecurringJob.AddOrUpdate<IEmailService>(
            "DailySendEmail",
           service => service.SendFavoritesEmailJob(),    
        //   () => Console.WriteLine("recurt"),

            Cron.Minutely)  ;
         //   await emailService.SendFavoritesEmailJob();
            return Ok("Emails sent to all users successfully.");
        }
     
        [Authorize]

        [HttpPost("send-favorites-test")]
        public async Task<IActionResult> SendFavoritesEmailToUser()
        {
            var userid = User.GetUserId();
            await Sender.Send(new SendFavoriteQuotesEmailCommand(userid));
            return Ok($"Email sent to user {userid} successfully.");
        }
    }
}

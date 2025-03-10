using Application.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Email.Command
{
    public class SendFavoriteQuotesEmailHandler : IRequestHandler<SendFavoriteQuotesEmailCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public SendFavoriteQuotesEmailHandler(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendFavoriteQuotesEmailCommand request, CancellationToken cancellationToken)
        {
            /*    List<AppUser> users;

                if (request.UserId == null)
                {
                    // Lấy tất cả users có favorite quotes
                    users = await _userManager.Users.ToListAsync(cancellationToken);

                }
                else
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);
                    users = user != null ? new List<AppUser> { user } : new List<AppUser>();
                }

                // Gửi email cho từng user có danh sách yêu thích
                foreach (var user in users)
                {
                   var jokeIds = users
            .SelectMany(u => u.LikedJokes) // Lấy tất cả LikedJokes của users
            .Select(fq => fq.JokeId) // Lấy JokeId
            .Distinct() // Loại bỏ trùng
            .ToList();
                    var emailBody = GenerateEmailContent(jokeIds);
                    await _emailService.SendEmailAsync(user.Email, "Danh sách câu nói yêu thích", emailBody);
                }*/
            await _emailService.SendEmailAsync(request.UserId);
            return Unit.Value;
        }

 
    }
}

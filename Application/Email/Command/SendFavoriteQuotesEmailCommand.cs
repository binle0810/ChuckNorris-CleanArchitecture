using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Email.Command
{
    public class SendFavoriteQuotesEmailCommand : IRequest
    {
        public string? UserId { get; set; }

        public SendFavoriteQuotesEmailCommand(string? userId = null) // Nếu null => gửi tất cả user
        {
            UserId = userId;
        }
    }
}

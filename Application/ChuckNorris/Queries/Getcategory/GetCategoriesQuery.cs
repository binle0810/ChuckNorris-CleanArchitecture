using Application.Abstractions.Messaging;
using Application.Webinars.Queries.GetWebinarById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.Getcategory
{
    public sealed record GetCategoriesQuery : IQuery<List<string>>
    {
    }
}

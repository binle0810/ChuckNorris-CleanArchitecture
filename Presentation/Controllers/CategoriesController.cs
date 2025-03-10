
using Application.ChuckNorris.Queries.Getcategory;
using Application.Webinars.Queries.GetWebinarById;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public sealed class CategoriesController:ApiController
    {
    

        public CategoriesController()
        {
         
        }

      
        [HttpGet]
        public async Task<IActionResult> GetCategogy( CancellationToken cancellationToken)
        {
            var query = new GetCategoriesQuery();

            var categories = await Sender.Send(query, cancellationToken);

            return Ok(categories);
        }
    
    }
}

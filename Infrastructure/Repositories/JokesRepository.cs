using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;


namespace Infrastructure.Repositories
{
    public sealed class JokesRepository : IJokesRepository
    {
        public Task<string> GetRandomJoke()
        {
            throw new NotImplementedException();
        }
    }
}

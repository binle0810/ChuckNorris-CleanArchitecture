using Application.DTOs.Jokes;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IChuckNorrisService
    {
        Task<List<string>> GetCategoriesAsync();
        Task<string> GetRandomJokeByCategoryAsync(string category);
        Task<string> GetRandomJokeAsync();
        Task<string> GetRandomJokeBySearchAsync(string search);
        Task<LikeJoke> CreateLikeJokeAsync(UserLikeJoke userLikeJoke);
        Task<JokeDto> DeleteLikeJokeAsync(string userid, string jokeid);
        Task<List<JokeHistoryDto>> GetUserLikeJokeAsync(string userid, QueryWithPage queryWithPage);


    }

}

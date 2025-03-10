using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTOs.Jokes;
using Domain.Abstractions;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Reflection.Metadata.BlobBuilder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Infrastructure.Repositories
{
    public sealed class ChuckNorrisService : IChuckNorrisService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChuckNorrisService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ChuckNorrisService(HttpClient httpClient, ILogger<ChuckNorrisService> logger, ApplicationDbContext dbContext)
        {
            _httpClient = httpClient;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("jokes/categories");
               
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch categories. Status Code: {StatusCode}", response.StatusCode);
                    return new List<string>();
                }

                var categories = await response.Content.ReadFromJsonAsync<List<string>>();
                return categories ?? new List<string>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error while fetching categories");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching categories");
            }

            return new List<string>();
        }
        public async Task<string> GetRandomJokeByCategoryAsync(string category)
        {
            try
            {
                var response = await _httpClient.GetAsync($"jokes/random?category={category}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch joke for category {Category}. Status Code: {StatusCode}", category, response.StatusCode);
                    return "Error: Unable to fetch joke.";
                }

                var jokeResponse = await response.Content.ReadAsStringAsync();

                return jokeResponse ?? "No joke found.";
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error while fetching joke for category {Category}", category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unexpected error while fetching joke for category {Category}", category);
            }

            return "Error: An unexpected error occurred.";
        }

        public async Task<string> GetRandomJokeAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("jokes/random");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch random joke . Status Code: {StatusCode}", response.StatusCode);
                    return "Error: Unable to fetch random joke.";
                }

                var jokeResponse = await response.Content.ReadAsStringAsync();

                return jokeResponse;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error while fetching random joke ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching random joke ");
            }

            return "Error: An unexpected error occurred.";
        }

        public async Task<string> GetRandomJokeBySearchAsync(string search)
        {
            try
            {
                var response = await _httpClient.GetAsync($"jokes/search?query={search}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch joke for search:{search}. Status Code: {StatusCode}", search, response.StatusCode);
                    return await response.Content.ReadAsStringAsync();
                }

                var jokeResponse = await response.Content.ReadAsStringAsync();

                return jokeResponse ?? "No joke found.";
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error while fetching joke for search:{Category}", search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching joke for search:{Category}", search);
            }

            return "Error: An unexpected error occurred.";
        }

        public async Task<LikeJoke> CreateLikeJokeAsync(UserLikeJoke userLikeJoke)
        {
            var existingLike = await _dbContext.UserLikeJokes
        .FirstOrDefaultAsync(ulj => ulj.UserId == userLikeJoke.UserId && ulj.JokeId == userLikeJoke.JokeId);

            if (existingLike != null)
            {
                // Nếu đã like, có thể bỏ qua hoặc xử lý unlike
                var jokeUnlike = await _dbContext.LikeJokes.FirstOrDefaultAsync(j => j.Id == userLikeJoke.JokeId);

                if (jokeUnlike.Like > 0) { jokeUnlike.Like--; }

                _dbContext.UserLikeJokes.Remove(existingLike);
                _logger.LogWarning("Đã like 2 lần");
               return jokeUnlike;
            }
           
         

            // Tìm joke tương ứng để cập nhật số lượt like
            var joke = await _dbContext.LikeJokes.FirstOrDefaultAsync(j => j.Id == userLikeJoke.JokeId);

            if (joke == null)
            {
                joke = new LikeJoke
                {
                    Id = userLikeJoke.JokeId, 
                };

                await _dbContext.LikeJokes.AddAsync(joke);
                _logger.LogWarning("add new joke");
            }

          
            joke.Like++;


            await _dbContext.UserLikeJokes.AddAsync(userLikeJoke);

            return joke;
        }

        public async Task<JokeDto> DeleteLikeJokeAsync(string userid, string jokeid)
        {
            var existingLike = await _dbContext.UserLikeJokes
      .FirstOrDefaultAsync(ulj => ulj.UserId == userid && ulj.JokeId == jokeid);

            if (existingLike == null)
            {
                // Nếu chưa like, có thể bỏ qua 
                throw new InvalidOperationException("User chưa like joke này trước đó.");
            }



            // Tìm joke tương ứng để cập nhật số lượt like
            var joke = await _dbContext.LikeJokes.FirstOrDefaultAsync(j => j.Id == jokeid);

        

            
            if (joke.Like > 0) { joke.Like--; }

            
             _dbContext.UserLikeJokes.Remove(existingLike);
            var jokedto = joke.Adapt<JokeDto>();
            _logger.LogWarning("Unlike successful");
            return jokedto;
        }

        public async Task<List<JokeHistoryDto>> GetUserLikeJokeAsync(string userid, QueryWithPage queryWithPage)
        {
            var history= await _dbContext.UserLikeJokes.Where(u => u.UserId == userid)
            .Select(joke => new JokeHistoryDto
            {
                Id = joke.JokeId,
                Like=joke.Joke.Like,
                likeat = joke.LikedAt
            }
            ).ToListAsync();
            if (history == null) { _logger.LogWarning("empty history"); }
            return await PaginatedList< JokeHistoryDto>.CreateAsync(history, queryWithPage.PageNumber, queryWithPage.PageSize); ;
        }
    }
}

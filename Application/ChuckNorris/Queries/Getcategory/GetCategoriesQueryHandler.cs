using Application.Abstractions;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.Getcategory
{
    internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<string>>
    {
        private readonly IChuckNorrisService _chuckNorrisService;
        private readonly ICacheService _caceService;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
        
        public GetCategoriesQueryHandler(IChuckNorrisService chuckNorrisService, ICacheService caceService)
        {
            _chuckNorrisService = chuckNorrisService;
            _caceService = caceService;
        }

        public async Task<List<string>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "Getcategory";
            var cate = _caceService.Get<List<string>>(cacheKey);
            if (cate != null)
            {
                Console.WriteLine("in cache");
                return cate;  // Trả về từ cache nếu có
            }

            // Nếu cache không có, truy vấn database
            cate = await _chuckNorrisService.GetCategoriesAsync();

            // Lưu vào cache
            _caceService.Set(cacheKey, cate, _cacheDuration);
            Console.WriteLine("in httpclient");
            return cate;
        }
    }
}

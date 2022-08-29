using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceContracts
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTopRevenueMovies();
        Task<List<MovieCardModel>> GetTopRatedMovies();

        Task<MovieDetailsModel> GetMovieDetails(int movieId);

        Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int genreId, int pageSize = 30, int page = 1);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByTitlePagination(int pageSize, int page, string title);
        Task<List<MovieReviewResponseModel>> GetReviewsByMovie(int id);
    }
}

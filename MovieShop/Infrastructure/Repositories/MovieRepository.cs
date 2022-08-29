using ApplicationCore.Entities;
using ApplicationCore.RepositoryContracts;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;

namespace Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public MovieRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<Movie> GetById(int id)
        {
            // select * from movie where id = 1 join genre, cast, moviegerne, moviecast
            var movieDetails = await _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .FirstOrDefaultAsync(m => m.Id == id);

            var rating = await _movieShopDbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty().AverageAsync(r => r == null ? 0 : r.Rating);

            return movieDetails;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int page = 1)
        {
            // get total row count
            var totalMoviesCountOfGenre = await _movieShopDbContext.movieGenres.Where(g => g.GenreId == genreId).CountAsync();
            if (totalMoviesCountOfGenre == 0)
            {
                throw new Exception("No Movies found for this genre");
            }

            // get the actual data
            var movies = await _movieShopDbContext.movieGenres.Where(g => g.GenreId == genreId).Include(g => g.Movie).OrderByDescending(m => m.Movie.Revenue)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalMoviesCountOfGenre);
            return pagedMovies;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByTitlePagination(int pageSize, int page, string title)
        {
            // get total row count
            var totalMoviesCountOfTitle = await _movieShopDbContext.Movies.Where(t => t.Title.Contains(title)).CountAsync();
            if (totalMoviesCountOfTitle == 0)
            {
                throw new Exception("No Movies found");
            }

            // get the actual data
            var movies = await _movieShopDbContext.Movies.Where(t => t.Title.Contains(title)).OrderByDescending(m => m.Revenue)
                .Select(m => new Movie
                {
                    Id = m.Id,
                    PosterUrl = m.PosterUrl,
                    Title = m.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalMoviesCountOfTitle);
            return pagedMovies;
        }

        public async Task<List<Movie>> GetTop30HighestRevenueMovies()
        {
            // call the database with EF Core and get the data
            // use MovieShopDbContext and Movies DbSet
            // select top 30 * from Movies order by Revenue
            // corresponding LINQ Query

            var movies = await _movieShopDbContext.Movies
                .OrderByDescending(m => m.Revenue)
                .Select(m => new Movie { Id = m.Id, Title = m.Title, PosterUrl = m.PosterUrl })
                .Take(30).ToListAsync();
            return movies;
        }

        public async Task<List<Movie>> GetTop30RatedMovies()
        {
            var movies = await _movieShopDbContext.Movies
                .OrderByDescending(m => m.Rating)
                .Select(m => new Movie { Id = m.Id, Title = m.Title, PosterUrl = m.PosterUrl })
                .Take(30).ToListAsync();
            return movies;
        }
    }
}
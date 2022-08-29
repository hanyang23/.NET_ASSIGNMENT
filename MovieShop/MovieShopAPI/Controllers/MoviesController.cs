using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        ///     Get collection of movies by pagination and search term for movie title
        ///     default page size is 30
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <returns></returns>


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
        public async Task<ActionResult<PagedResultSet<MovieCardResponseModel>>> GetAllMovies([FromQuery] int pageSize = 30,
       [FromQuery] int page = 1, string title = "")
        {
            var movies = await _movieService.GetMoviesByTitlePagination(pageSize, page, title);
            if (movies?.Data?.Any() == false)
            {
                return NotFound(new ErrorModel { Message = "No movies found for search query" });
            }

            return Ok(movies);
        }


        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId, int pageSize = 30, int page = 1)
        {
            var pagedMovies = await _movieService.GetMoviesByPagination(genreId, pageSize, page);

            if (pagedMovies == null)
            {
                // 404
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(pagedMovies);
        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetReviewsByMovie(int id)
        {
            var reviews = await _movieService.GetReviewsByMovie(id);

            if (reviews == null)
            {
                return NotFound(new { errorMessage = "No reviews Found" });
            }
            return Ok(reviews);
        }


        [HttpGet]
        [Route("top-rated")]
        // Attribute Routing
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();

            if (movies == null || !movies.Any())
            {
                // 404
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("top-revenue")]
        // Attribute Routing
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            var movies = await _movieService.GetTopRevenueMovies();

            if (movies == null || !movies.Any())
            {
                // 404
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { errorMessage = $"No Movie Found for {id}" });
            }

            return Ok(movie);
        }
    }
}

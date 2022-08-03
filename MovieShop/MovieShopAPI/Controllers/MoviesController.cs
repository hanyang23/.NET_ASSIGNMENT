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

        [HttpGet]
        [Route("top-revenue")]
        // attribute routing
        // http:/ /localhost/movies/GetToprevenueMovies
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            //call my service
            var movies = await _movieService.GetTopRevenueMovies();
            //return the movies information in JSON Format

            if (movies == null || !movies.Any())
            {
                //404
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
                return NotFound(new {errorMessage = $"No Movie Found for {id}"});
            }

            return Ok(movie);
        }
    }
}

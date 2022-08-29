using ApplicationCore.ServiceContracts;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShopAPI.Infra;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public UserController(IUserService userService, ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [HttpGet]
        [Route("details/{id:int}")]
        public async Task<IActionResult> GetUserDetailsById(int id)
        {
            var details = await _userService.GetUserDetailsById(id);
            if (details == null)
            {
                return NotFound("No User Found");
            }
            return Ok(details);
        }

        //---------------------------------- Purchase --------------------------------------

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser()
        {
            var userId = _currentUser.UserId;
            var Movies = await _userService.GetAllPurchasesForUser(userId);
            if (Movies == null)
            {
                return NotFound(new { errorMessage = "No Movies Purchased" });
            }

            return Ok(Movies);
        }

        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> BuyMovie([FromBody] UserRequestModel userRequest)
        {
            //var userId = _currentUser.UserId;
            PurchaseRequestModel model = new PurchaseRequestModel
            {
                MovieId = userRequest.movieId,
                UserId = userRequest.userId
            };

            var movie = await _userService.PurchaseMovie(model, model.UserId);
            if (model.MovieId == null)
            {
                return NotFound(new { errorMessage = "No Movie Found" });
            }

            return Ok(movie);
        }

        [HttpGet]
        [Route("check-movie-purchased/{movieId:int}")]
        public async Task<IActionResult> CheckMoviePurchased(int movieId)
        {
            var userId = _currentUser.UserId;
            PurchaseRequestModel model = new PurchaseRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };

            var movie = await _userService.IsMoviePurchased(model, userId);
            if (!movie)
            {
                return NotFound(new { errorMessage = "Movie is not purchased" });
            }

            return Ok();
        }

        [HttpGet]
        [Route("purchase-details/{movieId:int}")]
        public async Task<IActionResult> MoviePurchaseDetails(int movieId)
        {
            var userId = _currentUser.UserId;

            var movieDetails = await _userService.GetPurchasesDetails(userId, movieId);
            if (movieDetails == null)
            {
                return NotFound(new { errorMessage = "Movie is not purchased" });
            }

            return Ok(movieDetails);
        }

        //-------------------------------- Favorite --------------------------------------
        [HttpGet]
        [Route("favorites")]
        public async Task<IActionResult> Favorites()
        {
            var userId = _currentUser.UserId;
            var Movies = await _userService.GetAllFavoritesForUser(userId);
            if (Movies == null)
            {
                return NotFound(new { errorMessage = "No Favorite Movies" });
            }

            return Ok(Movies);
        }

        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> FavoriteMovie([FromBody] FavoriteRequestModel favoriteRequest)
        {
            //var userId = _currentUser.UserId;
            FavoriteRequestModel model = new FavoriteRequestModel
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            var movie = await _userService.AddFavorite(model);
            if (!movie)
            {
                return NotFound(new { errorMessage = "No Movie Found" });
            }

            return Ok(movie);
        }

        [HttpPost]
        [Route("un-favorite")]
        public async Task<IActionResult> UnFavoriteMovie([FromBody] FavoriteRequestModel favoriteRequest)
        {
            FavoriteRequestModel model = new FavoriteRequestModel
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            var movie = await _userService.RemoveFavorite(model);
            if (!movie)
            {
                return NotFound(new { errorMessage = "No Movie Found" });
            }

            return Ok(movie);
        }

        [HttpGet]
        [Route("check-movie-favorite/{movieId:int}")]
        public async Task<IActionResult> CheckMovieFavorite(int movieId)
        {
            var userId = _currentUser.UserId;
            FavoriteRequestModel model = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };

            var movie = await _userService.IsMovieFavorite(model, userId);
            if (!movie)
            {
                return NotFound(new { errorMessage = "Movie is not favorite" });
            }

            return Ok(movie);
        }

        //-------------------------------- Review --------------------------------------

    }
}

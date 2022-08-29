using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryContracts;
using ApplicationCore.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IReviewRepository _reviewRepository;

        public UserService(IPurchaseRepository purchaseRepository, IUserRepository userRepository, IFavoriteRepository favoriteRepository, IReviewRepository reviewRepository)
        {
            _purchaseRepository = purchaseRepository;
            _userRepository = userRepository;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            if (await _purchaseRepository.CheckIfPurchaseExists(userId, purchaseRequest.MovieId))
                return true;
            return false;
        }

        public async Task<bool> IsMovieFavorite(FavoriteRequestModel favoriteRequest, int userId)
        {
            if (await _favoriteRepository.CheckIfFavoriteExists(userId, favoriteRequest.MovieId))
                return true;
            return false;
        }

        public async Task<bool> AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var newReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText,
                CreatedDate = reviewRequest.CreatedDate
            };

            await _reviewRepository.ReviewAdd(newReview);
            return true;
        }

        public async Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            var newFavorite = new Favorite
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            await _favoriteRepository.FavoriteAdd(newFavorite);
            return true;
        }

        public async Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var delFavorite = new Favorite
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            await _favoriteRepository.FavoriteRemove(delFavorite);
            return true;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            var newPurchase = new Purchase
            {
                MovieId = purchaseRequest.MovieId,
                UserId = userId,
                TotalPrice = purchaseRequest.Price,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime,
                PurchaseNumber = purchaseRequest.PurchaseNumber
            };

            var savedPurchase = await _purchaseRepository.AddPurchase(newPurchase);
            if (savedPurchase.Id > 1)
            {
                return true;
            }
            return false;
        }

        public async Task<List<MovieCardModel>> GetAllPurchasesForUser(int id)
        {
            var purchases = await _purchaseRepository.GetById(id);
            var movieCards = new List<MovieCardModel>();
            foreach (var purchase in purchases)
            {
                movieCards.Add(new MovieCardModel { Id = purchase.MovieId, PosterUrl = purchase.Movie.PosterUrl, Title = purchase.Movie.Title });
            }
            return movieCards;
        }

        public async Task<PurchaseModel> GetPurchasesDetails(int userId, int movieId)
        {
            var purchase = await _purchaseRepository.FindMovieByUserId(movieId, userId);
            if (purchase == null)
                return null;
            var purchaseDetails = new PurchaseModel
            {
                MovieId = movieId,
                UserId = userId,
                TotalPrice = purchase.TotalPrice,
                PurchaseDateTime = purchase.PurchaseDateTime,
                PurchaseNumber = purchase.PurchaseNumber
            };
            return purchaseDetails;
        }


        public async Task<List<MovieCardModel>> GetAllFavoritesForUser(int id)
        {
            var favorites = await _favoriteRepository.GetById(id);
            var movieCards = new List<MovieCardModel>();
            foreach (var favorite in favorites)
            {
                movieCards.Add(new MovieCardModel { Id = favorite.MovieId, PosterUrl = favorite.Movie.PosterUrl, Title = favorite.Movie.Title });
            }
            return movieCards;
        }

        public async Task<bool> DeleteMovieReview(ReviewRequestModel reviewRequest)
        {
            var review = await _reviewRepository.GetById(reviewRequest.UserId, reviewRequest.MovieId);
            await _reviewRepository.ReviewRemove(review);
            return true;
        }

        public async Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var UpdatedReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText,
                CreatedDate = reviewRequest.CreatedDate
            };

            await _reviewRepository.ReviewUpdate(UpdatedReview);
            return true;
        }

        public Task<List<ReviewModel>> GetAllReviewsByUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReviewModel> GetReview(int userId, int movieId)
        {
            var review = await _reviewRepository.GetById(userId, movieId);
            var model = new ReviewModel
            {
                MovieId = movieId,
                UserId = userId,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };

            return model;
        }

        public async Task<UserProfileResponseModel> GetUserDetailsById(int id)
        {
            var userDetails = await _userRepository.GetUserById(id);
            if (userDetails == null)
            {
                throw new Exception("This user is not exsit");
            }

            var model = new UserProfileResponseModel
            {
                id = userDetails.Id,
                email = userDetails.Email,
                firstName = userDetails.FirstName,
                lastName = userDetails.LastName,
                dateOfBirth = userDetails.DateOfBirth,
                phoneNumber = userDetails.PhoneNumber,
                profilePictureUrl = userDetails.ProfilePictureUrl,
                //roles
            };

            return model;
        }
    }
}

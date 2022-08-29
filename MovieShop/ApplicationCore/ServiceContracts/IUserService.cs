using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceContracts
{
    public interface IUserService
    {
        Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        Task<List<MovieCardModel>> GetAllPurchasesForUser(int id);
        Task<PurchaseModel> GetPurchasesDetails(int userId, int movieId);

        Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest);
        Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest);
        Task<List<MovieCardModel>> GetAllFavoritesForUser(int id);

        Task<bool> AddMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> DeleteMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest);
        Task<List<ReviewModel>> GetAllReviewsByUser(int id);
        Task<ReviewModel> GetReview(int userId, int movieId);
        Task<bool> IsMovieFavorite(FavoriteRequestModel favoriteRequest, int userId);
        Task<UserProfileResponseModel> GetUserDetailsById(int id);
    }
}

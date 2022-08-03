using ApplicationCore.RepositoryContracts;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;

        public ReviewRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<bool> CheckIfReviewExists(int userId, int movieId)
        {
            var review = await _movieShopDbContext.Reviews
                .Where(r => r.UserId == userId && r.MovieId == movieId)
                .FirstOrDefaultAsync();
            return review != null;
        }

        public async Task<Review> GetById(int userId, int movieId)
        {
            var review = await _movieShopDbContext.Reviews
                .Where(r => r.UserId == userId && r.MovieId == movieId)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync();
            return review;
        }

        public async Task<Review> ReviewRemove(Review review)
        {
            _movieShopDbContext.Reviews.Remove(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review> ReviewUpdate(Review review)
        {
            var UpdatedReview = await GetById(review.UserId, review.MovieId);
            UpdatedReview.Rating = review.Rating;
            UpdatedReview.ReviewText = review.ReviewText;
            UpdatedReview.CreatedDate = review.CreatedDate;
            await _movieShopDbContext.SaveChangesAsync();
            return UpdatedReview;
        }

        public async Task<Review> ReviewAdd(Review review)
        {
            _movieShopDbContext.Reviews.Add(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }
    }
}
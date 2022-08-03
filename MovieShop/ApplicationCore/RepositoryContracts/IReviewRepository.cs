using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IReviewRepository
    {
        Task<Review> GetById(int userId, int movieId);
        Task<Review> ReviewAdd(Review review);
        Task<Review> ReviewRemove(Review review);
        Task<Review> ReviewUpdate(Review review);
        Task<bool> CheckIfReviewExists(int userId, int movieId);
    }
}

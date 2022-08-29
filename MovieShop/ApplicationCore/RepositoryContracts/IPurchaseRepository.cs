using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetById(int userId);
        Task<Purchase> AddPurchase(Purchase purchase);
        Task<bool> CheckIfPurchaseExists(int userId, int movieId);
        Task<Purchase> FindMovieByUserId(int movieId, int userId);
    }
}

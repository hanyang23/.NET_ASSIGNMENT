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
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }

        public async Task<CastModel> GetCastDetails(int castId)
        {
            var cast = await _castRepository.GetById(castId);

            string gender;
            if (cast.Gender == "1") gender = "Female";
            else if (cast.Gender == "2") gender = "Male";
            else gender = "Not available";

            var castDetails = new CastModel
            {
                Id = cast.Id,
                Name = cast.Name,
                ProfilePath = cast.ProfilePath
                Gender = gender
            };

            foreach (var movie in cast.MoviesOfCast)
            {
                castDetails.Movies.Add(new MovieCardModel
                {
                    Id = movie.MovieId,
                    Title = movie.Movie.Title,
                    PosterUrl = movie.Movie.PosterUrl
                });
            }
            return castDetails;
        }
    }
}

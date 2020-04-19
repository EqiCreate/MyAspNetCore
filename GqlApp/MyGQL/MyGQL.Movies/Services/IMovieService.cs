using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGQL.Movies.Services
{
    public interface IMovieService
    {
        Task<Movie> GetByIdAsync(int id);
        Task<IEnumerable<Movie>> GetAsync();
        Task<Movie> CteateAsync(Movie movie);
    }
}

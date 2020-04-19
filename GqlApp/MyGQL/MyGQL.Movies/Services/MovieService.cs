using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGQL.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IList<Movie> _movies;
        public MovieService()
        {
            _movies = new List<Movie>() { 
                new Movie{ Id=1,Name="a",ActorId=1,Company="a",MovieRating=Models.MovieRating.G,ReleaseDate=new DateTime(1999,1,1)},
                new Movie{ Id=2,Name="b",ActorId=2,Company="b",MovieRating=Models.MovieRating.PG,ReleaseDate=new DateTime(2010,1,1)},
                new Movie{ Id=3,Name="c",ActorId=3,Company="c",MovieRating=Models.MovieRating.NC17,ReleaseDate=new DateTime(2012,1,1)},

            };
        }
        public Task<Movie> CteateAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAsync()
        {
            return Task.FromResult(this._movies.AsEnumerable());
        }

        public Task<Movie> GetByIdAsync(int id)
        {
            return Task.FromResult(this._movies.SingleOrDefault(x => x.Id == id));
        }
    }
}

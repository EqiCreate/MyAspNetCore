using MyGQL.Movies.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGQL.Movies.Services
{
   public interface IActorService
    {
        Task<Actor> GetByIdAsync(int id);
        Task<IEnumerable<Actor>> GetAsync();
    }
}

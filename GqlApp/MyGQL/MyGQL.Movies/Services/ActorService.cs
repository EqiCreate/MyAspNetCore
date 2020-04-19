using MyGQL.Movies.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGQL.Movies.Services
{
    public class ActorService : IActorService
    {
        private readonly IList<Actor> _actors;
        public ActorService()
        {
            _actors = new List<Actor>() {
                new Actor{ Id=1,Name="jane1"},
                new Actor{ Id=2,Name="jane2"},
                new Actor{ Id=3,Name="jane3"},
                new Actor{ Id=4,Name="jane4"},
                new Actor{ Id=5,Name="jane5"},

            };
        }
        public Task<IEnumerable<Actor>> GetAsync()
        {
            return Task.FromResult(this._actors.AsEnumerable());
        }

        public Task<Actor> GetByIdAsync(int id)
        {
            return Task.FromResult(this._actors.SingleOrDefault(x => x.Id == id));
        }
    }
}

using GraphQL.Types;
using MyGQL.Movies.Models;

namespace MyGQL.Movies.Schema
{
    public class ActorType : ObjectGraphType<Actor>
    {
        public ActorType()
        {
            Field(x => x.Id);
            Field(x => x.Name);
        }
    }
}

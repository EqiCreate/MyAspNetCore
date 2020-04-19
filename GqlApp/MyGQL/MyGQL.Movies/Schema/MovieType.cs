using GraphQL.Types;
using MyGQL.Movies.Models;
using MyGQL.Movies.Services;

namespace MyGQL.Movies.Schema
{
    public class MovieType:ObjectGraphType<Movie>
    {
        public MovieType(IActorService actorService)
        {
            Name = nameof(Movie);
            Description = "";
            Field(x=>x.Id);
            Field(x => x.Company);
            Field(x => x.Name);
            Field(x => x.ReleaseDate);
            Field(x => x.ActorId);
            Field<ActorType>("actor", resolve:  context=>actorService.GetByIdAsync(context.Source.ActorId));//?
            Field<MovieRatingEnum>("movieratings", resolve: context => context.Source.MovieRating);//?
            Field<StringGraphType>("customString", resolve: context => "asdf");//?


        }
    }
}

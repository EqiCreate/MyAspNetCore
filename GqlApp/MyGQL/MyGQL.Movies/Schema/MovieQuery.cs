using GraphQL.Types;
using MyGQL.Movies.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGQL.Movies.Schema
{
    public class MovieQuery: ObjectGraphType
    {
        public MovieQuery(IMovieService movieService)
        {
            Name = "Query";
            Field<ListGraphType<MovieType>>("movies",resolve:context=>movieService.GetAsync() );
        }
    }
}

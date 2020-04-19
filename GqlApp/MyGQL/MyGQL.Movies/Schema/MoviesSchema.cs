using GraphQL.Types;
using GraphQL;
using System;

namespace MyGQL.Movies.Schema
{
    public class MoviesSchema: GraphQL.Types.Schema
    {
        public MoviesSchema(IDependencyResolver dependencyResolver,MovieQuery movieQuery)/*:base(dependencyResolver)*/
        {
            this.DependencyResolver = dependencyResolver;
            this.Query = movieQuery;
        }
    }
}

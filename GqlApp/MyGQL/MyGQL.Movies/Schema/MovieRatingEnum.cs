using GraphQL.Types;
using MyGQL.Movies.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGQL.Movies.Schema
{
   public class MovieRatingEnum: EnumerationGraphType<MovieRating>
    {
        public MovieRatingEnum()
        {
            Name = nameof(MovieRating);
            Description = "";
            AddValue(MovieRating.G.ToString(), MovieRating.G.ToString(), MovieRating.G);
            AddValue(MovieRating.PG.ToString(), MovieRating.PG.ToString(), MovieRating.G);
            AddValue(MovieRating.PG13.ToString(), MovieRating.PG13.ToString(), MovieRating.G);
            AddValue(MovieRating.NC17.ToString(), MovieRating.NC17.ToString(), MovieRating.G);
            AddValue(MovieRating.R.ToString(), MovieRating.R.ToString(), MovieRating.G);
            AddValue(MovieRating.Unrated.ToString(), MovieRating.Unrated.ToString(), MovieRating.G);
        }
    }
}

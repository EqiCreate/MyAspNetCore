﻿using System;

namespace MyGQL.Movies.Models
{
    [Flags]
    public enum MovieRating
    {
        Unrated=0,
        G=1,
        PG=2,
        PG13=3,
        R=4,
        NC17=5
    }
}

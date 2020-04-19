using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyGQL.Movies.Schema;
using MyGQL.Movies.Services;
using System;


namespace MyGQL.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IActorService, ActorService>();
            //services.AddSingleton<IDependencyResolver>(s=>new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<MovieType>();
            services.AddSingleton<ActorType>();
            services.AddSingleton<MovieRatingEnum>();
            services.AddSingleton<MovieQuery>();
            services.AddSingleton<MoviesSchema>();
            services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = true;
                    //  options.UnhandledExceptionDelegate = ctx => { Console.WriteLine(ctx.OriginalException) };
                })
                // Add required services for de/serialization
                // .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { }) // For .NET Core 3+
                // .AddNewtonsoftJson(deserializerSettings => { }, serializerSettings => { }) // For everything else
                .AddWebSockets() // Add required services for web socket support
                .AddDataLoader();// Add required services for DataLoader support
                //.AddGraphTypes(typeof(MoviesSchema)); // Add all IGraphType implementors in assembly which ChatSchema exists 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseMiddleware<GraphQLMiddleware>(new GraphQLSettings
            //{
            //    BuildUserContext = ctx => new GraphQLUserContext
            //    {
            //        User = ctx.User
            //    }
            //});

            // this is required for websockets support
            app.UseWebSockets();

            // use websocket middleware for ChatSchema at path /graphql
            app.UseGraphQLWebSockets<MoviesSchema>("/graphql");

            // use HTTP middleware for ChatSchema at path /graphql
            app.UseGraphQL<MoviesSchema>("/graphql");

            // use graphql-playground middleware at default url /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            // use graphiQL middleware at default url /ui/graphiql
            app.UseGraphiQLServer(new GraphiQLOptions());
        }
    }
}

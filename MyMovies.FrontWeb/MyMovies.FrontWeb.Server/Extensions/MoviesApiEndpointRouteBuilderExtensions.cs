using MyMovies.FrontWeb.Server.Models;
using System.Net.Http.Headers;

namespace MyMovies.FrontWeb.Server.Extensions;

public static class MoviesApiEndpointRouteBuilderExtensions
{
    public record class Token(string Access_token, string expires_in, string token_type, string scope)
    {
    }

    private static async Task<string> GetAccessToken()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:5001");

        // Reponse en JSON 
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // Data à envoyer en POST
        List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
        postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        postData.Add(new KeyValuePair<string, string>("client_id", "bbfapplication"));
        postData.Add(new KeyValuePair<string, string>("client_secret", "bbfapplication"));
        FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

        // Post sur le client
        HttpResponseMessage response = await client.PostAsync("connect/token", content);

        Token jsonString = await response.Content.ReadAsAsync<Token>();

        // return the Access Token.
        return jsonString.Access_token;

    }
    private static HttpClient AddBearer(this HttpClient client)
    {
        var access_token = GetAccessToken().GetAwaiter().GetResult();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
        return client;
    }

    public static IEndpointConventionBuilder MapMoviesApi(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var routeGroup = endpoints.MapGroup("api/movies");

        // Url de l'API Movies
        var baseUri = new Uri(endpoints.ServiceProvider.GetService<IConfiguration>()!["MoviesAPI:BaseUrl"]!);

        // MapGet
        routeGroup.MapGet("/", async Task<IResult> (HttpContext context, HttpClient client) =>
        {
            try
            {
                List<MovieViewModel> result = new();
                HttpResponseMessage response = await client.AddBearer().GetAsync($"{baseUri}/");
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<MovieViewModel>>();
                }
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        // MapGet
        routeGroup.MapGet("/{id}", async Task<IResult> (HttpContext context, HttpClient client, int id) =>
        {
            try
            {
                MovieViewModel? result = null;
                HttpResponseMessage response = await client.AddBearer().GetAsync($"{baseUri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<MovieViewModel>();
                }

                if (result == null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        // MapPost
        routeGroup.MapPost("/", async Task<IResult> (HttpContext context, HttpClient client, MovieViewModel value) =>
        {
            try
            {
                MovieViewModel? result = null;
                HttpResponseMessage response = await client.AddBearer().PostAsJsonAsync<MovieViewModel>($"{baseUri}/", value);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<MovieViewModel>();
                }
                if (result == null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        // MapPut
        routeGroup.MapPut("/{id}", async Task<IResult> (HttpContext context, HttpClient client, int id, MovieViewModel value) =>
        {
            try
            {
                MovieViewModel? result = null;
                HttpResponseMessage response = await client.AddBearer().PutAsJsonAsync<MovieViewModel>($"{baseUri}/{id}", value);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<MovieViewModel>();
                }
                if (result == null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        // MapDelete
        routeGroup.MapDelete("/{id}", async Task<IResult> (HttpContext context, HttpClient client, int id) =>
        {
            try
            {
                HttpResponseMessage response = await client.AddBearer().DeleteAsync($"{baseUri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return TypedResults.NoContent();
                }
                else
                {
                    return TypedResults.NotFound();
                }
            }
            catch (Exception ex)
            {
                //log error
                return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        return routeGroup;
    }
}


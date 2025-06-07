using System.Net.Http.Json;
using System.Net.Http.Headers;
using api.Tests.Fakers;
using System.Net;
using api.Models;
using api.Models.Common;

namespace api.Tests.Controller;

[TestClass]
[TestCategory("Controller")]
public class MovieControllerTests 
{
    private static Movie _movie = null!;
    private static HttpClient _client = null!;
    private static RecordUser _recordUser = null!;
    private static UserAuth? _userAuth = null!;
    
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext contex)
    {
        _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:8080") // ajuste conforme necessário
        };
        
        _recordUser = UserFaker.GenerateRecordUser();
        
        await _client.PostAsJsonAsync("/auth/signup", new {
            name = _recordUser.Name,
            email = _recordUser.Email,
            password = _recordUser.Password
        });

        var response = await _client.PostAsJsonAsync("/auth/sign", new {
            email = _recordUser.Email,
            password = _recordUser.Password
        });

       if(response is not null && response.IsSuccessStatusCode) 
       {
            _userAuth = (await response.Content.ReadFromJsonAsync<UserAuth>());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", _userAuth?.Token
            );
       }
       else Console.WriteLine("Erro na autenticação");
    }

    [TestMethod]
    [Priority(1)]
    public async Task Add_ShouldReturnOk_WhenCreatedMovie()
    {
        var movie = MovieFaker.GenerateFakeMovie();

        var response = await _client.PostAsJsonAsync("/movies/new", new {
            title = movie.Title,
            director = movie.Director,
            stock = movie.Stock,
            coversource = movie.CoverSource,
            source = movie.Source
        });

        var resultMovie = await response.Content.ReadFromJsonAsync<Movie>();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(resultMovie?.Id);
        _movie = resultMovie;
    }

    [TestMethod]
    [DataRow("", "Albert Spiers", 10)] // invalid title
    [DataRow("Senhor dos Anéis", "", 15)] // invalid director
    [DataRow("A Freira", "Albert Spiers", 0)] // invalid stock
    public async Task Add_ShouldReturnBadRequest_WhenInvalidFields(
            string title, 
            string director,
            int stock)
    {
        var movie = MovieFaker.GenerateFakeMovie();
        var response = await _client.PostAsJsonAsync("/movies/new", new {
            title = title,
            director = director,
            stock = stock,
            coversource = movie.CoverSource,
            source = movie.Source
        });

        var resultMovie = await response.Content.ReadFromJsonAsync<Movie>();

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    [Priority(2)]
    public async Task Get_ShouldReturnMovie_WhenIsValidId()
    {
        var response = await _client.GetAsync($"/movies/movie/{_movie.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var movie = await response.Content.ReadFromJsonAsync<Movie>();

        Assert.IsNotNull(movie?.Id);
    }
    
    [TestMethod]
    [DataRow("Freira", 10, 1)]
    public async Task Seach_ShouldReturnResults_WhenValidFields(
        string searchTerm,
        int itemsPerPage,
        int page
        )
    {
        var response = await _client.PostAsJsonAsync("/movies/search", new {
            searchTerm,
            itemsPerPage,
            page
        });

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<Result<Movie>>();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow("Fr", 10, 1)] // invalid searchTerm
    [DataRow("Freira", 0, 1)] // invalid intemsPerPage
    [DataRow("A Freira", 10, 0)] // invalid page
    public async Task Seach_ShouldReturnBadRequest_WhenInvalidFields(
        string searchTerm,
        int itemsPerPage,
        int page
        )
    {
        var response = await _client.PostAsJsonAsync("/movies/search", new {
            searchTerm,
            itemsPerPage,
            page
        });

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Rent_ShouldReturnOK_WhenHaveTheMovieInStock()
    {
        var response = await _client.GetAsync($"/movies/rent/{_movie.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

}

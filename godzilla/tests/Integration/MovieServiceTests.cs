using api.Models;
using api.Service;
using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Tests.Integration.Faker;

namespace api.Tests.Integration;

[TestClass]
public class MovieServiceTests 
{
    private static AppDbContex _dbContext = null!;
    private static MovieService _movieService = null!;
    private static Movie _testMovie = null!;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        var options = new DbContextOptionsBuilder<AppDbContex>()
            .UseInMemoryDatabase("UserServiceTestDb")
            .Options;
        
        _dbContext = new AppDbContex(options);
        _movieService = new MovieService(_dbContext);

        _testMovie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "A Freira",
            Director = "Christopher Nolan",
            Stock = 3,
            CoverSource = "http://assets.com/inception.jpg",
            Source = "http://assets.com/inception.mp4",
            CreatedAt = DateTime.Now
        };

        await _movieService.AddAsync(_testMovie);
        
        var fakeMovies = MovieFaker.GenerateFakeMovies(50);
        await _dbContext.Movies.AddRangeAsync(fakeMovies);

        await _dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task AddAsync_ShouldAddMovieSuccefully()
    {
        var record = new RecordMovie
        {
            Title = "Vingadores Gerra Infinita",
            Director = "Christopher Nolan",
            Stock = 3,
            CoverSource = "http://assets.com/inception.jpg",
            Source = "http://assets.com/inception.mp4"
        };

        var result =  await _movieService.AddAsync(record);

        Assert.IsTrue(result.Success);
        Assert.AreEqual(result.Message, "Filme adicionado com sucesso!");
    }

    [TestMethod]
    public async Task GetById_ShouldReturnMovie_WhenIdExist()
    {
        var result = await _movieService.GetById(_testMovie.Id);

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(result.Data.Id, _testMovie.Id);
    }

    [TestMethod]
    public async Task GetById_ShouldFail_WhenDoesNotExist()
    {
        var result = await _movieService.GetById(Guid.NewGuid());

        Assert.IsFalse(result.Success);
        Assert.AreEqual(result.Message, "Filme não encontrado!");
    }

    [TestMethod]
    public async Task SearchByTitle_ShouldReturnResults_WhenTitleMatches()
    {
        var model = new MovieSearch
        {
            SearchTerm = "Freira",
            Page = 1,
            ItemsPerPage = 10
        };

        var response = await _movieService.SearchByTitle(model);

        Assert.IsTrue(response.Success);
        Assert.IsTrue(response.Data?.Items.Any(m => m.Id == _testMovie.Id));
        Assert.IsTrue(response.Data!.TotalItems > 0);
    }

    [TestMethod]
    public async Task SearchByTitle_ShouldReturnEmpty_WhenNoMatch()
    {
        var model = new MovieSearch
        {
            SearchTerm = "Nonexistent",
            Page = 1,
            ItemsPerPage = 10
        };

        var response = await _movieService.SearchByTitle(model);

        Assert.IsTrue(response.Success);
        Assert.AreEqual(0, response.Data!.TotalItems);
        Assert.AreEqual(0, response.Data.Items.Count);
    }

    [TestMethod]
    public async Task RentMovie_ShouldReturnMovie_WhenInStock()
    {
        var response = await _movieService.RentMovie(_testMovie.Id);

        Assert.IsTrue(response.Success);
        Assert.AreEqual(response.Data?.Id, _testMovie.Id);
    }

    [TestMethod]
    public async Task RentMovie_ShouldFail_WhenOutOfStock()
    {
        var outOfStock = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "No Stock Movie",
            Director = "Somebody",
            Stock = 0,
            CoverSource = "nostock.jpg",
            Source = "nostock.mp4"
        };

        _dbContext.Movies.Add(outOfStock);
        await _dbContext.SaveChangesAsync();

        var response = await _movieService.RentMovie(outOfStock.Id);

        Assert.IsFalse(response.Success);
        Assert.AreEqual("Filme indisponível em estoque!", response.Message);
    }

    [TestMethod]
    public async Task RentMovie_ShouldFail_WhenMovieNotFound()
    {
        var response = await _movieService.RentMovie(Guid.NewGuid());

        Assert.IsFalse(response.Success);
        Assert.AreEqual("Filme não encontrado!", response.Message);
    }     
}


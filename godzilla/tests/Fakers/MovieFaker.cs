
using Bogus;
using api.Models;

namespace api.Tests.Fakers;

public static class MovieFaker 
{
    public static List<Movie> GenerateFakeMovies(int count)
    {
        var faker = new Faker<Movie>()
            .RuleFor(m => m.Id, f => Guid.NewGuid())
            .RuleFor(m => m.Title, f => f.Lorem.Sentence(3))
            .RuleFor(m => m.Director, f => f.Person.FullName)
            .RuleFor(m => m.Stock, f => f.Random.Int(0, 15))
            .RuleFor(m => m.CoverSource, f => f.Image.PicsumUrl())
            .RuleFor(m => m.Source, f => f.Internet.Url());

        return faker.Generate(count);
    }
}

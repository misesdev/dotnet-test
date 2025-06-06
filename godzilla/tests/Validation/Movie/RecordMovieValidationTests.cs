
using api.Models;
using api.Tests.Validation.Helpers;

namespace api.Tests.Validation.Movie;

[TestClass]
[TestCategory("Validation")]
public class RecordMovieValidationTests
{
    [TestMethod]
    public void RecordMovie_InvalidData_ShouldFailValidation()
    {
        var model = new RecordMovie
        {
            Title = "abc",
            Director = "",
            Stock = 0,
            CoverSource = "invalid-url",
            Source = "also-invalid-url"
        };

        var results = ValidationHelper.Validate(model);
        Assert.AreEqual(5, results.Count);

        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("`Title` deve ter um tamanho mínimo")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("`Director` é um campo obrigatório")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("`Stock` deve ser maior que zero")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("CoverSource")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("Source")));
    }

    [TestMethod]
    public void RecordMovie_ValidData_ShouldPassValidation()
    {
        var model = new RecordMovie
        {
            Title = "O Senhor dos Anéis",
            Director = "Peter Jackson",
            Stock = 5,
            CoverSource = "https://example.com/cover.jpg",
            Source = "https://example.com/movie.mp4"
        };

        var results = ValidationHelper.Validate(model);
        Assert.AreEqual(0, results.Count);
    }
}

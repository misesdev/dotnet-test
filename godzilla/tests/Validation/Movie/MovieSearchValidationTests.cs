using api.Tests.Validation.Helpers;
using api.Models;

namespace api.Tests.Validation.Movie;

[TestClass]
[TestCategory("validation")]
public class MovieSearchValidationTests
{
    [TestMethod]
    public void MovieSearch_InvalidData_ShouldFailValidation()
    {
        var model = new MovieSearch
        {
            SearchTerm = "a",  // muito curto
            Page = 0,
            ItemsPerPage = 0
        };

        var results = ValidationHelper.Validate(model);
        Assert.AreEqual(3, results.Count);

        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("SearchTerm")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("Page")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("ItemsPerPage")));
    }

    [TestMethod]
    public void MovieSearch_ValidData_ShouldPassValidation()
    {
        var model = new MovieSearch
        {
            SearchTerm = "Matrix",
            Page = 1,
            ItemsPerPage = 10
        };

        var results = ValidationHelper.Validate(model);
        Assert.AreEqual(0, results.Count);
    }
}

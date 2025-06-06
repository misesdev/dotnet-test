using api.Tests.Validation.Helpers;
using api.Models;

namespace api.Tests.Validation.Movie;

[TestClass]
[TestCategory("validation")]
public class MovieSearchValidationTests
{
    [TestMethod]
    [DataRow("d", 1, 10)] // invalid search term
    [DataRow("Teste Search", 0, 10)] // invalid page number
    [DataRow("Teste busca", 1, 0)] // invalid items per page number
    public void MovieSearch_InvalidData_ShouldFailValidation(string term, int page, int items)
    {
        var model = new MovieSearch
        {
            SearchTerm = term,  // muito curto
            Page = page,
            ItemsPerPage = items
        };

        var results = ValidationHelper.Validate(model);
        Assert.IsTrue(results.Count != 0);
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

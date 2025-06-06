
using api.Models;
using api.Tests.Validation.Helpers;

namespace api.Tests.Validation.User;

[TestClass]
[TestCategory("Validation")]
public class RecordUserValidationTests
{
    [TestMethod]
    public void RecordUser_InvalidFields_ShouldFailValidation()
    {
        var user = new RecordUser
        {
            Name = "Jo", // muito curto
            Email = "invalid-email", // inválido
            Password = "123" // muito curto
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(3, results.Count);

        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("Name")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("e-mail fornecido está inválido")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("password")));
    }

    [TestMethod]
    public void RecordUser_ValidFields_ShouldPassValidation()
    {
        var user = new RecordUser
        {
            Name = "João da Silva",
            Email = "joao@example.com",
            Password = "securePassword123"
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(0, results.Count);
    }
}

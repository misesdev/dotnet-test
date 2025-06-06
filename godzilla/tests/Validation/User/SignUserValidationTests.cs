
using api.Models;
using api.Tests.Validation.Helpers;

namespace api.Tests.Validation.User;

[TestClass]
[TestCategory("Validation")]
public class SignUserValidationTests
{
    [TestMethod]
    public void SignUser_InvalidFields_ShouldFailValidation()
    {
        var user = new SignUser
        {
            Email = "abc", // muito curto e inválido
            Password = "123" // muito curto
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(3, results.Count);

        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("e-mail fornecido está inválido")));
        Assert.IsTrue(results.Any(r => r.ErrorMessage!.Contains("senha") || r.ErrorMessage!.Contains("password")));
    }

    [TestMethod]
    public void SignUser_ValidFields_ShouldPassValidation()
    {
        var user = new SignUser
        {
            Email = "mises@dev.com",
            Password = "@#$Ffdf2311"
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(0, results.Count);
    }
}


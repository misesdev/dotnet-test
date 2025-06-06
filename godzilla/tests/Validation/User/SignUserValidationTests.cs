
using api.Models;
using api.Tests.Validation.Helpers;

namespace api.Tests.Validation.User;

[TestClass]
[TestCategory("Validation")]
public class SignUserValidationTests
{
    [TestMethod]
    [DataRow("gmail.com", "1234Rdsd")]
    [DataRow("misesdev@@gmailll", "1234Rdsd")]
    [DataRow("euclides@mises.com", "!@wdwesd")]
    public void SignUser_InvalidFields_ShouldFailValidation(string email, string password)
    {
        var user = new SignUser
        {
            Email = email, // muito curto e inválido
            Password = password // muito curto
        };

        var results = ValidationHelper.Validate(user);

        Assert.IsTrue(results.Count > 0);
    }

    [TestMethod]
    [DataRow("mises@dev.com", "@#$Ffdf2311")]
    [DataRow("misesdev@gmail.com", "@#$#@#DFfdfdfdfgv564343")]
    public void SignUser_ValidFields_ShouldPassValidation(string email, string password)
    {
        var user = new SignUser
        {
            Email = email,
            Password = password
        };

        var results = ValidationHelper.Validate(user);
        
        Assert.AreEqual(0, results.Count);
    }
}



using api.Models;
using api.Tests.Validation.Helpers;

namespace api.Tests.Validation.User;

[TestClass]
[TestCategory("Validation")]
public class RecordUserValidationTests
{
    [TestMethod]
    [DataRow("Mises Dev", "mises@gmail.com", "4erdd3")] // invalid password
    [DataRow("M", "mises@gmail.com", "#$@@#weddSDe43233")] // invalid name
    [DataRow("Mises dev", "mises@@gmail.com", "#$@@#weddSDe43233")] // invalid email
    public void RecordUser_InvalidFields_ShouldFailValidation(string name, string email, string password)
    {
        var user = new RecordUser {
            Name = name, 
            Email = email, 
            Password = password 
        };

        var results = ValidationHelper.Validate(user);

        Assert.IsTrue(results.Count > 0);
    }

    [TestMethod]
    [DataRow("Mises Dev", "evale@gmail.com", "#$@@#weddSDe43233")]
    [DataRow("João da Silva", "mises@gmail.com", "#$@@#weddSDe43233")]
    public void RecordUser_ValidFields_ShouldPassValidation(string name, string email, string password)
    {
        var user = new RecordUser {
            Name = name,
            Email = email,
            Password = password
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(0, results.Count);
    }
}

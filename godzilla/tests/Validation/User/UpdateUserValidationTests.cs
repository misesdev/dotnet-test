using api.Tests.Validation.Helpers;
using api.Models;

namespace api.Tests.Validation.User;

[TestClass]
[TestCategory("Validation")]
public class UpdateUserValidationTests
{
    [TestMethod]
    [DataRow("M", "mises@gmail.com")] // invalid name
    [DataRow("Mises dev", "mises@@gmail.com")] // invalid email
    public void UpdateUser_InvalidFields_ShouldFailValidation(string name, string email)
    {
        var user = new UpdateUserDTO {
            Name = name, // muito curto
            Email = email, // inválido
        };

        var results = ValidationHelper.Validate(user);

        Assert.IsTrue(results.Count > 0);
    }

    [TestMethod]
    [DataRow("Mises Dev", "evale@gmail.com")]
    [DataRow("João da Silva", "mises@gmail.com")]
    public void UpdateUser_ValidFields_ShouldPassValidation(string name, string email)
    {
        var user = new UpdateUserDTO {
            Name = name,
            Email = email,
        };

        var results = ValidationHelper.Validate(user);
        Assert.AreEqual(0, results.Count);
    }
}

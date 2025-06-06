using System.Net.Http.Json;
using System.Net;
using api.Models;
using api.Tests.Fakers;

namespace api.Tests.Controller;

[TestClass]
[TestCategory("Controller")]
public class AuthControllerTests
{
    private static RecordUser _recordUser = null!;
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext contex)
    {
        _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:8080") // ajuste conforme necessário
        };
        
        _recordUser = UserFaker.GenerateRecordUser();
    }

    [TestMethod]
    [Priority(1)]
    public async Task SignUp_ShouldReturnOk_WhenUserIsValid()
    {
        var user = new
        {
            name = _recordUser.Name,
            email = _recordUser.Email,
            password = _recordUser.Password
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", user);

        var content = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, content);
    }

    [TestMethod]
    [DataRow("Mi", "mises@dev.com", "@3123Dfse$3")] // invalid name
    [DataRow("Mises Dev", "mises@@dev.com", "@3123Dfse$3")] // invalid email
    [DataRow("Mises Dev", "mises@dev.com", "123")] // invalid password name
    public async Task SignUp_ShouldReturnBadRequest_WhenUserIsInvalid(
        string name,
        string email,
        string password
        )
    {
        var body = new
        {
            name = name,
            email = email,
            password = password
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", body);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    [DataRow(null, "1234")] // invalid password
    [DataRow(null, "23#$Der#234Wsdd")] // incorrect password 
    [DataRow("mises@@frame..com", "23#$Der#234Wwed")] // invalid email
    [DataRow("mises12345@euclides.eu", "23#$Der#234Wwed")] // invalid email (not exists) 
    public async Task Sign_ShouldReturnBadRequest_WhenCredentialIsInvalid(
        string email, 
        string password
        )
    {
        var login = new
        {
            email = email ?? _recordUser.Email,
            password = password
        };

        var response = await _client.PostAsJsonAsync("/auth/sign", login);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    [Priority(2)]
    public async Task Sign_ShouldReturnOK_WhenCredentialIsValid()
    {
        var login = new
        {
            email = _recordUser.Email,
            password = _recordUser.Password
        };

        var response = await _client.PostAsJsonAsync("/auth/sign", login);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}

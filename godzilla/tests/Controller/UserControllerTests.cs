using api.Models;
using api.Tests.Fakers;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;

namespace api.Tests.Controller;

[TestClass]
[TestCategory("Controller")]
public class UserControllerTests 
{
    private static HttpClient _client = null!;
    private static UserAuth? _userAuth = null!;
    private static RecordUser _recordUser = null!;
    
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext contex)
    {
        _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:8080") // ajuste conforme necessário
        };
        
        _recordUser = UserFaker.GenerateRecordUser();
        
        await _client.PostAsJsonAsync("/auth/signup", new {
            name = _recordUser.Name,
            email = _recordUser.Email,
            password = _recordUser.Password
        });

        var response = await _client.PostAsJsonAsync("/auth/sign", new {
            email = _recordUser.Email,
            password = _recordUser.Password
        });
       
        if(response is not null && response.IsSuccessStatusCode) 
       {
            _userAuth = (await response.Content.ReadFromJsonAsync<UserAuth>());

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", _userAuth?.Token
            );
       }
       else Console.WriteLine("Erro na autenticação");
    }

    [TestMethod]
    public async Task Get_ShouldReturnOk_WhenUserExist()
    {
        var response = await _client.GetAsync($"/users/user/{_userAuth?.User?.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<User>();
        
        Assert.AreEqual(_userAuth?.User?.Id, user?.Id);
    }

    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenUserNotExist()
    {
        var response = await _client.GetAsync($"/users/user/{Guid.NewGuid()}");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Update_ShouldReturnOk_WhenValidFields()
    {
        var user = UserFaker.GenerateRecordUser();

        var response = await _client.PostAsJsonAsync("/users/update", new {
            name = user.Name,
            email = user.Email
        });

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    [DataRow("E", "elima@gmail.com")] // invalid name
    [DataRow("Elisson Vale", "elima@")] // invalid email
    public async Task Update_ShouldReturnBadRequest_WhenInvalidFields(
        string name,
        string email
        )
    {
        var response = await _client.PostAsJsonAsync("/users/update", new {
            name,
            email
        });

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

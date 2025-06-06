using api.Service;
using api.Models;
using api.Config;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.Tests.Service;

[TestClass]
public class TokenServiceTests
{
    private TokenService? _service;
    private JwtSettings? _jwtSettings;

    [TestInitialize]
    public void Setup()
    {
        _jwtSettings = new JwtSettings
        {
            Key = "4fe9dd19ebae4d8ca87d802bdc4239aa4e91743bdce84b0e9243b29b7fa616b6", // Deve ter pelo menos 16 caracteres
            Issuer = "godzilla-api",
            Audience = "godzilla-clients",
            ExpireMinutes = 60
        };

        var options = Options.Create(_jwtSettings);
        _service = new TokenService(options);
    }

    [TestMethod]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Name = "Test User"
        };

        // Act
        var token = _service?.GenerateToken(user);

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(token));
    }

    [TestMethod]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@test.com",
            Name = "User Test"
        };

        // Act
        var token = _service?.GenerateToken(user);

        // Decode token
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // Assert
        Assert.AreEqual(user.Id.ToString(), jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.AreEqual(user.Email, jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        Assert.AreEqual(user.Name, jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value);
    }

    [TestMethod]
    public void GenerateToken_ShouldBeValidAccordingToSettings()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "valid@test.com",
            Name = "Valid User"
        };

        var token = _service?.GenerateToken(user);

        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings?.Key ?? "");

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings?.Issuer,
            ValidAudience = _jwtSettings?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        // Act
        handler.ValidateToken(token, parameters, out var validatedToken);

        // Assert
        Assert.IsInstanceOfType(validatedToken, typeof(JwtSecurityToken));
    }
}

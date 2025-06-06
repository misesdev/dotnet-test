using Bogus;
using api.Models;
using System.Text;

namespace api.Tests.Fakers;

public static class UserFaker {

    public static SignUser GenerateSignUser() 
    {
        var faker = new Faker<SignUser>()
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => UserFaker.GeneratePassWord(24));

        return faker.Generate();
    }

    public static RecordUser GenerateRecordUser() 
    {
        var faker = new Faker<RecordUser>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => UserFaker.GeneratePassWord(15));

        return faker.Generate();
    }

    public static string GeneratePassWord(int size = 12)
    {
        var faker = new Faker();

        string[] letrasMaiusculas = { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        string[] letrasMinusculas = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        string[] numeros = {"0","1","2","3","4","5","6","7","8","9"};
        string[] especiais = {"!","@","#","$","%","^","&","*","(",")","_","-","+","=","<",">","?"};
        
        var todos = letrasMaiusculas.Concat(letrasMinusculas)
            .Concat(numeros)
            .Concat(especiais)
            .ToArray();

        var senha = new StringBuilder(letrasMaiusculas[faker.Random.Int(0, letrasMaiusculas.Length - 1)]);
        senha.Append(letrasMinusculas[faker.Random.Int(0, letrasMinusculas.Length - 1)]);
        senha.Append(numeros[faker.Random.Int(0, numeros.Length - 1)]);
        senha.Append(especiais[faker.Random.Int(0, especiais.Length - 1)]);
        senha.Append(letrasMinusculas[faker.Random.Int(0, letrasMinusculas.Length - 1)]);
        
        for (int i = senha.Length; i < size; i++)
        {
            senha.Append(todos[faker.Random.Int(0, todos.Length - 1)]);
        }

        return senha.ToString(); 
    }
}


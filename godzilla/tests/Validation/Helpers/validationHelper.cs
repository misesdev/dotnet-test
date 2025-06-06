
using System.ComponentModel.DataAnnotations;

namespace api.Tests.Validation.Helpers;

public static class ValidationHelper
{
    public static List<ValidationResult> Validate<Entity>(Entity model)
    {
        var context = new ValidationContext(model!);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model!, context, results, true);
        return results;
    }
}

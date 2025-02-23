using IndevLabs.Validation;
using static IndevLabs.Validation.ErrorCode;

namespace IndevLabs.Util;

public class ValidationUtils
{
    public static void AddUnknownWineError(IValidationStorage validationStorage, int id)
    {
        validationStorage.AddError(UnknownWine, $"Wine with Id {id} does not exist");
    }
}
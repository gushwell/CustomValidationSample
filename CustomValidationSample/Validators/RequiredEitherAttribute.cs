using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace CustomValidationSample.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredEitherAttribute : ValidationAttribute
{
    public string OtherProperty { get; set; }

    public RequiredEitherAttribute(string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
            return ValidationResult.Success;
        // 検証に依存するプロパティへの参照を取得
        var containerType = validationContext.ObjectType;
        var pinfo = containerType.GetProperty(OtherProperty);
        if (pinfo != null)
        {
            // 依存プロパティの値を取得
            var dependentvalue = pinfo.GetValue(validationContext.ObjectInstance, null);
            if (dependentvalue != null)
                return ValidationResult.Success;
        }
        var otherPropertyDisplayName = GetOtherPropertyDisplayName(validationContext.ObjectInstance, this.OtherProperty);

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, otherPropertyDisplayName));
    }

    private string FormatErrorMessage(string thisPropertyDisplayName, string otherPropertyDisplayName)
    {
        return String.Format(ErrorMessageString, thisPropertyDisplayName, otherPropertyDisplayName);
    }

    private static string GetOtherPropertyDisplayName<T>(T model, string otherPropertyName) where T: notnull
    {
        var type = model.GetType();
        var prop = type.GetProperty(otherPropertyName);
        if (prop == null)
            return "";
        var attr = prop.GetCustomAttribute<DisplayAttribute>();
        return attr?.Name ?? otherPropertyName;
    }
}


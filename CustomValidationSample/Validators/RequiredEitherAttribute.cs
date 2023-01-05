using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomValidationSample.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

public sealed class RequiredEitherAttribute : ValidationAttribute
{

    /// <summary>
    /// 内部で検証時に利用する RequiredAttribute。派生クラスで利用
    /// </summary>
    //private RequiredAttribute InnerAttribute { get; set; } = new RequiredAttribute();

    /// <summary>
    /// 依存するプロパティの名前
    /// </summary>
    public string OtherProperty { get; set; }

    /// <summary>
    /// 依存するプロパティの値 （等しい時に検証）
    /// </summary>
    //public IEnumerable<object> TargetValues { get; set; }


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="otherProperty"></param>
    public RequiredEitherAttribute(string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    /// <summary>
    /// 検証を実施
    /// </summary>
    /// <param name="value">検証する値</param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
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


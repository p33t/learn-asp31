using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.binder
{
    // Adapted from http://www.vickram.me/custom-datetime-model-binding-in-asp-net-core-web-api
    public class DateTimeModelBinder : IModelBinder
    {
        // public static readonly Type[] SUPPORTED_TYPES = new Type[] { typeof(DateTime), typeof(DateTime?) };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (!IsSupported(bindingContext.ModelType))
            {
                return Task.CompletedTask;
            }

            var modelName = GetModelName(bindingContext);

            // This is always 'None' when DateTime is supplied in Body (with or without a surrounding model)
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            var nullIsAllowed = bindingContext.ModelType == typeof(DateTime?);
            if (valueProviderResult == ValueProviderResult.None)
            {
                if (!nullIsAllowed)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }
                return Task.CompletedTask;
            }

            // bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var dateToParse = valueProviderResult.FirstValue;

            // original
            // if (string.IsNullOrEmpty(dateToParse))
            // {
                // return Task.CompletedTask;
            // }

            // var dateTime = ParseDate(bindingContext, dateToParse);

            // Null only allowed if type permits and no string is supplied
            if (nullIsAllowed && string.IsNullOrWhiteSpace(dateToParse))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                
            }
            else if (DateTime.TryParse(dateToParse, out var dateTime))
            {
                // Must have timezone specified
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }
                else
                {
                    if (dateTime.Kind == DateTimeKind.Local)
                    {
                        dateTime = dateTime.ToUniversalTime();
                    }

                    bindingContext.Result = ModelBindingResult.Success(dateTime);
                }
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
            
            return Task.CompletedTask;
        }

        public static bool IsSupported(Type modelType)
        {
            return typeof(DateTime) == modelType || typeof(DateTime?) == modelType;
        }

        // private DateTime? ParseDate(ModelBindingContext bindingContext, string dateToParse)
        // {
        //     var attribute = GetDateTimeModelBinderAttribute(bindingContext);
        //     var dateFormat = attribute?.DateFormat;
        //     
        //     if (string.IsNullOrEmpty(dateFormat))
        //     {
        //         return Helper.ParseDateTime(dateToParse);
        //     }
        //     
        //     return Helper.ParseDateTime(dateToParse, new string[] { dateFormat });
        // }

        // private DateTimeModelBinderAttribute GetDateTimeModelBinderAttribute(ModelBindingContext bindingContext)
        // {
        //     var modelName = GetModelName(bindingContext);
        //
        //     var paramDescriptor = bindingContext.ActionContext.ActionDescriptor.Parameters
        //         .Where(x => x.ParameterType == typeof(DateTime?))
        //         .Where((x) =>
        //         {
        //             // See comment in GetModelName() on why we do this.
        //             var paramModelName = x.BindingInfo?.BinderModelName ?? x.Name;
        //             return paramModelName.Equals(modelName);
        //         })
        //         .FirstOrDefault();
        //
        //     var ctrlParamDescriptor = paramDescriptor as ControllerParameterDescriptor;
        //     if (ctrlParamDescriptor == null)
        //     {
        //         return null;
        //     }
        //
        //     var attribute = ctrlParamDescriptor.ParameterInfo
        //         .GetCustomAttributes(typeof(DateTimeModelBinderAttribute), false)
        //         .FirstOrDefault();
        //
        //     return (DateTimeModelBinderAttribute)attribute;
        // }

        private string GetModelName(ModelBindingContext bindingContext)
        {
            // The "Name" property of the ModelBinder attribute can be used to specify the
            // route parameter name when the action parameter name is different from the route parameter name.
            // For instance, when the route is /api/{birthDate} and the action parameter name is "date".
            // We can add this attribute with a Name property [DateTimeModelBinder(Name ="birthDate")]
            // Now bindingContext.BinderModelName will be "birthDate" and bindingContext.ModelName will be "date"
            if (!string.IsNullOrEmpty(bindingContext.BinderModelName))
            {
                return bindingContext.BinderModelName;
            }

            return bindingContext.ModelName;
        }
    }
//
// public class DateTimeModelBinderAttribute : ModelBinderAttribute
// {
//     public string DateFormat { get; set; }
//
//     public DateTimeModelBinderAttribute()
//         : base(typeof(DateTimeModelBinder))
//     {
//     }
// }
}
﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            var elementtype = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];//类型 gudi
            var converter = TypeDescriptor.GetConverter(elementtype);//字符串转换器
            var values=value.Split(new[] { "," },StringSplitOptions.RemoveEmptyEntries).Select(x=>converter.ConvertFromString(x.Trim())).ToArray();//获取objects

            var typevalues = Array.CreateInstance(elementtype, values.Length);
            values.CopyTo(typevalues,0);
            bindingContext.Model = typevalues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}

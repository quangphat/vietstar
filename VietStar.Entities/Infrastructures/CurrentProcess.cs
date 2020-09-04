using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VietStar.Entities.ViewModels;

namespace VietStar.Entities.Infrastructures
{
    public class CurrentProcess
    {
        public CurrentProcess()
        {
            Errors = new List<ErrorMessage>();
            Items = new Dictionary<string, object>();
        }
        public Account User { get; set; }
        public List<ErrorMessage> Errors { get; }

        public void AddError(string errorMessage, params object[] traceKeys)
        {
            Errors.Add(new ErrorMessage
            {
                Message = errorMessage,
                TraceKeys = traceKeys != null ? traceKeys.ToList() : null
            });
        }

        public bool HasError { get { return Errors.Count > 0; } }

        public ErrorMessage ToError()
        {
            if (HasError)
                return Errors[0];

            return null;
        }
        public List<ErrorMessage> ToErrors()
        {
            if (HasError)
                return Errors;

            return null;
        }

        public Dictionary<string, object> Items { get; }

        public void AddItem(string key, object value)
        {
            Items.Add(key, value);
        }
        public T GetItem<T>(string key)
        {
            return Items.ContainsKey(key) ? (T)Items[key] : TypeExtensions.GetDefaultValue<T>();
        }
        public static Account CurrentUser { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }
        public List<object> TraceKeys { get; set; }
    }
    public static class TypeExtensions
    {
        public static T GetDefaultValue<T>()
        {
            return (T)GetDefaultValue(typeof(T));
        }
        public static object GetDefaultValue(this Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}

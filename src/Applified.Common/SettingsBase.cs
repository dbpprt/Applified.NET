using System;
using System.Collections.Generic;
using Applified.Common.Exceptions;

namespace Applified.Common
{
    public abstract class SettingsBase
    {
        private readonly Dictionary<string, string> _settings;
        private readonly Dictionary<string, Tuple<object, Type>> _mappings;

        public SettingsBase(Dictionary<string, string> settings)
        {
            _settings = settings;
            _mappings = new Dictionary<string, Tuple<object, Type>>();
        }

        public T GetValue<T>(string key)
        {
            var optional = GetRawValue<T>(key);

            if (optional.IsSet && optional.IsValid)
            {
                return optional.Value;
            }

            if (!optional.IsSet)
            {
                Tuple<object, Type> mapping;
                if (!_mappings.TryGetValue(key, out mapping))
                {
                    throw new InvalidSettingException(key);
                }

                if (typeof(T) != mapping.Item2)
                {
                    throw new InvalidCastException("The registered type doesnt match with the desired type!");
                }

                return (T)mapping.Item1;
            }

            throw optional.InnerException;
        }

        public Optional<T> GetRawValue<T>(string key)
        {
            string value;

            if (_settings.TryGetValue(key, out value))
            {
                try
                {
                    var parsedValue = ParseType<T>(value);

                    return new Optional<T>
                    {
                        InnerException = null,
                        IsSet = true,
                        IsValid = true,
                        Value = parsedValue
                    };
                }
                catch (Exception ex)
                {
                    return new Optional<T>
                    {
                        InnerException = ex,
                        IsSet = true,
                        IsValid = false,
                        Value = default(T)
                    };
                }
            }

            return new Optional<T>
            {
                InnerException = null,
                IsSet = false,
                IsValid = false,
                Value = default(T)
            };
        }

        public void Register<T>(string key, T defaultValue)
        {
            _mappings.Add(key, new Tuple<object, Type>(defaultValue, typeof(T)));
        }

        public virtual T ParseType<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

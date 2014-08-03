﻿using System;
using System.Collections.Generic;
using System.Linq;
using Applified.Common.Exceptions;

namespace Applified.Common
{
    public abstract class SettingsBase
    {
        private readonly Dictionary<string, string> _settings;
        private readonly Dictionary<string, Tuple<object, Type, string>> _mappings;

        public SettingsBase(Dictionary<string, string> settings)
        {
            _settings = settings;
            _mappings = new Dictionary<string, Tuple<object, Type, string>>();
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
                Tuple<object, Type, string> mapping;
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

        public List<AvaliableSetting> GetAvaliableSettings()
        {
            return _mappings.Select(mapping => new AvaliableSetting
            {
                Key = mapping.Key,
                DefaultValue = mapping.Value.Item1,
                ValueType = mapping.Value.Item2,
                Description = mapping.Value.Item3,
            }).ToList();
        } 

        public void Register<T>(string key, T defaultValue, string description)
        {
            _mappings.Add(key, new Tuple<object, Type, string>(defaultValue, typeof(T), description));
        }

        public virtual T ParseType<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    public class AvaliableSetting
    {
        public string Key { get; set; }

        public object DefaultValue { get; set; }

        public Type ValueType { get; set; }

        public string Description { get; set; }
    }
}

#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Applified.Common.Exceptions;

namespace Applified.Common.Configuration
{
    public abstract class SettingsBase
    {
        protected readonly Dictionary<string, string> Settings;
        protected readonly Dictionary<string, Tuple<object, Type, string>> Mappings;

        public SettingsBase(Dictionary<string, string> settings)
        {
            Settings = settings;
            Mappings = new Dictionary<string, Tuple<object, Type, string>>();
        }

        public SettingsBase()
            : this(null)
        {
            
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
                if (!Mappings.TryGetValue(key, out mapping))
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

            if (Settings != null && Settings.TryGetValue(key, out value))
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
            return Mappings.Select(mapping => new AvaliableSetting
            {
                Key = mapping.Key,
                DefaultValue = mapping.Value.Item1,
                ValueType = mapping.Value.Item2.Name,
                Description = mapping.Value.Item3,
            }).ToList();
        } 

        public void Register<T>(string key, T defaultValue, string description)
        {
            Mappings.Add(key, new Tuple<object, Type, string>(defaultValue, typeof(T), description));
        }

        public virtual T ParseType<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

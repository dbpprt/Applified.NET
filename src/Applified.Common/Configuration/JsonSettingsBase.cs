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
using System.IO;
using Newtonsoft.Json;

namespace Applified.Common.Configuration
{
    public class JsonSettingsBase : SettingsBase
    {
        private readonly string _filePath;

        public JsonSettingsBase(string filePath)
            : base(LoadFromFile(filePath))
        {
            _filePath = filePath;
        }

        private static Dictionary<string, string> LoadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(filePath);
            }

            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            var contents = File.ReadAllText(filePath);
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);

            return jsonObject;
        }

        private Dictionary<string, string> MergeSettings()
        {
            var result = new Dictionary<string, string>();

            foreach (var mapping in Mappings)
            {
                string value;

                if (!Settings.TryGetValue(mapping.Key, out value))
                {
                    value = mapping.Value.Item1.ToString();
                }

                result.Add(mapping.Key, value);
            }

            return result;
        }

        public void Save()
        {
            var settings = MergeSettings();
            var jsonString = JsonConvert.SerializeObject(settings);

            File.WriteAllText(_filePath, jsonString);
        }

    }
}

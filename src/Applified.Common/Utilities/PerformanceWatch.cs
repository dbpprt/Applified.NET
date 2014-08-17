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
using System.Diagnostics;

namespace Applified.Common.Utilities
{
    public class PerformanceWatch : IDisposable
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private Action<TimeSpan> _callback;

        public PerformanceWatch()
        {
            _stopwatch.Start();
        }

        public PerformanceWatch(Action<TimeSpan> callback)
            : this()
        {
            _callback = callback;
        }

        public static PerformanceWatch Start(Action<TimeSpan> callback)
        {
            return new PerformanceWatch(callback);
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            if (_callback != null)
                _callback(Result);
        }

        public TimeSpan Result
        {
            get { return _stopwatch.Elapsed; }
        }
    }
}
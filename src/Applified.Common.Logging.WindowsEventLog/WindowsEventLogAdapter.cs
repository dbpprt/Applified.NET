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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common.Utilities;

namespace Applified.Common.Logging.WindowsEventLog
{
    public class WindowsEventLogAdapter : ILogAdapter
    {
        private readonly IEventLogSettings _settings;

        public WindowsEventLogAdapter(
            IEventLogSettings settings
            )
        {
            _settings = settings;
        }

        public void Save(Event data)
        {
            if (!EventLog.SourceExists(_settings.Source))
                EventLog.CreateEventSource(_settings.Source, _settings.Log);

            var message = SerializeEvent(data);

            EventLog.WriteEntry(_settings.Source, message,
                ToEventLogEntryType(data.Level));
        }

        public Task SaveAsync(Event data)
        {
            Save(data);
            return Task.FromResult(0);
        }

        private static string SerializeEvent(Event data)
        {
            var result = "";
            var exception = data.Exception;

            result += "Message: " + data.Message + Environment.NewLine + Environment.NewLine;
            result += "CorrelationId: " + data.CorrelationId + Environment.NewLine + Environment.NewLine;

            if (exception != null)
            {
                result += "Exception message: " + exception
                    .Innermost()
                    .Message() + Environment.NewLine + Environment.NewLine;

                result += "Exception: " + Environment.NewLine +
                    exception.Innermost() + Environment.NewLine + Environment.NewLine;
            }

            foreach (var obj in data.Objects)
            {
                result += "Additional Object: " + Environment.NewLine;

                try
                {
                    result += ObjectDumper.Dump(obj);
                }
                catch (Exception ex)
                {
                    result += "Unable to dump object! (Exception: " + ex.Message + " )";
                }

                result += Environment.NewLine + Environment.NewLine;
            }

            return result;
        }

        private static EventLogEntryType ToEventLogEntryType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Critical:
                    return EventLogEntryType.Error;

                case LogLevel.Error:
                    return EventLogEntryType.Error;

                case LogLevel.Information:
                    return EventLogEntryType.Information;

                case LogLevel.Warning:
                    return EventLogEntryType.Warning;

                case LogLevel.Verbose:
                    return EventLogEntryType.Information;

                case LogLevel.Audit:
                    return EventLogEntryType.SuccessAudit;

                case LogLevel.FailureAudit:
                    return EventLogEntryType.FailureAudit;

            }

            return EventLogEntryType.Warning;
        }
    }
}

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
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Applified.Core.Middleware
{
    public class ConsoleLoggerMiddleware 
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public ConsoleLoggerMiddleware(
            Func<IDictionary<string, object>, Task> next)
        {

            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            //var requestContainer = (IServiceProvider) context.Environment.GetRequestContainer();
            //var unitOfWork = requestContainer.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            var path = context.Request.Path;

            var stopWatch = Stopwatch.StartNew();
            return _next(environment).ContinueWith(t =>
            {
                Console.WriteLine("{0} - {1} in {2}ms with response {3} {4}", 
                    context.Request.Method,
                    path, 
                    stopWatch.ElapsedMilliseconds,
                    context.Response.StatusCode,
                    string.IsNullOrEmpty(context.Response.ReasonPhrase) ? "" : " - " + context.Response.ReasonPhrase);
                return t;
            });

        }
    }
}

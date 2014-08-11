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
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;

namespace Applified.IntegratedFeatures.PageOfDeath.Wrappers
{
    class ErrorPageMiddlewareWrapper : OwinMiddleware
    {
         private readonly ErrorPageMiddleware _underlayingMiddleware;

         public ErrorPageMiddlewareWrapper(
            OwinMiddleware next, 
            ErrorPageOptions options
            ) : base(next)
        {
            _underlayingMiddleware = new ErrorPageMiddleware(
                objects => next.Invoke(new OwinContext(objects)),
                options,
                null,
                false);
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

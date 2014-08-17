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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace Applified.Common
{
    public class MockAppBuilder : IAppBuilder
    {
        public Dictionary<int,Tuple<object, object[]>> Middlewares { get; private set; }
        private int curr = 0;

        public MockAppBuilder()
        {
            Middlewares = new Dictionary<int, Tuple<object, object[]>>();
            Properties = new Dictionary<string, object>();
        }

        public IAppBuilder Use(object middleware, params object[] args)
        {
            Middlewares.Add(curr, new Tuple<object, object[]>(middleware, args));
            curr++;
            return this;
        }

        //public OwinMiddleware BuildPipe(OwinMiddleware next)
        //{
        //    for (var i = Middlewares.Count - 1; i >= 0; i--)
        //    {
        //        var current = 
        //    }
        //}

        public object Build(Type returnType)
        {
            throw new NotImplementedException();
        }

        public IAppBuilder New()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> Properties { get; private set; }
    }
}

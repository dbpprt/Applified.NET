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
using Xunit;

namespace Applified.Core.Tests
{
    /// <summary>
    /// This class is really stupid but this are my first experiments with xUnit in visual studio 
    /// </summary>
    public class XUnitTest
    {
        [Fact]
        public void Ensure_xUnitIsWorking()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Ensure_ThisTestShouldFail()
        {
            Assert.Equal(1, 0);
        }
    }
}

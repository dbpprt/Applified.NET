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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Migrations;
using Applified.Core.ServiceContracts;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class MigrateDatabaseCommand : CommandBase
    {
        public MigrateDatabaseCommand(
            Options options
            )
            : base(options)
        {
        }

        public override async Task<int> Execute()
        {
            var migrator = new DbMigrator(new EntityContextConfiguration());

            var pendingMigrations = migrator
                .GetPendingMigrations()
                .ToList();

            foreach (var migration in pendingMigrations)
            {
                Console.WriteLine("Pending migration: " + migration);
            }

            if (!pendingMigrations.Any())
            {
                Console.WriteLine("No pending migrations");
            } 

            migrator.Update();

            return 0;
        }
    }
}

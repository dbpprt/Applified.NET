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
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Identity;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Owin;

namespace Applified.Core.Identity
{
    public class TenantDemandAuthenticationMiddleware : OwinMiddleware
    {
        private readonly IAppBuilder _appBuilder;
        private static ConcurrentDictionary<Guid, OwinMiddleware> _tenantAuthenticationPipe;
        private static readonly object TenantAuthenticationPipeLock = new object();

        public TenantDemandAuthenticationMiddleware(
            OwinMiddleware next, 
            IAppBuilder appBuilder)
            : base(next)
        {
            _appBuilder = appBuilder;
            lock (TenantAuthenticationPipeLock)
            {
                _tenantAuthenticationPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
            }
        }

        public override async Task Invoke(IOwinContext context)
        {
            var scope = context.GetRequestContainer();
            var current = scope.Resolve<ICurrentContext>();

            OwinMiddleware pipe;
            if (_tenantAuthenticationPipe.TryGetValue(current.ApplicationId, out pipe))
            {
                await pipe.Invoke(context);
                return;
            }

            var registeredProviders = await scope.Resolve<IRepository<ExternalOAuthProvider>>().Query()
                .ToListAsync();

            var count = registeredProviders.Count;
            var lastMiddleware = Next;

            for (var i = count - 1; i >= 0; i--)
            {
                var provider = registeredProviders[i];

                var currentMiddleware =  provider.ToMiddleware(lastMiddleware, _appBuilder); ;

                if (currentMiddleware != null || lastMiddleware == null)
                {
                    lastMiddleware = currentMiddleware;
                }
            }

            if (lastMiddleware != null)
            {
                SetPipe(current.ApplicationId, lastMiddleware);
                await lastMiddleware.Invoke(context);
            }
            else
            {
                SetPipe(current.ApplicationId, Next);
                await Next.Invoke(context);
            }
        }

        private void SetPipe(Guid application, OwinMiddleware middleware)
        {
            lock (TenantAuthenticationPipeLock)
            {
                if (_tenantAuthenticationPipe == null)
                {
                    _tenantAuthenticationPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
                }

                _tenantAuthenticationPipe.TryAdd(application, middleware);
            }
        }
    }
}

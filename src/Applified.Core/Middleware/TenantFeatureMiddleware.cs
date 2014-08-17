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
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Common.Logging;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Owin;

namespace Applified.Core.Middleware
{
    public class TenantFeatureMiddleware : OwinMiddlewareBase
    {
        private readonly IAppBuilder _appBuilder;
        private static ConcurrentDictionary<Guid, OwinMiddleware> _tenantOwinPipe;
        private static SemaphoreSlim _tenantPipeLock;

        public TenantFeatureMiddleware(
            OwinMiddleware next,
            IAppBuilder appBuilder)
            : base(next, appBuilder)
        {
            _appBuilder = appBuilder;
            _tenantOwinPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
            _tenantPipeLock = new SemaphoreSlim(1);
        }

        public override async Task Invoke(IOwinContext context)
        {
            var scope = context.GetRequestContainer();
            var current = scope.Resolve<ICurrentContext>();

            OwinMiddleware pipe;
            if (_tenantOwinPipe.TryGetValue(current.ApplicationId, out pipe))
            {
                await pipe.Invoke(context);
                return;
            }

            var featureService = scope.Resolve<IFeatureService>();
            var registeredFeatures = await featureService.GetActivatedFeaturesAsync();

            registeredFeatures = registeredFeatures
                .OrderByDescending(feature => feature.ExecutionOrderKey)
                .ToList();

            var count = registeredFeatures.Count;
            var lastMiddleware = Next;

            for (var i = count - 1; i >= 0; i--)
            {
                var feature = registeredFeatures[i];

                // TODO: redudant database queries.. GetFeatureAsync() get called twice
                var instance = await featureService.InstantiateFeatureAsync(feature.Id);

                if (instance == null)
                {
                    continue;
                }

                OwinMiddleware currentMiddleware = null;

                try
                {
                    currentMiddleware = await instance.UseAsync(
                        current.ApplicationId,
                        lastMiddleware,
                        _appBuilder,
                        scope
                        );
                }
                catch (Exception exception)
                {
                    exception.ToEvent()
                        .IsError()
                        .WithObject(feature)
                        .SetMessage("Unable to add feature to tenants feature pipeline.")
                        .Save(Log);
                }

                if (currentMiddleware != null || lastMiddleware == null)
                {
                    lastMiddleware = currentMiddleware;
                }
            }

            if (lastMiddleware != null)
            {
                await SetPipe(current.ApplicationId, lastMiddleware);
                await lastMiddleware.Invoke(context);
            }
            else
            {
                await SetPipe(current.ApplicationId, Next);
                await Next.Invoke(context);
            }
        }

        private static async Task SetPipe(Guid application, OwinMiddleware middleware)
        {
            await _tenantPipeLock.WaitAsync();

            try
            {
                if (_tenantOwinPipe == null)
                {
                    _tenantOwinPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
                }

                _tenantOwinPipe.TryAdd(application, middleware);
            }
            finally
            {
                _tenantPipeLock.Release();
            }
        }
    }
}

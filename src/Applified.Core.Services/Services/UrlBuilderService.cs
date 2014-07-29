using System;
using Applified.Core.ServiceContracts;

namespace Applified.Core.Services.Services
{
    public class UrlBuilderService : IUrlBuilderService
    {
        public string GetStoredObjectUrl(Guid storedObjectId)
        {
            return "/api/media/" + storedObjectId;
        }
    }
}

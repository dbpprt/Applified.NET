using System;

namespace Applified.Core.ServiceContracts
{
    public interface IUrlBuilderService
    {
        string GetStoredObjectUrl(Guid storedObjectId);
    }
}

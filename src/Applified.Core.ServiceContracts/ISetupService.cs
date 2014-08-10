using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applified.Core.ServiceContracts
{
    public interface ISetupService
    {
        Task InitializeIntegratedFeatures(string directory);


    }
}

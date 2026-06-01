using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WOT_CS.Core.Configuration
{
    public interface IAppSettings
    {
        string ConnectionString { get; }
    }
}

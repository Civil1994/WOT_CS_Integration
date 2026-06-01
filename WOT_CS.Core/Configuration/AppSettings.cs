using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WOT_CS.Core.Configuration
{
    public class AppSettings : IAppSettings
    {
        public string ConnectionString { get; set; }
    }
}

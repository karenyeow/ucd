using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCD.Repository;

namespace UCD.API.Helpers
{
    public class UCDGlobal
    {
        public static IConfiguration Config { get; set; }

       
        public static IUCDRepository UCDRepository { get { return new UCDRepositoryProvider(new UCDConnectionProvider(Config));} }

        public static IConfigurationSection Drools { get { return Config.GetSection("DroolsConfiguration"); } }
    }
}

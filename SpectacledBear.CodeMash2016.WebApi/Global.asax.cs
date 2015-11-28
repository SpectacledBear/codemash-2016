using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using SpectacledBear.CodeMash2016.WebApi.Data;

namespace SpectacledBear.CodeMash2016.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            PersistentSqliteDatabase.Initialize();
        }

        protected void Application_End()
        {
            PersistentSqliteDatabase.Terminate();
        }
    }
}

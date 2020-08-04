using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace IncIncASP
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            //The first Session "Logged" which is an indicator to the
            //status of the user
            Session["Logged"] = "No";
            //The second Session "User" stores the name of the current user
            Session["User"] = "";

            //The third Session "URL" stores the URL of the
            //requested WebForm before Logging In
            Session["URL"] = "Default.aspx";
        }
    }
}
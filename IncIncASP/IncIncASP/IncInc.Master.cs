using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncIncASP
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Signs out the user when they click the associated 'logout'
        /// button on the LoginStatus object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
        }
    }
}
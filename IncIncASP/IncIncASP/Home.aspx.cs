using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncIncASP
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the payroll button on our landing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPayroll_Click(object sender, EventArgs e)
        {
            Response.Redirect("/IncIncPayroll.aspx");
        }

        /// <summary>
        /// Handler for the summary button on our landing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSummary_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Summary.aspx");
        }

        /// <summary>
        /// Handler for the table button on our landing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Table.aspx");
        }
        
    }
}
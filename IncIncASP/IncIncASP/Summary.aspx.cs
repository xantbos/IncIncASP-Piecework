// Summary.aspx.cs
// Last Modified: 2018-11-20
// Modified By:   Adrian Kriz
// 
// This webform is designed to only display the database values for
// our application. Nothing special.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncIncASP
{
    public partial class Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // populate our display labels with database information every time the page is loaded
            lblWorkerCountOutput.Text = PieceworkWorker.TotalEmployees.ToString();
            lblTotalMessagesOutput.Text = PieceworkWorker.TotalMessages.ToString();
            lblOverallPayOutput.Text = PieceworkWorker.TotalPay.ToString("c");
            lblOverallAverageOutput.Text = PieceworkWorker.GetAveragePay().ToString("c");
        }
    }
}
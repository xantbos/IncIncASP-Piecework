// IncIncPayroll.aspx.cs
// Last Modified: 2018-11-20
// Modified By:   Adrian Kriz
// 
// This is the backend class of the payroll asp application.
// It handles all interactions done between the user and the webform.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncIncASP
{
    public partial class IncIncPayroll : System.Web.UI.Page
    {
        #region "Form Loading"

        /// <summary>
        /// Default form load constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnCalculate.UniqueID;
            if (!Page.IsPostBack) { lblWorkerPayOutput.Text = "$0.00"; }
            //ClearEntryForm();
            DisplayPayOutputs();
        }

        #endregion

        #region "Form Controls"

        /// <summary>
        /// Event handler for the Calculate button
        /// Will attempt to process a worker based on the
        /// selected radiobutton and then update the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_OnClick(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid) // ensure our validation checks went through
            {
                Worker myWorker; // create an uninstantiated worker

                if (rdWorkerType.SelectedIndex == 0) // if our Piecework Worker radio button is selected
                {
                    // do normal worker
                    // myWorker = new PieceworkWorker(txtWorkerFirstNameEntry.Text, txtWorkerLastNameEntry.Text, txtMessagesEntry.Text);
                    myWorker = new PieceworkWorker(txtWorkerNameEntry.Text, txtMessagesEntry.Text);
                }
                else if (rdWorkerType.SelectedIndex == 1) // if our Senior worker radio button is selected
                {
                    // do senior worker
                    // myWorker = new SeniorWorker(txtWorkerFirstNameEntry.Text, txtWorkerLastNameEntry.Text, txtMessagesEntry.Text);
                    myWorker = new SeniorWorker(txtWorkerNameEntry.Text, txtMessagesEntry.Text);
                }
                else if (rdWorkerType.SelectedIndex == 2) // if our hourly worker radio button is selected
                {
                    // do hourly worker
                    myWorker = new HourlyWorker(txtWorkerNameEntry.Text, txtMessagesEntry.Text, txtHoursWorkedEntry.Text, txtHourlyPayEntry.Text);
                }
                else // in case something goes catastrophically wrong and our user has any other value
                {
                    ArgumentNullException ex = new ArgumentNullException("worker", "Worker type was not selected.");
                    throw ex;
                }

                // assume the data is valid and nothing went wrong
                // we've gone through client and then server side validation,
                // followed by class validation

                // increment our total data
                myWorker.UpdateTotals();

                // add our new worker to the database
                myWorker.AddWorkerToDB();

                // update our labels
                lblWorkerPayOutput.Text = myWorker.Pay.ToString("c");
                DisplayPayOutputs();

                // lock up our form and wait for the clear button to get pressed
                ToggleEnabledControls();

                // focus the clear button to make the user's life easier
                btnClear.Focus();

                lblOutNote.Text = myWorker.ToString() + "<br />This Worker has been successfully recorded.";

            }
        }

        /// <summary>
        /// Event handler for our clear button being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            ClearEntryForm();
            txtWorkerNameEntry.Focus();
        }

        /// <summary>
        /// Event handler for our reset totals button being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetTotals_OnClick(object sender, EventArgs e)
        {
            PieceworkWorker.ResetTotals();
            DisplayPayOutputs();
        }

        /// <summary>
        /// Handler for our radio button selection changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdWorkerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we only want to enable vision of our grouped panel if the hourly radio button is selected
            pnlHourly.Visible = (rdWorkerType.SelectedIndex != 2) ? false : true;            
        }

        #endregion

        #region "Form Updating"

        /// <summary>
        /// Method to reset our form controls to empty
        /// Also re-enables our inputs if currently disabled
        /// </summary>
        private void ClearEntryForm()
        {
            //txtWorkerFirstNameEntry.Text = "";
            //txtWorkerLastNameEntry.Text = "";
            txtWorkerNameEntry.Text = "";
            txtMessagesEntry.Text = "";
            lblOutNote.Text = "";
            lblWorkerPayOutput.Text = "$0.00";
            if(!btnCalculate.Enabled){ ToggleEnabledControls(); }
        }

        /// <summary>
        /// Method to toggle enabled status of any controls we don't want
        /// the user pressing after submitting a worker and before
        /// clearing the content
        /// </summary>
        private void ToggleEnabledControls()
        {
            //txtWorkerFirstNameEntry.Enabled = !txtWorkerFirstNameEntry.Enabled;
            //txtWorkerLastNameEntry.Enabled = !txtWorkerLastNameEntry.Enabled;
            txtWorkerNameEntry.Enabled = !txtWorkerNameEntry.Enabled;
            txtMessagesEntry.Enabled = !txtMessagesEntry.Enabled;
            btnCalculate.Enabled = !btnCalculate.Enabled;
            rdWorkerType.Enabled = !rdWorkerType.Enabled;
        }

        /// <summary>
        /// Method to access our Worker's exposed properties to update
        /// our displayed label totals
        /// </summary>
        private void DisplayPayOutputs()
        {
            lblWorkerCountOutput.Text = PieceworkWorker.TotalEmployees.ToString();
            lblTotalMessagesOutput.Text = PieceworkWorker.TotalMessages.ToString();
            lblOverallPayOutput.Text = PieceworkWorker.TotalPay.ToString("c");
            lblOverallAverageOutput.Text = PieceworkWorker.GetAveragePay().ToString("c");
        }
        #endregion

        #region "Custom Validation"

        /// <summary>
        /// Server side authentication for the name input
        /// Will check to ensure there is a space for 'two' entries
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ServerNameValidate(object source, ServerValidateEventArgs args)
        {
            string[] parts = args.Value.Split(' ');
            args.IsValid = (parts.Length > 1) ? true : false;
        }

        /// <summary>
        /// Server side authentication to make sure the name parts
        /// are at least a certain predefined length
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ServerNameLengthValidate(object source, ServerValidateEventArgs args)
        {
            string[] parts = args.Value.Split(' ');
            args.IsValid = true;
            foreach (string s in parts)
            {
                if (s.Length <= PieceworkWorker.minimumAlphaInName) { args.IsValid = false; break; }
            }
        }

        #endregion

    }
}
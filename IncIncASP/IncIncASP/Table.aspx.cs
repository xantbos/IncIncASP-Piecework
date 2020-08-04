// Table.aspx.cs
// Last Modified: 2018-11-30
// Modified By:   Adrian Kriz
// 
// This webform displays all our database workers
// in an easy-to-read table for the user.
// Now supports updating and deleting of workers stored
// in the db.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace IncIncASP
{
    public partial class Table : System.Web.UI.Page
    {
        List<Worker> tableWorkers; // global holder for referencing
        Worker loadedWorker; // global holder for current worker we're editing

        /// <summary>
        /// Event handler for our page being loaded.
        /// Will generate the table from our database every time.
        /// Ensures data being seen is the most up-to-date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // load up our table data from our db
            //Worker[] tableWorkers = DBL.GetEmployeeList();
            tableWorkers = DBL.GetEmployeeList();

            // quick check if we're posting back and if we stored a 'selection' from the user clicking
            // on one of our dynamic radiobuttons
            if(IsPostBack && ! (HttpContext.Current.Session["selectedIndex"] == null))
            {
                if(!((int)HttpContext.Current.Session["selectedIndex"] < 0)) // napkin error checking
                {
                    loadedWorker = tableWorkers[(int)HttpContext.Current.Session["selectedIndex"]];
                }
            }

            int currentWorker = 0; // used to increment button ID

            // do a foreach through each object and add required data to our table
            // we create new item instances every loop so we don't overwrite existing pointers
            foreach(Worker thisWorker in tableWorkers)
            {

                // create an empty tablerow
                TableRow thisRow = new TableRow();

                // create two arrays to store our data for looping
                string[] workerData = thisWorker.GetTableData();
                TableCell[] newCells = new TableCell[workerData.Length+1];

                // we create our radio button in the first slot
                // so we format the tablecell first
                newCells[0] = new TableCell();

                // create our radiobutton control
                RadioButton rdButton = new RadioButton();
                rdButton.GroupName = "WorkerTable"; // assign a group so only one can be selected
                rdButton.ID = "Worker_" + currentWorker; // use our incrementing value to assign a unique ID
                rdButton.CheckedChanged += CellRadioButtonClicked; // add an event handler
                rdButton.AutoPostBack = true; // set the buttons to cause a postback

                newCells[0].Controls.Add(rdButton); // add our button

                // set our data via loop
                for(int count=1;count<newCells.Length;count++)
                {
                    newCells[count] = new TableCell(); // init tablecell
                    newCells[count].Text = workerData[count-1];
                }

                // add all cells to our row
                foreach(TableCell currentCell in newCells)
                {
                    thisRow.Cells.Add(currentCell);
                }

                // add our row to our table
                tblWorker.Rows.Add(thisRow);

                currentWorker++; // increment
            }
        }

        /// <summary>
        /// Event hander for our radiobuttons being pressed.
        /// Parses the 'ID' of the radiobutton and then loads the
        /// matching worker from our global list.
        /// Places the parsed ID into a session value to be loaded
        /// after postbacks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CellRadioButtonClicked(object sender, EventArgs e)
        {
            pnlTableManipulation.Visible = true;
            lblOutMessage.Text = "";
            if (!btnUpdate.Enabled) { ToggleEnterableContent(); }
            RadioButton thisButton = (RadioButton)sender;
            int workerID = Int32.Parse(thisButton.ID.Remove(0, thisButton.ID.IndexOf('_')+1));
            loadedWorker = tableWorkers[workerID];
            txtWorkerNameEntry.Text = loadedWorker.FullName;
            txtMessagesEntry.Text = loadedWorker.Messages;
            if(loadedWorker.Type == (int)IncIncEnumerables.WorkerTypes.Hourly)
            {
                pnlHourly.Visible = true;
                txtHourlyPayEntry.Text = ((HourlyWorker)loadedWorker).HourlyWage;
                txtHoursWorkedEntry.Text = ((HourlyWorker)loadedWorker).HoursWorked;
            }
            HttpContext.Current.Session["selectedIndex"] = workerID;
            
        }

        /// <summary>
        /// Controls visibilty and enabled controls
        /// </summary>
        protected void ToggleEnterableContent()
        {
            loadedWorker = null;
            txtWorkerNameEntry.Enabled = !txtWorkerNameEntry.Enabled;
            txtWorkerNameEntry.Text = "";
            txtMessagesEntry.Enabled = !txtMessagesEntry.Enabled;
            txtMessagesEntry.Text = "";
            txtHourlyPayEntry.Enabled = !txtHourlyPayEntry.Enabled;
            txtHourlyPayEntry.Text = "";
            txtHoursWorkedEntry.Enabled = !txtHoursWorkedEntry.Enabled;
            txtHoursWorkedEntry.Text = "";
            btnUpdate.Enabled = !btnUpdate.Enabled;
            btnDelete.Enabled = !btnDelete.Enabled;
            pnlHourly.Visible = false;
        }

        /// <summary>
        /// Event handler for our Update button being pressed.
        /// Loads editable data into the associated object, then
        /// attempts to update the reference in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateWorker(object sender, EventArgs e)
        {
            Page.Validate(); // validate user input first
            if (Page.IsValid) // if we're valid
            {
                // update worker item
                loadedWorker.FullName = txtWorkerNameEntry.Text;
                loadedWorker.Messages = txtMessagesEntry.Text;
                if (pnlHourly.Visible)
                {
                    ((HourlyWorker)loadedWorker).HourlyWage = txtHourlyPayEntry.Text;
                    ((HourlyWorker)loadedWorker).HoursWorked = txtHoursWorkedEntry.Text;
                }
                loadedWorker.UpdatePay();
                // update our database reference
                loadedWorker.UpdateThisWorkerDB();
                ToggleEnterableContent(); // disable and hide controls
                HttpContext.Current.Session["selectedIndex"] = null; // remove our loaded worker reference
                //HttpContext.Current.Session["contextMessage"] = "Worker was updated!";
                //lblOutMessage.Text = "Worker was updated!";
                //loadedWorker = null;
                pnlTableManipulation.Visible = false;
                Page.Response.Redirect(Page.Request.Url.ToString(), false);
            }
        }

        protected void DeleteWorker(object sender, EventArgs e)
        {
            // delete our database reference
            loadedWorker.DeleteThisWorkerDB();
            ToggleEnterableContent(); // disable and hide controls
            HttpContext.Current.Session["selectedIndex"] = null; // remove our loaded worker reference
            //HttpContext.Current.Session["contextMessage"] = "Worker was deleted!";
            //lblOutMessage.Text = "Worker was deleted!";
            //loadedWorker = null;
            pnlTableManipulation.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), false);
        }

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
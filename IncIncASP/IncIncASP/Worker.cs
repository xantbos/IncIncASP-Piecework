// Worker.cs
// Last Modified: 2018-12-5
// Modified By:   Adrian Kriz
// 
// Parentmost Worker class. All base major worker types
// should be based on this.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncIncASP
{
    public abstract class Worker
    {
        #region "Variable Declarations"

        #region "Constants"

        const int messagesSentMinimum = 0; // minimum number of messages allowed
        const int messagesSentMaximum = 20000; // maximum amount of messages allowed

        #endregion

        #region "Instance Variables"
        protected int employeeId; // employee's ID
        protected string employeeFirstName; // employee's first name
        protected string employeeLastName; // employee's last name
        protected int employeeMessages = 0; // messages sent by the employee
        protected decimal employeePay = 0M; // total pay for the employee
        protected int employeeType; // denotes the type of worker, extended classes can use it
        protected DateTime creationDate;
        protected readonly int[] totalMessagesSentCap; // must redeclare in children classes
        protected readonly double[] totalPayoutValues; // must redeclare in children classes
        #endregion

        #region "Static Variables"
        protected static int overallNumberOfEmployees = 0; // total processed employee count
        protected static int overallMessages = 0; // total messages sent by all workers
        protected static decimal overallPayroll = 0M; // total payout for all workers
        protected internal static int minimumAlphaInName = 2; // the minimum allowed characters in a part of a name position
        #endregion

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        protected internal Worker() { }

        #endregion

        #region "Class Functions"

        /// <summary>
        /// The findPay() method is marked abstract
        /// as it must be declared for each worker
        /// type individually.
        /// </summary>
        protected internal abstract decimal findPay(int[] messageCapArray, double[] messagePayoutArray);

        /// <summary>
        /// Overrides the ToString functionality to give a
        /// more descriptive look at our object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string formattedOutput = "{0} - {1} messages - {2}";
            return String.Format(formattedOutput, FullName, Messages, Pay.ToString("c"));
        }

        /// <summary>
        /// Adds the current Worker data to our totals
        /// </summary>
        protected internal void UpdateTotals()
        {
            overallNumberOfEmployees += 1;
            overallMessages += employeeMessages;
            overallPayroll += employeePay;
        }

        /// <summary>
        /// Updates the worker's pay
        /// Referenced for db updating
        /// </summary>
        protected internal abstract void UpdatePay();
        //{
        //    employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
        //}

        /// <summary>
        /// Function to get the average pay for all processed workers
        /// </summary>
        /// <returns></returns>
        protected internal static decimal GetAveragePay()
        {
            return (TotalEmployees == 0) ? 0 : TotalPay / TotalEmployees;
        }

        /// <summary>
        /// Method to reset our tracked values
        /// </summary>
        protected internal static void ResetTotals()
        {
            overallMessages = 0;
            overallNumberOfEmployees = 0;
            overallPayroll = 0M;
        }

        /// <summary>
        /// Returns a string array formatted to be placed into a table
        /// More or less info can be added at will
        /// </summary>
        /// <returns>Array of string</returns>
        protected internal string[] GetTableData()
        {
            return new string[] { FullName, Messages.ToString(), Pay.ToString("c"), EntryDate.ToShortDateString(), TypeString };
        }

        #endregion

        #region "Database Functions"

        /// <summary>
        /// Adds this worker to the database after timestamping it
        /// </summary>
        protected internal void AddWorkerToDB()
        {
            creationDate = DateTime.Now;
            DBL.InsertNewRecord(this);
        }

        /// <summary>
        /// Updates this worker's database entry
        /// </summary>
        protected internal void UpdateThisWorkerDB()
        {
            DBL.UpdateExistingRow(this);
        }

        /// <summary>
        /// Deletes this worker's database entry
        /// </summary>
        protected internal void DeleteThisWorkerDB()
        {
            DBL.DeleteRow(this.Id);
        }

        /// <summary>
        /// Pulls our employee list from the database as a list of objects
        /// </summary>
        /// <returns>List of PieceworkWorker</returns>
        protected internal List<Worker> ReturnAllWorkers()
        {
            return DBL.GetEmployeeList();
        }

        /// <summary>
        /// Returns a total of messages sent by all workers
        /// </summary>
        /// <returns>Integer</returns>
        protected internal int ReturnMessagesCount()
        {
            return DBL.GetMessagesTotal();
        }

        /// <summary>
        /// Returns the total employee count in the database
        /// </summary>
        /// <returns>Integer</returns>
        protected internal int ReturnEmployeeCount()
        {
            return DBL.GetEmployeeTotal();
        }

        /// <summary>
        /// Returns the total pay given to all workers in the database
        /// </summary>
        /// <returns>Decimal</returns>
        protected internal decimal ReturnPayCount()
        {
            return DBL.GetPayTotal();
        }

        #endregion

        #region "Class Properties"

        /// <summary>
        /// Property to get and set the worker's Id number
        /// </summary>
        protected internal int Id
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        /// <summary>
        /// Property to get and set the worker's full name
        /// References the FirstName and LastName properties
        /// </summary>
        protected internal string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {
                //split input
                string[] parts = value.Split(' ');
                this.FirstName = parts[0];
                this.LastName = parts[1];
            }
        }

        /// <summary>
        /// Property to get and set the worker's first name
        /// </summary>
        protected internal string FirstName
        {
            get
            {
                return employeeFirstName;
            }
            set
            {
                if (!(value == String.Empty))
                {
                    employeeFirstName = value;
                }
                else
                {
                    throw new Exception("You must enter a first name.");
                }
            }
        }

        /// <summary>
        /// Property to get and set the wokrer's last name
        /// </summary>
        protected internal string LastName
        {
            get
            {
                return employeeLastName;
            }
            set
            {
                if (!(value == String.Empty))
                {
                    employeeLastName = value;
                }
                else
                {
                    throw new Exception("You must enter a last name.");
                }
            }
        }

        /// <summary>
        /// Property to get and set the worker's messages sent
        /// Will attempt to parse the input into an integer
        /// </summary>
        protected internal string Messages
        {
            get
            {
                return employeeMessages.ToString();
            }
            set
            {
                int parseDump;
                // do a tryparse and dump into a holder variable if it succeeds
                // if it fails throw an exception
                if (!(int.TryParse(value, out parseDump))) { throw new Exception("You must enter a number for Messages."); }
                // do a range test
                if (parseDump > messagesSentMinimum && parseDump <= messagesSentMaximum) { employeeMessages = parseDump; }
                else { throw new Exception(String.Format("Messages must be between {} and {}", messagesSentMinimum, messagesSentMaximum)); }
            }
        }

        /// <summary>
        /// Property to return this worker's pay
        /// Readonly. Use findPay to recalculate.
        /// </summary>
        protected internal decimal Pay
        {
            get { return employeePay; }
        }

        /// <summary>
        /// Property to return this worker's creation
        /// date
        /// </summary>
        protected internal DateTime EntryDate
        {
            get { return creationDate; }
        }

        /// <summary>
        /// Property to return an int value representing this worker type
        /// </summary>
        protected internal int Type
        {
            get { return employeeType; }
        }

        protected internal string TypeString
        {
            get { return ((IncIncEnumerables.WorkerTypes)employeeType).ToString(); }
        }

        #endregion

        #region "Static Properties"

        /// <summary>
        /// Property to get the total pay for all processed workers
        /// Reads from the database every call for consistency
        /// </summary>
        protected internal static decimal TotalPay
        {
            get
            {
                return DBL.GetPayTotal();
                // return overallPayroll;
            }
        }

        /// <summary>
        /// Property to get the total messages sent by all processed workers
        /// Reads from the database every call for consistency
        /// </summary>
        protected internal static int TotalMessages
        {
            get
            {
                return DBL.GetMessagesTotal();
                // return overallMessages;
            }
        }

        /// <summary>
        /// Property to get the total processed worker count
        /// Reads from the database every call for consistency
        /// </summary>
        protected internal static int TotalEmployees
        {
            get
            {
                return DBL.GetEmployeeTotal();
                // return overallNumberOfEmployees; 
            }
        }

        #endregion
    }
}
// PieceworkWorker.cs
// Last Modified: 2018-12-5
// Modified By:   Adrian Kriz
// 
// This is a class representing pieceworkworker objects. It is a child
// of the Worker class, inheriting all properties and values.
// It overrides the default ToString to better represent the class.
// Part of the IncIncPayroll Worker system.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncIncASP
{
    public class PieceworkWorker : Worker
    {

        #region "Variables"
        protected readonly new int[] totalMessagesSentCap = { 2499, 4999, 7499, 9999, 10000 }; // message cap array for this class
        protected readonly new double[] totalPayoutValues = { 0.022, 0.024, 0.027, 0.031, 0.035 }; // message payout array for this class
        #endregion

        #region "Constructors"

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected internal PieceworkWorker() { }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="name">Worker's full name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal PieceworkWorker(string name, string messages) : base()
        {
            FullName = name;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Regular;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
            employeeId = ReturnEmployeeCount() + 1;
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="firstName">Worker's first name</param>
        /// <param name="lastName">Worker's last name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal PieceworkWorker(string firstName, string lastName, string messages) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Regular;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
            employeeId = ReturnEmployeeCount() + 1;
        }

        /// <summary>
        /// Parameterized Constructor
        /// For database content
        /// </summary>
        /// <param name="id">Worker's ID</param>
        /// <param name="firstName">Worker's first name</param>
        /// <param name="lastName">Worker's last name</param>
        /// <param name="messages">Messages sent by worker</param>
        /// <param name="createdAt">The time this worker was created</param>
        protected internal PieceworkWorker(int id, string firstName, string lastName, string messages, DateTime createdAt) : base()
        {
            Id = id;
            creationDate = createdAt;
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Regular;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
        }

        #endregion

        #region "Class Functions"

        /// <summary>
        /// Currently called in the constructor, the findPay() method is
        /// used to calculate a worker's pay using threshold values to
        /// change how much a worker is paid per message.
        /// </summary>
        /// <returns>Decimal pay amount</returns>
        protected internal override decimal findPay(int[] messageCap, double[] payRates)
        {
            decimal payHolder = 0.0M; // value to hold and eventually return our calculated pay amount
            // create arrays with our values and matching payouts
            //int[] totalMessagesSentCap = { 2499, 4999, 7499, 9999, 10000 };
            //double[] totalPayoutValues = { 0.022, 0.024, 0.027, 0.031, 0.035 };

            // Count through all our message caps
            // When we find a match for <= we do our math, but if we go past our 10k cap we do the math anyways
            // When we get a hit we break the loop so we don't do double ups.
            for (int count = 0; count < messageCap.Length; count++)
            {
                if ((employeeMessages <= messageCap[count]) ||
                    ((employeeMessages > messageCap[messageCap.Length - 1]) && (count == messageCap.Length - 1)))
                {
                    payHolder = Convert.ToDecimal(employeeMessages * payRates[count]);
                    //break;
                }
            }
            return payHolder;
        }

        /// <summary>
        /// Custom update pay, uses class array values
        /// </summary>
        protected internal override void UpdatePay()
        {
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
        }

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

        #endregion 
    }
}

/* 
 * Why is exception handling necessary (or not) for the operation of this program?
 * 
 * Relying on the validation rules we set up on the page level, it prevents the user from giving incorrect data.
 * I still included error checking as per the lab's demands, but I feel they weren't necessary with client/server validation.
*/
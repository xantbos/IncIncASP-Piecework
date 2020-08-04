// SeniorWorker.cs
// Last Modified: 2018-12-5
// Modified By:   Adrian Kriz
// 
// This is a class representing seniorworker objects. It is a child class
// of the 'PieceworkWorker' class, inheriting all the properties and values.
// It overrides the default ToString to better represent the class,
// and it also has a unique findPay method overriding the base class.
// Part of the IncIncPayroll Worker system.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncIncASP
{
    public class SeniorWorker : PieceworkWorker
    {
        #region "Variable Declarations"

        #region "Constants"

        const decimal basePay = 200.00M;
        protected readonly new double[] totalPayoutValues = { 0.011, 0.014, 0.017, 0.021, 0.025 }; // override parent member with new

        #endregion

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected internal SeniorWorker() { }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="name">Worker's full name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal SeniorWorker(string name, string messages) : base()
        {
            FullName = name;
            Messages = messages;
            employeeType = (int) IncIncEnumerables.WorkerTypes.Senior;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="firstName">Worker's first name</param>
        /// <param name="lastName">Worker's last name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal SeniorWorker(string firstName, string lastName, string messages) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Senior;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
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
        protected internal SeniorWorker(int id, string firstName, string lastName, string messages, DateTime createdAt) : base()
        {
            Id = id;
            creationDate = createdAt;
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Senior;
            employeePay = findPay(totalMessagesSentCap, totalPayoutValues);
        }

        #endregion

        #region "Functions"

        /// <summary>
        /// Currently called in the constructor, the findPay() method is
        /// used to calculate a worker's pay using threshold values to
        /// change how much a worker is paid per message.
        /// </summary>
        /// <returns>Decimal pay amount</returns>
        protected internal override decimal findPay(int[] messageCap, double[] payRates)
        {
            return base.findPay(messageCap, payRates) + basePay; // add our senior worker's base pay to the calculated amount
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
            return base.ToString() + " - Senior Worker";
        }

        #endregion
    }
}
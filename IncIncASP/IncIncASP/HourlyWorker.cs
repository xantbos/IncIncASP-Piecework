// HourlyWorker.cs
// Last Modified: 2018-12-5
// Modified By:   Adrian Kriz
// 
// This is a class representing hourlyworker objects. It is a child class
// of the 'Worker' class, inheriting all the properties and values.
// It overrides the default ToString to better represent the class,
// and it also has a unique findPay method overriding the base class.
// Part of the IncIncPayroll Worker system.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncIncASP
{
    public class HourlyWorker : Worker
    {
        #region "Variable Declarations"

        #region "Instance Variables"

        decimal employeeHourlyWage;
        int employeeHoursWorked;

        #endregion

        #region "Constants"

        const int hourlyCap = 168;
        const int hourlyCeiling = 40;

        #endregion

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected internal HourlyWorker() { }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="name">Worker's full name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal HourlyWorker(string name, string messages, string hoursworked, string hourlywage) : base()
        {
            FullName = name;
            Messages = messages;
            HourlyWage = hourlywage;
            HoursWorked = hoursworked;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Hourly;
            employeePay = findPay(null,null);
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="firstName">Worker's first name</param>
        /// <param name="lastName">Worker's last name</param>
        /// <param name="messages">Messages sent by worker</param>
        protected internal HourlyWorker(string firstName, string lastName, string messages, string hoursworked, string hourlywage) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            HourlyWage = hourlywage;
            HoursWorked = hoursworked;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Hourly;
            employeePay = findPay(null,null);
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
        protected internal HourlyWorker(int id, string firstName, string lastName, string messages, DateTime createdAt, int hours, decimal pay) : base()
        {
            Id = id;
            creationDate = createdAt;
            FirstName = firstName;
            LastName = lastName;
            Messages = messages;
            employeeType = (int)IncIncEnumerables.WorkerTypes.Hourly;
            HoursWorked = hours.ToString();
            HourlyWage = pay.ToString();
            employeePay = findPay(null,null);
        }

        #endregion

        #region "Class Properties"

        /// <summary>
        /// Property to get/set this Worker's hours worked
        /// </summary>
        protected internal string HoursWorked
        {
            get { return employeeHoursWorked.ToString(); }
            set { if (!int.TryParse(value, out employeeHoursWorked)) { throw new Exception("You must enter a number for hours worked."); }; }
        }

        /// <summary>
        /// Property to get/set this Worker's hourly wage
        /// </summary>
        protected internal string HourlyWage
        {
            get { return employeeHourlyWage.ToString(); }
            set { if (!Decimal.TryParse(value, out employeeHourlyWage)) { throw new Exception("You must enter valid currency for hourly wage."); }; }
        }

        #endregion

        #region "Functions"

        /// <summary>
        /// Currently called in the constructor, the findPay() method is
        /// used to calculate a worker's pay using threshold values to
        /// change how much a worker is paid per message.
        /// Asks for two values, send nulls as this is a leftover of parent Worker
        /// </summary>
        protected internal override decimal findPay(int[] messageCapArray, double[] messagePayoutArray)
        {
            // multiply hours worked by wages
            // if hours worked exceed 40, pay 50% more for each hour worked over 40

            int overtimeHours = 0; // create overtime pay counter
            decimal currentPay = 0M; // set our current working pay to zero

            if (employeeHoursWorked <= hourlyCeiling)
            {
                // do normal pay
                currentPay = employeeHoursWorked * employeeHourlyWage;
            }
            else
            {
                // do overtime pay
                overtimeHours = employeeHoursWorked - hourlyCeiling;
                currentPay = hourlyCeiling * employeeHourlyWage;
                currentPay += overtimeHours * (employeeHourlyWage * (Decimal)1.5);
            }

            return currentPay;
        }
        
        /// <summary>
        /// Custom update pay, uses class array values
        /// </summary>
        protected internal override void UpdatePay()
        {
            employeePay = findPay(null,null);
        }

        /// <summary>
        /// Overrides the ToString functionality to give a
        /// more descriptive look at our object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string formattedOutput = "{0} - {1} messages - {2} - Hourly Worker";
            return String.Format(formattedOutput, FullName, Messages, Pay.ToString("c"));
        }

        #endregion
    }
}
// DBL.vb
//         Title: DBL - Data Base Layer for Piecework Payroll
// Last Modified: 
//    Written By: 
// Adapted from PieceworkWorker by Kyle Chapman, October 2017
// 
// This is a module with a set of classes allowing for interaction between
// Piecework Worker data objects and a database.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IncIncASP
{
    class DBL
    {

        #region "Connection String"

        internal class Conn
        {

            internal static string GetConnectionString()
            {
                return Properties.Settings.Default.connectionString;
            }

        }

        #endregion

        #region "SQL Statements"

        /// <summary>
        /// Prepare SQL statements used to perform necessary actions in the database
        /// </summary>
        internal class SQLStatements
        {
            internal const string SelectById = "SELECT TOP 1 * FROM [tblEntries] WHERE [EntryId] = @entryId";
            internal const string SelectAll = "SELECT * FROM [tblEntries]";
            internal const string InsertNew = "INSERT INTO tblEntries VALUES(@firstName, @lastName, @messages, @pay, @entryDate, @workerType, @hoursWorked, @hourlyPay)";
            internal const string UpdateExisting = "UPDATE tblEntries Set FirstName = @firstName, LastName = @lastName, Messages = @messages, Pay = @pay,  WorkerType = @workerType WHERE EntryId = @entryId";
            internal const string UpdateExistingHourly = "UPDATE tblEntries Set FirstName = @firstName, LastName = @lastName, Messages = @messages, Pay = @pay,  WorkerType = @workerType, HoursWorked = @hoursWorked, HourlyPay = @hourlyPay WHERE EntryId = @entryId";
            internal const string DeleteExisting = "DELETE FROM [tblEntries] WHERE [EntryId] = @entryId";

            // These additional statements may be used to replace the summary values used in the class
            internal const string TotalPay = "SELECT SUM(Pay) FROM tblEntries";
            internal const string TotalMessages = "SELECT SUM(Messages) FROM tblEntries";
            internal const string TotalEmployees = "SELECT COUNT(EntryId) FROM tblEntries";
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Function used to select one row from database, takes workerID as the primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static object GetOneRow(int id)
        {
            // Declare new worker object and dbConnection
            PieceworkWorker returnWorker = new PieceworkWorker();
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new SQL command, assign it prepared statement
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.SelectById;
            command.Parameters.AddWithValue("@entryId", id);

            // Try to connect to the database, create a datareader. If successful, read from the database and fill created row
            // with information from matching record
            try
            {
                dbConnection.Open();
                IDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    returnWorker = new PieceworkWorker(reader.GetString(1), reader.GetString(2), reader.GetString(3));
                    returnWorker.Id = id;
                }
            }
            catch (Exception ex)
            {
                // System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }

            // Return the populated row
            return returnWorker;

        }

        /// <summary>
        /// Returns all workers in the database as a list of worker objects
        /// </summary>
        /// <returns></returns>
        internal static List<Worker> GetEmployeeList()
        {
            // Declare the connection
            SqlConnection dbConnection = new SqlConnection(Conn.GetConnectionString());

            // Create new SQL command, assign it prepared statement
            SqlCommand commandString = new SqlCommand(SQLStatements.SelectAll, dbConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(commandString);

            // Declare a DataTable object that will hold the return value
            DataTable employeeTable = new DataTable();

            // Declare a list of workers to store converted database pulls
            List<Worker> workerList = new List<Worker>();

            // Try to connect to the database, and use the adapter to fill the table
            try
            {
                dbConnection.Open();
                adapter.Fill(employeeTable);
            }
            catch (Exception ex)
            {
                // System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }

            // cycle through each row in our table
            foreach (DataRow item in employeeTable.Rows)
            {
                Worker thisWorker; // generic object creation

                // if tree to determine which worker type we want to create
                // uses enumerable comparisons in case things change later so we don't have
                // to worry about updating a bunch of static values everywhere
                if(Int32.Parse(item["workertype"].ToString()) == (int)IncIncEnumerables.WorkerTypes.Regular)
                {
                    thisWorker = new PieceworkWorker(
                        Int32.Parse(item["entryId"].ToString()),
                        item["firstName"].ToString(),
                        item["lastName"].ToString(),
                        item["messages"].ToString(),
                        Convert.ToDateTime(item["entryDate"])
                        );
                }
                else if(Int32.Parse(item["workertype"].ToString()) == (int)IncIncEnumerables.WorkerTypes.Senior)
                {
                    thisWorker = new SeniorWorker(
                        Int32.Parse(item["entryId"].ToString()),
                        item["firstName"].ToString(),
                        item["lastName"].ToString(),
                        item["messages"].ToString(),
                        Convert.ToDateTime(item["entryDate"])
                        );
                }
                else if (Int32.Parse(item["workertype"].ToString()) == (int)IncIncEnumerables.WorkerTypes.Hourly)
                {
                    thisWorker = new HourlyWorker(
                        Int32.Parse(item["entryId"].ToString()),
                        item["firstName"].ToString(),
                        item["lastName"].ToString(),
                        item["messages"].ToString(),
                        Convert.ToDateTime(item["entryDate"]),
                        Int32.Parse(item["hoursWorked"].ToString()),
                        Decimal.Parse(item["hourlyPay"].ToString())
                        );
                }
                else // catch-all we should never see unless some wise guy tampered with the db
                {
                    throw new ArgumentException("Non-existent worker type found in database.");
                }

                workerList.Add(thisWorker);
            }

            // Return an array of worker objects
            return workerList;//.ToArray();

            // Return the populated DataTable's DataView
            // return employeeTable;
        }

        /// <summary>
        /// Inserts a worker into the database
        /// </summary>
        /// <param name="insertWorker"></param>
        /// <returns></returns>
        internal static bool InsertNewRecord(Worker insertWorker)
        {
            // Create return value and dbConnection
            bool returnValue = false;
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new command, assign it prepared statement, and assign it paramaters
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.InsertNew;
            command.Parameters.AddWithValue("@firstName", insertWorker.FirstName);
            command.Parameters.AddWithValue("@lastName", insertWorker.LastName);
            command.Parameters.AddWithValue("@messages", insertWorker.Messages);
            command.Parameters.AddWithValue("@pay", insertWorker.Pay);
            command.Parameters.AddWithValue("@entryDate", insertWorker.EntryDate);
            command.Parameters.AddWithValue("@workerType", insertWorker.Type);

            // we only want to add the hourly content if our worker is an hourly
            // otherwise we send nulls since the datatype is nullable
            if(insertWorker.Type == (int)IncIncEnumerables.WorkerTypes.Hourly)
            {
                command.Parameters.AddWithValue("@hoursWorked", Int32.Parse(((HourlyWorker)insertWorker).HoursWorked));
                command.Parameters.AddWithValue("@hourlyPay", Decimal.Parse(((HourlyWorker)insertWorker).HourlyWage));
            }
            else
            {
                command.Parameters.AddWithValue("@hoursWorked", DBNull.Value);
                command.Parameters.AddWithValue("@hourlyPay", DBNull.Value);
            }

            // Try to insert the new record, return result
            try
            {
                dbConnection.Open();
                returnValue = (command.ExecuteNonQuery() == 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                // System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }

            // Return the populated DataTable's DataView
            return returnValue;
        }

        /// <summary>
        /// Delete record from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static bool DeleteRow(int id)
        {
            // Create return value and dbConnection
            bool returnValue = false;
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new SQL command, assign it prepared statement
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.DeleteExisting;
            command.Parameters.AddWithValue("@entryId", id);

            // Attempt to open connection to DB, return result if delete was successful
            try
            {
                dbConnection.Open();
                returnValue = (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                // System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }

            // Return the populated DataTable's DataView
            return returnValue;
        }

        /// <summary>
        /// Updates the target worker's data
        /// </summary>
        /// <param name="updateWorker"></param>
        /// <returns></returns>
        internal static bool UpdateExistingRow(Worker updateWorker)
        {
            // Create return value and dbConnection
            bool returnValue = false;

            // If row exists, create dbConnection
            if (updateWorker.Id > 0)
            {
                SqlConnection dbConnection = new SqlConnection();
                dbConnection.ConnectionString = Conn.GetConnectionString();

                // Create new command, assign it a prepared SQL statement and assign it paramaters
                SqlCommand command = new SqlCommand();
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@entryId", updateWorker.Id);
                command.Parameters.AddWithValue("@firstName", updateWorker.FirstName);
                command.Parameters.AddWithValue("@lastName", updateWorker.LastName);
                command.Parameters.AddWithValue("@messages", updateWorker.Messages);
                command.Parameters.AddWithValue("@pay", updateWorker.Pay);
                command.Parameters.AddWithValue("@entryDate", updateWorker.EntryDate);
                command.Parameters.AddWithValue("@workertype", updateWorker.Type);

                // we only want to add the hourly content if our worker is an hourly
                // otherwise we send nothing since the datatype doesn't need to be sent
                if (updateWorker.Type == (int)IncIncEnumerables.WorkerTypes.Hourly)
                {
                    command.CommandText = SQLStatements.UpdateExistingHourly;
                    command.Parameters.AddWithValue("@hoursWorked", Int32.Parse(((HourlyWorker)updateWorker).HoursWorked));
                    command.Parameters.AddWithValue("@hourlyPay", Decimal.Parse(((HourlyWorker)updateWorker).HourlyWage));
                }
                else
                {
                    command.CommandText = SQLStatements.UpdateExisting;
                }


                // Try to open a connection to the database and update the record. Return result.
                try
                {
                    dbConnection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
                    // System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
                }
                finally
                {
                    dbConnection.Close();
                }

            }

            // If the worker does not exist, attempt to insert it instead
            else
            {
                if (InsertNewRecord(updateWorker))
                {
                    returnValue = true;
                }
            }

            // Returns true if the query executed; always false if the row is invalid
            return returnValue;
        }

        /// <summary>
        /// Returns a count of all messages sent by all workers
        /// </summary>
        /// <returns></returns>
        internal static int GetMessagesTotal()
        {
            // Declare return value
            int returnValue = 0;

            // Declare dbConnection
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new SQL command, assign it prepared statement
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.TotalMessages;

            try
            {
                dbConnection.Open();
                Console.WriteLine(command.ExecuteScalar().ToString());
                returnValue = Convert.ToInt32(command.ExecuteScalar().ToString());
            }
            catch(Exception ex)
            {
                // error goes here
            }
            finally
            {
                dbConnection.Close();
            }

            return returnValue;
        }

        /// <summary>
        /// Returns a count of all pay totals of our workers
        /// </summary>z
        /// <returns></returns>
        internal static decimal GetPayTotal()
        {
            // Declare return value
            decimal returnValue = 0M;

            // Declare dbConnection
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new SQL command, assign it prepared statement
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.TotalPay;

            try
            {
                dbConnection.Open();
                returnValue = Convert.ToDecimal(command.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                // error goes here
            }
            finally
            {
                dbConnection.Close();
            }

            return returnValue;
        }

        /// <summary>
        /// Returns a count of all workers
        /// </summary>
        /// <returns></returns>
        internal static int GetEmployeeTotal()
        {
            // Declare return value
            int returnValue = 0;

            // Declare dbConnection
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = Conn.GetConnectionString();

            // Create new SQL command, assign it prepared statement
            SqlCommand command = new SqlCommand();
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = SQLStatements.TotalEmployees;

            try
            {
                dbConnection.Open();
                returnValue = Convert.ToInt32(command.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                // error goes here
            }
            finally
            {
                dbConnection.Close();
            }

            return returnValue;
        }
        #endregion



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;
using MySql.Data.MySqlClient;

namespace F2022SchoolDb.Models
{
    public class SchoolDbContext
    {
        // giving user name 
        private static string User { get { return "root"; } }

        // giving password 
        private static string Password { get { return "root"; } }

        // giving database name
        private static string Database { get { return "schooldb2022"; } }

        // giving server
        private static string Server { get { return "localhost"; } }

        //giving port id
        private static string Port { get { return "3306"; } }

        // Connectionstring is a series of credentials used to connect to the database
        protected static string ConnectionString
        {
            get
            {
                // Convert Zero Datetime is a setting that will interpret a 0000-00-00 as null
                // This makes it easier for C# to convert to a proper DateTime type
                return "server = " + Server
                    + "; user = " + User
                    + "; Database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }

        //This method is used to get database
        ///<summary>
        ///Return a connection to schooldb2022 database
        ///</summary>
        ///<example>
        ///private SchoolDbContext School = new SchoolDbContext();
        ///MySqlConnection Conn = School.AccessDatabase();
        ///</example>
        ///<returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            //We are instantiating the MySqlConnection Class to create an object
            //the object is a specific connection to our blog database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}
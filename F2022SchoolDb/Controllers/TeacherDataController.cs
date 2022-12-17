using F2022SchoolDb.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Web.Http.Cors;

namespace F2022SchoolDb.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table of our schooldb2022 database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET api/teacherData/ListTeachers</example>
        /// <returns>
        /// A list of Teacher objects.
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname,' ',teacherlname)) like lower(@key)";
            //adding securtiy to our LIKE function so there is no possibility of sql injection
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                string HireDate = ResultSet["hiredate"].ToString() ;
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();
                NewTeacher.teacherid = teacherId;
                NewTeacher.teacherfname = TeacherFname;
                NewTeacher.teacherlname = TeacherLname;
                NewTeacher.employeenumber = EmployeeNumber;
                NewTeacher.hiredate = HireDate;
                NewTeacher.salary = Salary;

                //Add teachers to the list
                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;
        }

        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherid
        /// </summary>
        /// <param name="id">the teacher's ID in the database</param>
        /// <returns>A Teacher object</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of connection
            MySqlConnection Conn = School.AccessDatabase();

            //open the connection between the web server and database
            Conn.Open();

            //Esatblish a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "select * from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                string HireDate = ResultSet["hiredate"].ToString();
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                NewTeacher.teacherid = teacherId;
                NewTeacher.teacherfname = TeacherFname;
                NewTeacher.teacherlname = TeacherLname;
                NewTeacher.employeenumber = EmployeeNumber;
                NewTeacher.hiredate = HireDate;
                NewTeacher.salary = Salary;
            }

            return NewTeacher;
        }

        /// <summary>
        /// Deletes a Teacher from the connected MySQL Database if the ID of that teacher exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //opening connection to database
            MySqlConnection Conn = School.AccessDatabase();

            //open the connection between database
            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }


        /// <summary>
        /// Adds a Teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        /// "Teacherid":"10"
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"Employeenumber" : "E345"
        ///	"HireDate":22/02/2012
        /// }
        /// </example>

        [HttpPost]
        [EnableCors(origins: "*", methods: "*",headers: "*")]

        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "insert into teachers (teacherid,teacherFname,teacherLname,employeenumber,hiredate,salary) values (@teacherid,@teacherFname,@teacherLname,@employeenumber,@hiredate,@salary)";
            cmd.Parameters.AddWithValue("@teacherid", NewTeacher.teacherid);
            cmd.Parameters.AddWithValue("@teacherFname", NewTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherLname", NewTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }


        /// <summary>
        /// Update a teacher in the system
        /// <param name="teacherId">The id of teacher in the system</param>
        /// <param name="UpdatedTeacher">post content, every column of teacher</param>
        /// </summary>
        /// <example>
        /// api/TeacherData/updateteacher/6</example>
        /// POST: POST CONTENT / FROM BODY / REQUEST BODY
        /// {"teacherid":"6","teacherid":"6","teacherid":"6","teacherid":"6","teacherid":"6","teacherid":"6"}

        [HttpPost]
        public void UpdateTeacher(int teacherid, [FromBody]Teacher UpdatedTeacher) 
        {
            Debug.WriteLine("Updating teaacher with an id of" + teacherid);
            Debug.WriteLine("Post CONTENT");
            Debug.WriteLine(UpdatedTeacher.teacherfname);
            Debug.WriteLine(UpdatedTeacher.teacherlname);

            string query = "update teachers set teacherFname=@teacherFname, teacherLname=@teacherLName, employeenumber=@employeenumber, hiredate=@hiredate, salary=@salary where teacherid=@teacherid";

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@teacherid", teacherid);
            cmd.Parameters.AddWithValue("@teacherFname", UpdatedTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherLname", UpdatedTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", UpdatedTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", UpdatedTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", UpdatedTeacher.salary);

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
    }
}

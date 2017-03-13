using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRangerEntities;

namespace AgeRangerRepository
{
    public class Repo : IDisposable
    {
        // This would be wrapped properly... Either set it from outside or will default to this.
        public string DbFile = @"c:\DataBase\AgeRanger.db";
        private SQLiteConnection _Connection;


        public SQLiteConnection Connection {
            get
            {
                if (_Connection == null)
                    _Connection = GetAndOpenConnection();
                return _Connection;
            }
        }
        
        public Repo()
        {
            GetAndOpenConnection();
        }


        public SQLiteConnection GetAndOpenConnection()
        {
            if (_Connection != null)
                return _Connection;

            if (!File.Exists(DbFile))
                throw new ApplicationException("Cannont find DB file!");

            try
            {
                var conString = string.Format("Data Source={0};Version=3;New=False;Compress=True;", DbFile);
                _Connection = new SQLiteConnection(conString);
                _Connection.Open();
                return _Connection;
            }
            catch
            {
                return null;
            }
        }

        public void CloseConnection()
        {
            // Any tidying up to do?
            if (_Connection == null)
                return;

            _Connection.Close();
            _Connection.Dispose();
            _Connection = null;
        }
        
        public int AddPerson(Person testPerson)
        {
            int result = -1;

            using (SQLiteCommand cmd = new SQLiteCommand(Connection))
            {
                cmd.CommandText = "INSERT INTO Person(FirstName, LastName, Age) VALUES (@FIRST, @LAST, @AGE)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@FIRST", testPerson.First);
                cmd.Parameters.AddWithValue("@LAST", testPerson.Last);
                cmd.Parameters.AddWithValue("@AGE", testPerson.Age);
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
        
        public List<Person> GetAllPeople()
        {
            List<Person> result = new List<Person>();

            string sql = "SELECT * FROM Person";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, Connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new Person();
                        person.Id = Int32.Parse(reader["Id"].ToString());
                        person.First = reader["FirstName"].ToString();
                        person.Last = reader["LastName"].ToString();
                        person.Age = Int32.Parse(reader["Age"].ToString());

                        person.Group = GetAgeGroupForThisAge(person.Age);

                        result.Add(person);
                    }
                }
            }
            
            return result;
        }

        public AgeGroup GetAgeGroupForThisAge(int age)
        {
            AgeGroup result = null;

            string sql = string.Format("SELECT * FROM AgeGroup WHERE {0} >= MinAge AND {0} < MaxAge ", age);
                    
            using (SQLiteCommand cmd = new SQLiteCommand(sql, Connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var range = new AgeGroup();
                        range.Id = Int32.Parse(reader["Id"].ToString());
                        range.MinAge = Int32.Parse(reader["MinAge"].ToString());
                        range.MaxAge = Int32.Parse(reader["MaxAge"].ToString());
                        range.Description = reader["Description"].ToString();
                        result = range;
                    }
                }
            }
            return result;
        }
        
        public Person GetPersonByFirstName(string first)
        {
            Person result = null;

            string sql = string.Format("SELECT * FROM Person WHERE FirstName = '{0}'", first);

            using (SQLiteCommand cmd = new SQLiteCommand(sql, Connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new Person();
                        person.Id = Int32.Parse(reader["Id"].ToString());
                        person.First = reader["FirstName"].ToString();
                        person.Last = reader["LastName"].ToString();
                        person.Age = Int32.Parse(reader["Age"].ToString());

                        person.Group = GetAgeGroupForThisAge(person.Age);

                        result = person;
                    }
                }
            }
            return result;
        }
        
        public Person GetPersonByLastName(string last)
        {
            Person result = null;

            string sql = string.Format("SELECT * FROM Person WHERE LastName = '{0}'", last);

            using (SQLiteCommand cmd = new SQLiteCommand(sql, Connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new Person();
                        person.Id = Int32.Parse(reader["Id"].ToString());
                        person.First = reader["FirstName"].ToString();
                        person.Last = reader["LastName"].ToString();
                        person.Age = Int32.Parse(reader["Age"].ToString());

                        person.Group = GetAgeGroupForThisAge(person.Age);

                        result = person;
                    }
                }
            }
            return result;
        }




        #region IDisposable Support

        // This disposable code probably isn't required,
        // most of it is auto-gen code.

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CloseConnection();
                }
                
                disposedValue = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}


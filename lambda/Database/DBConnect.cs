using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using AltV.Net;
using MySql.Data.MySqlClient;

namespace Lambda.Database
{
    public class DBConnect
    {
        //private MySqlConnection connection;
        private string connectionString;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        /// <summary>
        /// Default DbConnect
        /// </summary>
        /// <param name="connectionString">If a specific connection have to be specified</param>
        public DBConnect()
        {
            server = "localhost";
            database = "lambda";
            uid = "root";
            port = "3306";
            password = "";
            connectionString = $"server={server}; database={database}; uid={uid}; pwd='{password}'; port={port}";
            connectionString = $"server=149.91.90.131; database=lambda; uid=server; pwd=MaZmPcs7nt; port=3306";
        }
        /// <summary>
        /// Open the connection
        /// </summary>
        public MySqlConnection OpenConnection()
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
            //Alt.Log(mySqlConnection.ConnectionString);
            //Alt.Log(mySqlConnection.Database);
            try
            {
                //if (connection.State != ConnectionState.Open)
                mySqlConnection.Open();

                return mySqlConnection;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Alt.Log("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Alt.Log("Invalid username/password, please try again");
                        break;
                    default:
                        Alt.Log("Can't connect to the database");
                        Alt.Log(ex.ErrorCode + "");
                        break;
                }
                return null;
            }
        }
        /// <summary>
        /// Close the connection
        /// </summary>
        public bool CloseConnection(MySqlConnection connection)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Insert to the database
        /// </summary>
        /// <param name="table">The table to be insered</param>
        /// <param name="data">The dictionary of datas</param>
        public long Insert(string table, Dictionary<string, string> data)
        {
            MySqlConnection connection = OpenConnection();
            string query = DataToInsertQuery(table, data);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            int rowsAffected = cmd.ExecuteNonQuery();
            long last = cmd.LastInsertedId;
            CloseConnection(connection);
            return last;
        }


        /// <summary>
        /// Update the database
        /// </summary>
        /// <param name="table">The table to be updated</param>
        /// <param name="data">The dictionary of datas</param>
        /// <param name="wheres">The dictionary of conditions</param>
        public int Update(string table, Dictionary<string, string> datas, Dictionary<string, string> wheres)
        {
            MySqlConnection connection = OpenConnection();
            string query = DataToUpdateQuery(table, datas, wheres);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            int rowsAffected = cmd.ExecuteNonQuery();
            CloseConnection(connection);
            return rowsAffected;
        }
        /// <summary>
        /// Delete in the database
        /// </summary>
        /// <param name="table">The table where we delete</param>
        /// <param name="wheres">Conditions</param>
        /// <returns></returns>
        public int Delete(string table, Dictionary<string, string> wheres)
        {
            MySqlConnection connection = OpenConnection();
            string query = DataToDeleteQuery(table, wheres);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            int rowsAffected = cmd.ExecuteNonQuery();
            CloseConnection(connection);
            return rowsAffected;
        }

        /// <summary>
        /// Select the first item of a qurty result
        /// </summary>
        public Dictionary<string, string> SelectOne(string table, Dictionary<string, string> wheres, string[] fields = null)
        {
            MySqlConnection connection = OpenConnection();
            string query = DataToSelectQuery(table, wheres, fields);
            Dictionary<string, string> result = new Dictionary<string, string>();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    result[dataReader.GetName(i)] = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i).ToString();
                }
            }
            else
            {
                return result;
            }
            dataReader.Close();
            CloseConnection(connection);
            return result;
        }

        /// <summary>
        /// Select all items of a query result
        /// </summary>
        public List<Dictionary<string, string>> Select(string table, Dictionary<string, string> wheres, string[] fields = null)
        {
            MySqlConnection connection = OpenConnection();
            string query = DataToSelectQuery(table, wheres, fields); // Set data to a query
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

            MySqlCommand cmd = new MySqlCommand(query, connection);
            Alt.Log("Execute reader");
            MySqlDataReader dataReader = cmd.ExecuteReader();
            Alt.Log("Lecture des données");
            while (dataReader.Read())
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    //result[dataReader.GetName(i)] = dataReader.IsDBNull(i) ? null : dataReader.GetString(i);
                    if (!dataReader.IsDBNull(i))
                    {
                        result[dataReader.GetName(i)] = dataReader[dataReader.GetName(i)] + "";
                    }
                }
                results.Add(result);
            }
            dataReader.Close();
            CloseConnection(connection);
            return results;
        }


        public static string DataToInsertQuery(string table, Dictionary<string, string> datas)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentException("table param is empty", nameof(table));
            foreach (KeyValuePair<string, string> data in datas)
            {
                //if (string.IsNullOrWhiteSpace(data.Key)) throw new ArgumentException("Datas keys contains empty string", nameof(datas));
                //if (string.IsNullOrWhiteSpace(data.Value)) throw new ArgumentException("Datas values contains empty string", nameof(datas));
            }
            string query = $"INSERT INTO {table} (`";
            int i = 0;
            int size = datas.Count;
            foreach (string key in datas.Keys)
            {
                query += key + "`";
                if (i++ < size - 1) query += ",`";
            }
            query += ") VALUES ('";
            i = 0;
            foreach (string value in datas.Values)
            {
                query += value + "'";
                if (i++ < size - 1) query += ",'";
            }
            query += ");";
            return query;
        }


        public static string DataToUpdateQuery(string table, Dictionary<string, string> datas, Dictionary<string, string> wheres)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentException("table param is empty", nameof(table));
            /*foreach (KeyValuePair<string, string> data in datas)
                {
                    if (string.IsNullOrWhiteSpace(data.Key)) throw new ArgumentException("Datas keys contains empty string", nameof(datas));
                    if (string.IsNullOrWhiteSpace(data.Value)) throw new ArgumentException("Datas values contains empty string", nameof(datas));
                }*/
            foreach (KeyValuePair<string, string> where in wheres)
            {
                if (string.IsNullOrWhiteSpace(where.Key)) throw new ArgumentException("Wheres keys contains empty string", nameof(wheres));
                if (string.IsNullOrWhiteSpace(where.Value)) throw new ArgumentException("Wheres values contains empty string", nameof(wheres));
            }

            string query = $"UPDATE {table} SET ";
            int i = 0;
            int size = datas.Count;
            foreach (KeyValuePair<string, string> data in datas)
            {
                query += $"`{data.Key}` = '{data.Value}'";
                if (i++ < size - 1) query += " , ";
            }

            query += " WHERE ";
            i = 0;
            foreach (KeyValuePair<string, string> where in wheres)
            {
                query += $"`{where.Key}` = '{where.Value}'";
                if (i++ < wheres.Count - 1) query += " AND ";
            }

            query += ";";
            return query;
        }

        public static string DataToDeleteQuery(string table, Dictionary<string, string> wheres)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentException("table param is empty", nameof(table));
            foreach (KeyValuePair<string, string> where in wheres)
            {
                if (string.IsNullOrWhiteSpace(where.Key)) throw new ArgumentException("Wheres keys contains empty string", nameof(wheres));
                if (string.IsNullOrWhiteSpace(where.Value)) throw new ArgumentException("Wheres values contains empty string", nameof(wheres));
            }
            string query = $"DELETE FROM {table}";
            if (wheres.Count > 0)
            {
                query += " WHERE ";
                int i = 0;
                foreach (KeyValuePair<string, string> where in wheres)
                {
                    query += $"`{where.Key}` = '{where.Value}'";
                    if (i++ < wheres.Count - 1) query += " AND ";
                }
            }

            query += ";";
            return query;
        }

        public static string DataToSelectQuery(string table, Dictionary<string, string> wheres, string[] fields = null)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentException("table param is empty", nameof(table));
            foreach (KeyValuePair<string, string> where in wheres)
            {
                if (string.IsNullOrWhiteSpace(where.Key)) throw new ArgumentException("Wheres keys contains empty string", nameof(wheres));
                if (string.IsNullOrWhiteSpace(where.Value)) throw new ArgumentException("Wheres values contains empty string", nameof(wheres));
            }
            string query = $"SELECT ";
            int i;
            if (fields == null)
            {
                query += "*";
            }
            else
            {
                foreach (string field in fields)
                {
                    if (string.IsNullOrWhiteSpace(field)) throw new ArgumentException("Fields contains empty string", nameof(fields));
                }
                i = 0;
                foreach (string field in fields)
                {
                    query += $"{field}";
                    if (i++ < fields.Length - 1) query += " , ";
                }
            }

            query += $" FROM {table}";
            if (wheres.Count > 0)
            {
                query += " WHERE ";
            }
            else
            {
                query += ";";
            }
            i = 0;
            foreach (KeyValuePair<string, string> where in wheres)
            {
                query += $"`{where.Key}` = '{where.Value}'";
                if (i++ < wheres.Count - 1) query += " AND ";
            }
            query += ";";
            return query;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class TestSqlite : MonoBehaviour
{
    private string conn;

    // Start is called before the first frame update
    void Start()
    {
        conn = "URI=file:" + Application.dataPath + "/Eshtal.db";
        CreateDb();
    }

    private void CreateDb()
    {
        IDbConnection dbConn = new SqliteConnection(conn);
        dbConn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbConn.CreateCommand();
        string sqlQuery = "SELECT * from Eshtal";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            float length = reader.GetFloat(2);

            Debug.Log("value= " + id + "  name =" + name + "  random =" + length);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbConn.Close();
        dbConn = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

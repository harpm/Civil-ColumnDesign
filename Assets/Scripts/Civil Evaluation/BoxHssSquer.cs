using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class BoxHssSquer : MonoBehaviour
{
    private IDbConnection _connection;
    private IDbCommand _dbCmd;
    private IDataReader _reader;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OpenConnection()
    {
        string conn = "URI=file:" + Application.dataPath + "/Data/Eshtal.db";
        _connection = new SqliteConnection(conn);
    }

    private void RequestIpbData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"Box HSS Square\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private bool NextRow(out BoxHssSquareDataStructure data)
    {
        data = default;
        var res = false;

        if (res = _reader.Read())
        {
            data = new BoxHssSquareDataStructure()
            {
                Id = _reader.GetInt32(0),
                BoxHss = _reader.GetInt32(1).ToString(),
                Ag = _reader.GetFloat(2),
                I = _reader.GetFloat(3),
                R = _reader.GetFloat(4),
                B = _reader.GetFloat(5),
                T = _reader.GetFloat(6),
                F = _reader.GetFloat(7),

            };
        }

        return res;
    }

    private struct BoxHssSquareDataStructure
    {
        public int Id;
        public string BoxHss;
        public float Ag;
        public float I;
        public float R;
        public float B;
        public float T;
        public float F;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class BoxHssRectangular : MonoBehaviour
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
        _dbCmd.CommandText = "SELECT * from \"Box HSS Rectangular\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private bool NextRow(out BoxHssRectangularDataStructure data)
    {
        data = default;
        var res = false;

        if (res = _reader.Read())
        {
            data = new BoxHssRectangularDataStructure()
            {
                Id = _reader.GetInt32(0),
                BoxHss = _reader.GetInt32(1).ToString(),
                Ag = _reader.GetFloat(2),
                Ix = _reader.GetFloat(3),
                Iy = _reader.GetFloat(4),
                Rx = _reader.GetFloat(5),
                Ry = _reader.GetFloat(6),
                B = _reader.GetFloat(7),
                H = _reader.GetFloat(8),
                T = _reader.GetFloat(9),
                F = _reader.GetFloat(10),

            };
        }

        return res;
    }

    private struct BoxHssRectangularDataStructure
    {
        public int Id;
        public string BoxHss;
        public float Ag;
        public float Ix;
        public float Iy;
        public float Rx;
        public float Ry;
        public float B;
        public float H;
        public float T;
        public float F;
    }
}

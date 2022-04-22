using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class IPE : MonoBehaviour
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

    private void RequestIpeData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"IPE\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private bool NextRow(out IPEDataStructure data)
    {
        data = default;
        var res = false;

        if (res = _reader.Read())
        {
            data = new IPEDataStructure()
            {
                Id = _reader.GetInt32(0),
                IPE = _reader.GetInt32(1).ToString(),
                Ag = _reader.GetFloat(2),
                Ix = _reader.GetFloat(3),
                Iy = _reader.GetFloat(4),
                Rx = _reader.GetFloat(5),
                Ry = _reader.GetFloat(6),
                H = _reader.GetFloat(7),
                Tf = _reader.GetFloat(8),
                Bf = _reader.GetFloat(9),
                Df = _reader.GetFloat(10),
                Hw = _reader.GetFloat(11),
                Tw = _reader.GetFloat(12),
                A = _reader.GetFloat(13),
                Cw = _reader.GetFloat(14),
                J = _reader.GetFloat(15),
                F = _reader.GetFloat(16),

            };
        }

        return res;
    }

    private struct IPEDataStructure
    {
        public int Id;
        public string IPE;
        public float Ag;
        public float Ix;
        public float Iy;
        public float Rx;
        public float Ry;
        public float H;
        public float Tf;
        public float Bf;
        public float Df;
        public float Hw;
        public float Tw;
        public float A;
        public float Cw;
        public float J;
        public float F;
    }
}

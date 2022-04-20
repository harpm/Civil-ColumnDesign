using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class IPB : MonoBehaviour
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
        _dbCmd.CommandText = "SELECT * from IPB";
        _reader = _dbCmd.ExecuteReader();
    }

    private bool NextRow(out IPBDataStructure data)
    {
        data = default;
        var res = false;

        if (res = _reader.Read())
        {
            data = new IPBDataStructure()
            {
                Id = _reader.GetInt32(0)
            };
        }

        return res;
    }

    private void CloseConnection()
    {
        _reader.Close();
        _reader.Dispose();
        _dbCmd.Dispose();
        _connection.Close();
        _connection.Dispose();
    }

    private void Evaluate(Line mainColumn)
    {
        var topBeams = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.EndPoint) &&
            (l._curAxis != Line.Axis.Z || l._curAxis != Line.Axis.Nz));

        var topColumns = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.EndPoint) &&
            (l._curAxis == Line.Axis.Z || l._curAxis == Line.Axis.Nz));

        var botBeams = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.FirstPoint) &&
            (l._curAxis != Line.Axis.Z || l._curAxis != Line.Axis.Nz));


        var botColumns = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.FirstPoint) &&
            (l._curAxis == Line.Axis.Z || l._curAxis == Line.Axis.Nz));



        OpenConnection();
        RequestIpbData();

        IPBDataStructure data;
        while (NextRow(out data))
        {
            if (mainColumn.FirstPoint.S == 0)
            {

            }
            else
            {
                
            }
        }

        CloseConnection();
    }

    private static float G(float i, float[] cLengths, float[] bLengths, float[] b)
    {
        float res = 0.0f;

        float topTotal = 0;
        for (int j = 0; j < cLengths.Length; j++)
        {
            topTotal += (i / cLengths[j]);
        }

        float botTotal = 0;
        for (int j = 0; j < bLengths.Length; j++)
        {
            botTotal += b[j] * (i / bLengths[j]);
        }

        res = topTotal / botTotal;

        return res;
    }


    private struct IPBDataStructure
    {
        public int Id;
    }

}

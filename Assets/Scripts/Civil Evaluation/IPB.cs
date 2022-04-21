using System.Data;
using System.Linq;
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
        _dbCmd.CommandText = "SELECT * from \"IPB\"";
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
                Id = _reader.GetInt32(0),
                IPB = _reader.GetInt32(1).ToString(),
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

        var topBeamsX = topBeams.FindAll(b => b._curAxis == Line.Axis.X || b._curAxis == Line.Axis.Nx);
        var botBeamsX = botBeams.FindAll(b => b._curAxis == Line.Axis.X || b._curAxis == Line.Axis.Nx);
        var topBeamsY = topBeams.FindAll(b => b._curAxis == Line.Axis.Y || b._curAxis == Line.Axis.Ny);
        var botBeamsY = botBeams.FindAll(b => b._curAxis == Line.Axis.Y || b._curAxis == Line.Axis.Ny);


        float Gbx = 0.0f, Gtx = 0.0f, Gby = 0.0f, Gty = 0.0f, Kx = 0.0f, Ky = 0.0f;


        float[] topBX = new float[topBeamsX.Count];
        float[] topBY = new float[topBeamsY.Count];
        float[] botBY = new float[botBeamsY.Count];
        float[] botBX = new float[botBeamsX.Count];

        for (int i = 0; i < topBeams.Count; i++)
        {
            var beam = topBeamsX[i];

            var nearConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.LowerConnection : beam.HigherConnection;
            var farConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.HigherConnection : beam.LowerConnection;


            if (nearConnection == Line.ConnectionType.PinConnection)
                topBX[i] = 0.0f;
            else if (farConnection == Line.ConnectionType.FixedConnection)
                topBX[i] = 2.0f;
            else if (farConnection == Line.ConnectionType.PinConnection)
                topBX[i] = 1.5f;
            else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                topBX[i] = 2.0f / 3.0f;
            else if ((beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).IsEmpty)
                topBX[i] = 0.0f;
            else
                topBX[i] = 1.0f;
        }
        
        for (int i = 0; i < topBeams.Count; i++)
        {
            var beam = topBeamsY[i];

            var nearConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.LowerConnection : beam.HigherConnection;
            var farConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.HigherConnection : beam.LowerConnection;


            if (nearConnection == Line.ConnectionType.PinConnection)
                topBY[i] = 0.0f;
            else if (farConnection == Line.ConnectionType.FixedConnection)
                topBY[i] = 2.0f;
            else if (farConnection == Line.ConnectionType.PinConnection)
                topBY[i] = 1.5f;
            else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                topBY[i] = 2.0f / 3.0f;
            else if ((beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).IsEmpty)
                topBY[i] = 0.0f;
            else
                topBX[i] = 1.0f;
        }

        
        if (!mainColumn.IsOnGround)
        {
            for (int i = 0; i < botBeamsX.Count; i++)
            {
                var beam = botBeamsX[i];

                var nearConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.LowerConnection : beam.HigherConnection;
                var farConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.HigherConnection : beam.LowerConnection;


                if (nearConnection == Line.ConnectionType.PinConnection)
                    botBX[i] = 0.0f;
                else if (farConnection == Line.ConnectionType.FixedConnection)
                    botBX[i] = 2.0f;
                else if (farConnection == Line.ConnectionType.PinConnection)
                    botBX[i] = 1.5f;
                else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                    botBX[i] = 2.0f / 3.0f;
                else if ((beam.FirstPoint == mainColumn.FirstPoint ? beam.EndPoint : beam.FirstPoint).IsEmpty)
                    botBX[i] = 0.0f;
                else
                    botBX[i] = 1.0f;
            }

            
            for (int i = 0; i < botBeamsY.Count; i++)
            {
                var beam = botBeamsY[i];

                var nearConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.LowerConnection : beam.HigherConnection;
                var farConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.HigherConnection : beam.LowerConnection;


                if (nearConnection == Line.ConnectionType.PinConnection)
                    botBY[i] = 0.0f;
                else if (farConnection == Line.ConnectionType.FixedConnection)
                    botBY[i] = 2.0f;
                else if (farConnection == Line.ConnectionType.PinConnection)
                    botBY[i] = 1.5f;
                else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                    botBY[i] = 2.0f / 3.0f;
                else if ((beam.FirstPoint == mainColumn.FirstPoint ? beam.EndPoint : beam.FirstPoint).IsEmpty)
                    botBY[i] = 0.0f;
                else
                    botBY[i] = 1.0f;
            }
        }
        else
        {
            if (!mainColumn.IsBracedX)
            {
                Gbx = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;
            }
            else
                Kx = 1.0f;

            if (!mainColumn.IsBracedY)
            {
                Gby = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;
            }
            else
                Ky = 1.0f;


        }



        OpenConnection();
        RequestIpbData();

        IPBDataStructure data;
        while (NextRow(out data))
        {
            if (Gbx == 0 || Gby == 0)
            {
                Gbx = G(data.Ix, botColumns.Select(b => b.Length).ToArray(),
                    botBeamsX.Select(b => b.Length).ToArray(), botBX);

                Gby = G(data.Iy, botColumns.Select(b => b.Length).ToArray(),
                    botBeamsY.Select(b => b.Length).ToArray(), botBY);
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
        public string IPB;
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

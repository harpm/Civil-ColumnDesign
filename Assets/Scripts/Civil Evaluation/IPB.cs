using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;

public class IPB : MonoBehaviour
{
    private IDbConnection _connection;
    private IDbCommand _dbCmd;
    private IDataReader _reader;

    private const float E = 2400000.0f;
    private const float V = 0.3f;
    private const float Phy = 0.9f;


    private FeState lastState = default;


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

    public void Evaluate(Line mainColumn)
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

            Gbx = G(mainColumn, botColumns, botBeamsX, botBX);


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

            Gby = G(mainColumn, botColumns, botBeamsY, botBY);

            for (int i = 0; i < topBeamsX.Count; i++)
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

            Gtx = G(mainColumn, topColumns, topBeamsX, topBX);

            for (int i = 0; i < topBeamsY.Count; i++)
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

            Gty = G(mainColumn, topColumns, topBeamsY, topBY);
        }
        else
        {
            if (!mainColumn.IsBracedX)
            {
                for (int i = 0; i < topBeamsX.Count; i++)
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

                Gtx = G(mainColumn, topColumns, topBeamsX, topBX);
                Gbx = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;

                Kx = K(Gbx, Gtx);
                Ky = K(Gby, Gty);
            }
            else
                Kx = 1.0f;

            if (!mainColumn.IsBracedY)
            {
                for (int i = 0; i < topBeamsY.Count; i++)
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

                Gty = G(mainColumn, topColumns, topBeamsY, topBY);
                Gby = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;

                Ky = K(Gby, Gty);
            }
            else
                Ky = 1.0f;
        }



        OpenConnection();
        RequestIpbData();

        IPBDataStructure data;
        while (NextRow(out data))
        {
            if (!mainColumn.IsOnGround)
            {
                var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(data.Rx, data.Ry);
                var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(data.Rx, data.Ry);
                var lamba = Mathf.Max(lambdaX, lambdaY);

                if (lamba > 200.0f)
                    lamba = 200.0f;

                var feD = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lamba, 2);

                var feG = (Mathf.Pow(Mathf.PI, 2) * E * data.Cw / mainColumn.Length + (E / 2 * (1 + V)) * data.J)
                          * (1.0f / data.Iy + data.Ix);

                float Fe;

                if (feD > feG)
                {
                    Fe = feD;
                    lastState = FeState.Declined;
                }
                else
                {
                    Fe = feG;
                    lastState = FeState.Rotational;
                }

                float Fcr;

                if (mainColumn.Fy / Fe <= 2.25f)
                {
                    Fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
                }
                else
                {
                    Fcr = 0.877f * Fe;
                }

                float fu = mainColumn.ForceAD ? Mathf.Max(1.4f * mainColumn.DeadForces,
                        1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces) : mainColumn.UltimateForce;

                var Pn = Fcr * data.Ag;

                if (fu <= Pn * Phy)
                {
                    MainManager.Instance.MainWindow.StatusMessage(data.IPB + ": succeeded!", MainWindow.MessageType.Successful);
                    break;
                }
            }


        }

        CloseConnection();
    }

    private static float G(Line mainC, List<Line> columns, List<Line> beams, float[] b)
    {
        float res = 0.0f;

        float topTotal = mainC.Inertia / mainC.Length;

        for (int j = 0; j < columns.Count; j++)
        {
            topTotal += (columns[j].Inertia / columns[j].Length);
        }

        float botTotal = 0;
        for (int j = 0; j < beams.Count; j++)
        {
            botTotal += b[j] * (beams[j].Inertia / beams[j].Length);
        }

        res = topTotal / botTotal;

        return res;
    }

    private static float K(float Gb, float Gt)
    {
        return Mathf.Sqrt((1.6f * Gt * Gb + 4 * (Gt + Gb) + 7.5f) / (Gt + Gb + 7.5f));
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

    private enum FeState
    {
        Rotational,
        Declined
    }

}

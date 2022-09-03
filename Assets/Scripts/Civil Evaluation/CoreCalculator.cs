using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;

public class CoreCalculator : MonoBehaviour
{
    private IDbConnection _connection;
    private IDbCommand _dbCmd;
    private IDataReader _reader;

    private const float E = 2040000.0f;
    private const float V = 0.3f;
    private const float Phy = 0.9f;
    private static readonly float[] tps = { 8.0f, 10.0f, 12.0f, 15.0f, 18.0f, 20.0f, 25.0f };
    private string conn;

    private static FeState lastState = default;


    // Start is called before the first frame update
    void Start()
    {
        conn = "URI=file:" + Application.dataPath + "/Data/Eshtal.db";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OpenConnection()
    {
        _connection = new SqliteConnection(conn);
        _connection.Open();
    }

    private void RequestIpbData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"IPB\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private void RequestIpeData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"IPE\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private void RequestHssRectData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"Box HSS Rectanglular\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private void RequestHssSquareData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"Box HSS Square\"";
        _reader = _dbCmd.ExecuteReader();
    }

    private void RequestRoundHssData()
    {
        _dbCmd = _connection.CreateCommand();
        _dbCmd.CommandText = "SELECT * from \"Round Hss\"";
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
                H = _reader.GetFloat(7) / 10.0f,
                Tf = _reader.GetFloat(8) / 10.0f,
                Bf = _reader.GetFloat(9) / 10.0f,
                Df = _reader.GetFloat(10) / 10.0f,
                Hw = _reader.GetFloat(11) / 10.0f,
                Tw = _reader.GetFloat(12) / 10.0f,
                A = _reader.GetFloat(13) / 10.0f,
                Cw = _reader.GetFloat(14),
                J = _reader.GetFloat(15),
                F = _reader.GetFloat(16),

            };
        }

        return res;
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
                H = _reader.GetFloat(7) / 10.0f,
                Tf = _reader.GetFloat(8) / 10.0f,
                Bf = _reader.GetFloat(9) / 10.0f,
                Df = _reader.GetFloat(10) / 10.0f,
                Hw = _reader.GetFloat(11) / 10.0f,
                Tw = _reader.GetFloat(12) / 10.0f,
                A = _reader.GetFloat(13) / 10.0f,
                Cw = _reader.GetFloat(14),
                J = _reader.GetFloat(15),
                F = _reader.GetFloat(16),

            };
        }

        return res;
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
                BoxHss = _reader.GetString(1),
                Ag = _reader.GetFloat(2),
                Ix = _reader.GetFloat(3),
                Iy = _reader.GetFloat(5),
                Rx = _reader.GetFloat(4),
                Ry = _reader.GetFloat(6),
                B = _reader.GetFloat(7) / 10.0f,
                H = _reader.GetFloat(8) / 10.0f,
                T = _reader.GetFloat(9) / 10.0f,
                F = _reader.GetFloat(10),

            };
        }

        return res;
    }

    private bool NextRow(out RoundHssDataStructure data)
    {
        data = default;
        var res = false;

        if (res = _reader.Read())
        {
            data = new RoundHssDataStructure()
            {
                Id = _reader.GetInt32(0),
                RoundHss = _reader.GetString(1),
                Ag = _reader.GetFloat(2),
                I = _reader.GetFloat(3),
                R = _reader.GetFloat(4),
                D = _reader.GetFloat(5) / 10.0f,
                T = _reader.GetFloat(6) / 10.0f,
                F = _reader.GetFloat(7),

            };
        }

        return res;
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
                BoxHss = _reader.GetString(1),
                Ag = _reader.GetFloat(2),
                I = _reader.GetFloat(3),
                R = _reader.GetFloat(4),
                B = _reader.GetFloat(5) / 10.0f,
                T = _reader.GetFloat(6) / 10.0f,
                F = _reader.GetFloat(7),

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


    public ProfileCalcResult EvaluateIpb(Line mainColumn)
    {
        OpenConnection();
        RequestIpbData();
        bool findAns = false;
        ProfileCalcResult res = new ProfileCalcResult();

        IPBDataStructure data;
        while (NextRow(out data))
        {
            K(mainColumn, out float Kx, out float Ky, data.Ix, data.Iy);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(data.Rx, data.Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(data.Rx, data.Ry);
            var lambda = Mathf.Max(lambdaX, lambdaY);

            if (lambda > 200.0f)
                lambda = 200.0f;



            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;
            var Fcr = CoreCalculator.Fcr(lambda, data.Ix, data.Iy, data.Cw, mainColumn.Length, data.J, mainColumn.Fy);
            var Pn = Fcr * data.Ag;

            res = new ProfileCalcResult()
            {
                Kx = Kx,
                Ky = Ky,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                Fu = fu,
                Fcr = Fcr,
                Pn = Pn
            };

            if (fu <= Pn * Phy)
            {
                var Dpc = fu / (Pn * Phy);
                res.Answer = "Use: IPB " + data.IPB + "\nD / C : " + Dpc;
                findAns = true;
                res.Dpc = Dpc;
                break;
            }

        }

        res.IsAnswered = findAns;

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();

        return res;
    }

    public ProfileCalcResult EvaluateDoubleIpeComplete(Line mainColumn)
    {
        OpenConnection();
        RequestIpeData();
        ProfileCalcResult res = new ProfileCalcResult();
        IPEDataStructure data;
        bool findAns = false;

        while (NextRow(out data))
        {
            float tp = ChooseTb(data.Tf * 10.0f) / 10.0f;
            var Ag = 2 * (data.Ag + data.A * tp);
            var Ix = 2 * data.Ix + Mathf.Pow(tp, 3) * data.A / 6 +
                     2 * data.A * tp * Mathf.Pow((data.H / 2) + (tp / 2), 2);
            var Rx = Mathf.Sqrt(Ix / Ag);
            var Iy = 2 * data.Iy + 2 * data.Ag * Mathf.Pow(data.A, 2) / 4 + Mathf.Pow(data.A, 3) * tp / 6;
            var Ry = Mathf.Sqrt(Iy / Ag);
            var Cw = ((Mathf.Pow(data.Bf, 3) * data.Tf * Mathf.Pow(data.Df, 2)) / 12) +
                     (data.Tw * Mathf.Pow(data.Hw, 3) * Mathf.Pow(data.A, 2) / 24) +
                     ((tp * Mathf.Pow(data.A, 3) / 12) * Mathf.Pow(tp + data.Tf + data.Df, 2) / 2);
            var J = 2.0f * data.J + (2.0f / 3.0f) * data.A * Mathf.Pow(tp, 3);

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);
            var lambda = Mathf.Max(lambdaX, lambdaY);

            if (lambda > 200.0f)
                lambda = 200.0f;

            var feD = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            var g = E / (2 * (1 + V));
            var feG = (Mathf.Pow(Mathf.PI, 2) * E * Cw / Mathf.Pow(mainColumn.Length, 2) + g * J) * (1.0f / (Iy + Ix));

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

            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            var Pn = Fcr * Ag;

            res = new ProfileCalcResult()
            {
                Kx = Kx,
                Ky = Ky,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Tp = tp,
                Ag = Ag,
                Cw = Cw,
                J = J,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                FeD = feD,
                FeG = feG,
                G = g,
                Fe = Fe,
                Fcr = Fcr,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Dpc = dpc;
                res.Answer = "Use: 2 IPE " + data.IPE + " @ " + Mathf.RoundToInt(data.A) +
                                                           " + 2 PL " + " * " + tp + "\n" + "D / C: " + dpc;
                findAns = res.IsAnswered = true;
                break;
            }
        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateDoubleIpeDiagonal(Line mainColumn)
    {
        var alpha = 60.0f;
        bool findAns = false;
        OpenConnection();
        RequestIpeData();
        ProfileCalcResult res = new ProfileCalcResult();

        IPEDataStructure data;
        while (NextRow(out data))
        {
            var Ag = 2 * data.Ag;
            var Ix = 2 * data.Ix;
            var Rx = data.Rx;
            var Iy = 2 * data.Ix;
            var Ry = data.Rx;
            var Cw = 2 * (Mathf.Pow(data.Bf, 3) * data.Tf * Mathf.Pow(data.Df, 2) / 24) +
                     (data.Tw * Mathf.Pow(data.Df, 3) * Mathf.Pow(data.A, 2) / 24);
            var J = 2 * data.J;

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var a = data.A * 2 * Mathf.Tan(90 - alpha);
            var rMin = Mathf.Min(data.Rx, data.Ry);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);

            var lambdaM1 = Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(a / rMin, 2));
            var lambdaM2 = a / rMin <= 40
                ? lambdaY
                : Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(0.86f * a / rMin, 2));


            var mLambda = Mathf.Max(lambdaM1, Math.Max(lambdaM2, lambdaX));

            if (mLambda > 200.0f)
                mLambda = 200.0f;

            var fcr1 = Fcr(mLambda, Ix, Iy, Cw, mainColumn.Length, J, mainColumn.Fy);
            var Pn = 2 * fcr1 * Ag;
            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            if (fu > Phy * Pn)
                continue;

            if (a / rMin > 3.0f / 4.0f * mLambda)
                continue;

            var S = data.A / Mathf.Sin(alpha);
            var tb = ChooseTb(S * Mathf.Sqrt(12) / 140);
            var rb = tb * Mathf.Sqrt(1.0f / 12.0f);
            var lambda = S / rb;
            if (lambda > 140)
                lambda = 140;

            var Fe = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            float fcr;

            if (mainColumn.Fy / Fe <= 2.25f)
            {
                fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
            }
            else
            {
                fcr = 0.877f * Fe;
            }


            var Vux = 0.02f * fu;
            var Pb = Vux / (2 * Mathf.Sin(alpha));

            var bs = ChooseTb(Pb / (0.9f * fcr * tb));

            res = new ProfileCalcResult()
            {
                Ag = Ag,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Cw = Cw,
                J = J,
                Lambda = lambda,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                LambdaM = mLambda,
                S = S,
                A = a,
                Fcr = fcr,
                Tb = tb,
                Rb = rb,
                Vux = Vux,
                Pb = Pb,
                Pn = Pn
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: 2 IPE " + data.IPE + " @ " + Mathf.RoundToInt(data.A)
                             + " + 2 PL " + bs + " * " + tb + "\nAlpha: " + alpha + "\n" + "D / C: " + dpc;
                res.Dpc = dpc;
                findAns = res.IsAnswered = true;
                break;
            }

        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateDoubleIpeCross(Line mainColumn)
    {
        var alpha = 45.0f;
        bool findAns = false;
        OpenConnection();
        RequestIpeData();

        ProfileCalcResult res = new ProfileCalcResult();

        IPEDataStructure data;
        while (NextRow(out data))
        {
            var Ag = 2 * data.Ag;
            var Ix = 2 * data.Ix;
            var Rx = data.Rx;
            var Iy = 2 * data.Ix;
            var Ry = data.Rx;
            var Cw = 2 * (Mathf.Pow(data.Bf, 3) * data.Tf * Mathf.Pow(data.Df, 2) / 24) +
                     (data.Tw * Mathf.Pow(data.Df, 3) * Mathf.Pow(data.A, 2) / 24);
            var J = 2 * data.J;

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var a = data.A * 2 * Mathf.Tan(90 - alpha);
            var rMin = Mathf.Min(data.Rx, data.Ry);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);

            var lambdaM1 = Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(a / rMin, 2));
            var lambdaM2 = a / rMin <= 40
                ? lambdaY
                : Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(0.86f * a / rMin, 2));


            var mLambda = Mathf.Max(lambdaM1, Math.Max(lambdaM2, lambdaX));

            if (mLambda > 200.0f)
                mLambda = 200.0f;

            var fcr1 = Fcr(mLambda, Ix, Iy, Cw, mainColumn.Length, J, mainColumn.Fy);
            var Pn = 2 * fcr1 * Ag;
            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            if (fu > Phy * Pn)
                continue;

            if (a / rMin > 3.0f / 4.0f * mLambda)
                continue;

            var S = data.A / Mathf.Sin(alpha);
            var tb = ChooseTb(0.7f * S * Mathf.Sqrt(12) / 200) / 10.0f;
            var rb = tb * Mathf.Sqrt(1.0f / 12.0f);
            var lambda = 0.7f * S / rb;

            if (lambda > 140)
                lambda = 140;

            var Fe = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            float fcr;

            if (mainColumn.Fy / Fe <= 2.25f)
            {
                fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
            }
            else
            {
                fcr = 0.877f * Fe;
            }


            var Vux = 0.02f * fu;
            var Pb = Vux / (4 * Mathf.Sin(alpha));

            var bs = ChooseTb(Pb / (0.9f * fcr * tb));

            res = new ProfileCalcResult
            {
                Ag = Ag,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Cw = Cw,
                J = J,
                Kx = Kx,
                Ky = Ky,
                A = a,
                R = rMin,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                LambdaM = mLambda,
                Fu = fu,
                S = S,
                Pn = Pn,
                Tb = tb,
                Rb = rb,
                Fe = Fe,
                Fcr = fcr
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: 2 IPE " + data.IPE + " @ " + Mathf.RoundToInt(data.A)
                                                           + " + 2 PL " + bs + " * " + tb + "\nAlpha: " + alpha + "\n"
                                                           + "D / C: " + dpc;
                res.Dpc = dpc;
                findAns = res.IsAnswered = true;
                break;
            }

        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateHssBoxRect(Line mainColumn)
    {
        var lambdaR = 1.4f * Mathf.Sqrt(E / mainColumn.Fy);
        bool findAns = false;
        OpenConnection();
        RequestHssRectData();
        var res = new ProfileCalcResult();

        BoxHssRectangularDataStructure data;
        while (NextRow(out data))
        {
            K(mainColumn, out float kx, out float ky, data.Ix, data.Iy);

            var lambdaX = Mathf.Max(kx, ky) * mainColumn.Length / Mathf.Max(data.Rx, data.Ry);
            var lambdaY = Mathf.Min(kx, ky) * mainColumn.Length / Mathf.Min(data.Rx, data.Ry);
            var lambda = Mathf.Max(lambdaX, lambdaY);
            if (lambda > 140)
                lambda = 140;
            var Fe = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            float fcr;
            if (mainColumn.Fy / Fe <= 2.25f)
            {
                fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
            }
            else
            {
                fcr = 0.877f * Fe;
            }

            float Ag = data.Ag;

            if (data.H / data.T > Mathf.Sqrt(lambdaR / mainColumn.Fy))
            {
                var fel = Mathf.Pow(1.49f * (lambdaR / (data.H / data.T)), 2) * mainColumn.Fy;
                var he = data.H * (1 - 0.22f * Mathf.Sqrt(fel / fcr)) * Mathf.Sqrt(fel / fcr);
                Ag = data.Ag - (data.H - he) * data.T;
            }

            var Pn = fcr * Ag;

            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            res = new ProfileCalcResult
            {
                Kx = kx,
                Ky = ky,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                Fe = Fe,
                Fcr = fcr,
                Ag = Ag,
                Pn = Pn,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: Box HSS Rect " + data.BoxHss + " t = " + data.T + "cm" + "\nD / C: " + dpc;
                res.Dpc = dpc;
                findAns = res.IsAnswered = true;
                break;
            }
        }


        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateSquareHss(Line mainColumn)
    {
        var lambdaR = 1.4f * Mathf.Sqrt(E / mainColumn.Fy);

        bool findAns = false;
        var res = new ProfileCalcResult();

        OpenConnection();
        RequestHssSquareData();

        BoxHssSquareDataStructure data;
        while (NextRow(out data))
        {
            K(mainColumn, out float kx, out float ky, data.I, data.I);

            var lambdaX = Mathf.Max(kx, ky) * mainColumn.Length / Mathf.Max(data.R, data.R);
            var lambdaY = Mathf.Min(kx, ky) * mainColumn.Length / Mathf.Min(data.R, data.R);
            var lambda = Mathf.Max(lambdaX, lambdaY);
            if (lambda > 140)
                lambda = 140;
            var Fe = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            float fcr;
            if (mainColumn.Fy / Fe <= 2.25f)
            {
                fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
            }
            else
            {
                fcr = 0.877f * Fe;
            }

            float Ag = data.Ag;

            if (data.B / data.T > Mathf.Sqrt(lambdaR / mainColumn.Fy))
            {
                var fel = Mathf.Pow(1.49f * (lambdaR / (data.B / data.T)), 2) * mainColumn.Fy;
                var he = data.B * (1 - 0.22f * Mathf.Sqrt(fel / fcr)) * Mathf.Sqrt(fel / fcr);
                Ag = data.Ag - (data.B - he) * data.T;
            }

            var Pn = fcr * Ag;

            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            res = new ProfileCalcResult()
            {
                Kx = kx,
                Ky = ky,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                Fe = Fe,
                Fcr = fcr,
                Ag = Ag,
                Pn = Pn,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: Box HSS Square " + data.BoxHss + " t = " + data.T + " cm\nD/C: " + dpc;
                res.Dpc = dpc;
                findAns = res.IsAnswered = true;
                break;
            }
        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateRoundHss(Line mainColumn)
    {
        OpenConnection();
        RequestRoundHssData();

        var res = new ProfileCalcResult();
        bool findAns = false;

        RoundHssDataStructure data;
        while (NextRow(out data))
        {
            K(mainColumn, out float kx, out float ky, data.I, data.I);
            var lambdaX = Mathf.Max(kx, ky) * mainColumn.Length / Mathf.Max(data.R, data.R);
            var lambdaY = Mathf.Min(kx, ky) * mainColumn.Length / Mathf.Min(data.R, data.R);
            var lambda = Mathf.Max(lambdaX, lambdaY);
            if (lambda > 140)
                lambda = 140;

            var Fe = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);
            float fcr;
            if (mainColumn.Fy / Fe <= 2.25f)
            {
                fcr = Mathf.Pow(0.658f, mainColumn.Fy / Fe) * mainColumn.Fy;
            }
            else
            {
                fcr = 0.877f * Fe;
            }

            float Ag = data.Ag;

            if (data.D / data.T > 0.11f * E / mainColumn.Fy)
            {
                Ag = ((0.038f * E / mainColumn.Fy * (data.D / data.T)) + (2.0f / 3.0f)) * Ag;
            }

            var Pn = fcr * Ag;

            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            res = new ProfileCalcResult()
            {
                Kx = kx,
                Ky = ky,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                Fe = Fe,
                Fcr = fcr,
                Ag = Ag,
                Pn = Pn,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: Round HSS " + data.RoundHss + " D = " + data.D + " cm, t = " + data.T + "\nD/C: " + dpc;
                res.Dpc = dpc;
                findAns = res.IsAnswered = true;
                break;
            }
        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateReinforcedIpb(Line mainColumn)
    {
        OpenConnection();
        RequestIpbData();

        bool findAns = false;
        var res = new ProfileCalcResult();

        IPBDataStructure data;
        while (NextRow(out data))
        {
            float tp = ChooseTb(data.Tf * 10.0f) / 10.0f;
            var Ag = data.Ag + 2 * data.Bf * tp;
            var Ix = data.Ix + Mathf.Pow(tp, 3) * data.Bf / 6 + 2 * data.Bf * tp * Mathf.Pow((data.H / 2) + (tp / 2), 2);
            var Rx = Mathf.Sqrt(Ix / Ag);
            var Iy = data.Iy + Mathf.Pow(data.Bf, 3) * tp / 6;
            var Ry = Mathf.Sqrt(Iy / Ag);
            var Cw = data.Cw;
            var J = data.J + (2.0f / 3.0f) * data.Bf * Mathf.Pow(tp, 3);

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);
            var lambda = Mathf.Max(lambdaX, lambdaY);

            if (lambda > 200.0f)
                lambda = 200.0f;

            var feD = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);

            var feG = (Mathf.Pow(Mathf.PI, 2) * E * Cw / Mathf.Pow(mainColumn.Length, 2) + (E / (2 * (1 + V))) * J)
                      * (1.0f / (Iy + Ix));

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

            var Pn = Fcr * Ag;

            res = new ProfileCalcResult()
            {
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                Lambda = lambda,
                Tp = tp,
                Ag = Ag,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Cw = Cw,
                J = J,
                FeD = feD,
                FeG = feG,
                Fe = Fe,
                Fcr = Fcr,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: " + "IPB" + data.IPB + " + 2 PL " + data.Bf + " * " + tp + " cm \nD/C: " + dpc;
                res.Dpc = dpc;
                res.IsAnswered = findAns = true;
                break;
            }
        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateReinforcedIpe(Line mainColumn)
    {
        OpenConnection();
        RequestIpeData();

        var res = new ProfileCalcResult();
        bool findAns = false;

        IPEDataStructure data;
        while (NextRow(out data))
        {
            float tp = ChooseTb(data.Tf * 10.0f) / 10.0f;
            var Ag = data.Ag + 2 * data.Bf * tp;
            var Ix = data.Ix + Mathf.Pow(tp, 3) * data.Bf / 6 + 2 * data.Bf * tp * Mathf.Pow((data.H / 2) + (tp / 2), 2);
            var Rx = Mathf.Sqrt(Ix / Ag);
            var Iy = data.Iy + Mathf.Pow(data.Bf, 3) * tp / 6;
            var Ry = Mathf.Sqrt(Iy / Ag);
            var Cw = data.Cw;
            var J = data.J + (2.0f / 3.0f) * data.Bf * Mathf.Pow(tp, 3);

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);
            var lambda = Mathf.Max(lambdaX, lambdaY);

            if (lambda > 200.0f)
                lambda = 200.0f;

            var feD = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);

            var feG = (Mathf.Pow(Mathf.PI, 2) * E * Cw / Mathf.Pow(mainColumn.Length, 2) + (E / (2 * (1 + V))) * J)
                      * (1.0f / (Iy + Ix));

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

            var Pn = Fcr * Ag;

            res = new ProfileCalcResult()
            {
                Tp = tp,
                Ag = Ag,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Kx = Kx,
                Ky = Ky,
                Cw = Cw,
                J = J,
                Lambda = lambda,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                FeD = feD,
                FeG = feG,
                Fe = Fe,
                Fcr = Fcr,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = fu / (Phy * Pn);
                res.Answer = "Use: " + "IPE " + data.IPE + " + 2 PL " + data.Bf + " * " + tp + " cm \nD/C: " + dpc;
                res.Dpc = dpc;
                res.IsAnswered = findAns = true;
                break;
            }
        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }

    public ProfileCalcResult EvaluateDoubleIpeParallel(Line mainColumn)
    {
        OpenConnection();
        RequestIpeData();

        bool findAns = false;
        var res = new ProfileCalcResult();

        IPEDataStructure data;
        while (NextRow(out data))
        {
            var Ag = 2 * data.Ag;
            var Ix = 2 * data.Ix;
            var Rx = data.Rx;
            var Iy = 2 * data.Ix;
            var Ry = data.Rx;
            var Cw = 2 * (Mathf.Pow(data.Bf, 3) * data.Tf * Mathf.Pow(data.Df, 2) / 24) +
                     (data.Tw * Mathf.Pow(data.Df, 3) * Mathf.Pow(data.A, 2) / 24);
            var J = 2 * data.J;

            K(mainColumn, out float Kx, out float Ky, Ix, Iy);

            var rMin = Mathf.Min(data.Rx, data.Ry);

            var lambdaX = Mathf.Max(Kx, Ky) * mainColumn.Length / Mathf.Max(Rx, Ry);
            var lambdaY = Mathf.Min(Kx, Ky) * mainColumn.Length / Mathf.Min(Rx, Ry);

            var a = Mathf.Round((3.0f / 4.0f) * Mathf.Max(lambdaX, lambdaY) * rMin);

            var lambdaM1 = Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(a / rMin, 2));
            var lambdaM2 = a / rMin <= 40
                ? lambdaY : Mathf.Sqrt(Mathf.Pow(lambdaY, 2) + Mathf.Pow(0.86f * a / rMin, 2));


            var mLambda = Mathf.Max(lambdaM1, Math.Max(lambdaM2, lambdaX));

            if (mLambda > 200.0f)
                mLambda = 200.0f;

            var fcr1 = Fcr(mLambda, Ix, Iy, Cw, mainColumn.Length, J, mainColumn.Fy);
            var Pn = 2 * fcr1 * Ag;
            float fu = mainColumn.ForceAD
                ? Mathf.Max(1.4f * mainColumn.DeadForces,
                    1.2f * mainColumn.DeadForces + 1.6f * mainColumn.AliveForces)
                : mainColumn.UltimateForce;

            if (fu > Phy * Pn)
                continue;

            if (a / rMin > 3.0f / 4.0f * mLambda)
                continue;





            var Vux = 0.02f * fu;
            var Vb = (Vux * a) / (2.0f * data.A);
            var Mb = Vux * a / 4.0f;
            var bs = Mathf.Round((data.A + 1.0f) / 2.0f);
            if (bs >= data.A)
                bs = Mathf.Round(data.A / 2.0f);

            var maxTs = Mathf.Max(Vb / (0.6f * mainColumn.Fy * bs), (4 * Mb) / Mathf.Pow(0.9f * mainColumn.Fy * bs, 2));

            var ts = tps.First(f => f >= maxTs);

            res = new ProfileCalcResult()
            {
                Ag = Ag,
                Ix = Ix,
                Iy = Iy,
                Rx = Rx,
                Ry = Ry,
                Kx = Kx,
                Ky = Ky,
                Cw = Cw,
                J = J,
                LambdaX = lambdaX,
                LambdaY = lambdaY,
                LambdaM = mLambda,
                R = rMin,
                A = a,
                Vux = Vux,
                Fcr = fcr1,
                Vb = Vb,
                Mb = Mb,
                Bs = bs,
                Ts = ts,
                Fu = fu
            };

            if (fu <= Pn * Phy)
            {
                var dpc = res.Dpc = fu / (Phy * Pn);
                res.Answer = "Use: 2 IPE " + data.IPE + " @ " + Mathf.Round(data.A) + " + 2 PL " + Mathf.Round(data.A) +
                             " * " + bs + " * " + ts + " cm @" + "\na = " + a + " cm" + "\n\n" + "D / C: " + dpc;
                findAns = res.IsAnswered = true;
                break;
            }

        }

        if (!findAns)
        {
            res.Answer = "No answer found!";
        }

        CloseConnection();
        return res;
    }


    private static float Fcr(float lambda, float Ix, float Iy, float cw, float length, float j, float Fy)
    {
        var feD = Mathf.Pow(Mathf.PI, 2) * E / Mathf.Pow(lambda, 2);

        var g = E / (2 * (1 + V));

        var feG = (Mathf.Pow(Mathf.PI, 2) * E * cw / Mathf.Pow(length, 2) + g * j) * (1.0f / (Iy + Ix));

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

        if (Fy / Fe <= 2.25f)
        {
            Fcr = Mathf.Pow(0.658f, Fy / Fe) * Fy;
        }
        else
        {
            Fcr = 0.877f * Fe;
        }

        return Fcr;
    }

    private static void K(Line mainColumn, out float kx, out float ky, float ix, float iy)
    {
        var topBeams = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.EndPoint) &&
            (l._curAxis != Line.Axis.Z && l._curAxis != Line.Axis.Nz));

        var topColumns = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.EndPoint || l.FirstPoint == mainColumn.EndPoint) &&
            (l._curAxis == Line.Axis.Z || l._curAxis == Line.Axis.Nz) &&
            l != mainColumn);

        var botBeams = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.FirstPoint || l.FirstPoint == mainColumn.FirstPoint) &&
            (l._curAxis != Line.Axis.Z && l._curAxis != Line.Axis.Nz));


        var botColumns = MainManager.Instance.MouseManager.Lines.FindAll(l =>
            (l.EndPoint == mainColumn.FirstPoint || l.FirstPoint == mainColumn.FirstPoint) &&
            (l._curAxis == Line.Axis.Z || l._curAxis == Line.Axis.Nz) &&
            l != mainColumn);

        var topBeamsX = topBeams.FindAll(b => b._curAxis == Line.Axis.X || b._curAxis == Line.Axis.Nx);
        var botBeamsX = botBeams.FindAll(b => b._curAxis == Line.Axis.X || b._curAxis == Line.Axis.Nx);
        var topBeamsY = topBeams.FindAll(b => b._curAxis == Line.Axis.Y || b._curAxis == Line.Axis.Ny);
        var botBeamsY = botBeams.FindAll(b => b._curAxis == Line.Axis.Y || b._curAxis == Line.Axis.Ny);


        float Gbx, Gtx, Gby, Gty;

        float[] topBX = new float[topBeamsX.Count];
        float[] topBY = new float[topBeamsY.Count];
        float[] botBY = new float[botBeamsY.Count];
        float[] botBX = new float[botBeamsX.Count];

        if (!mainColumn.IsOnGround)
        {
            if (mainColumn.IsBracedX)
            {
                kx = 1.0f;
            }
            else
            {
                var inertia = 0.0f;
                if (mainColumn.IsBracedY)
                    inertia = Mathf.Max(ix, iy);
                else
                    inertia = ix;

                for (int i = 0; i < botBeamsX.Count; i++)
                {
                    var beam = botBeamsX[i];

                    var nearConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.LowerConnection : beam.HigherConnection;
                    var farConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.HigherConnection : beam.LowerConnection;

                    if (!(beam.FirstPoint == mainColumn.FirstPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        botBX[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        botBX[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        botBX[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        botBX[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        botBX[i] = 2.0f / 3.0f;
                    else
                        botBX[i] = 1.0f;
                }

                Gbx = G(mainColumn, botColumns, botBeamsX, botBX, inertia);

                for (int i = 0; i < topBeamsX.Count; i++)
                {
                    var beam = topBeamsX[i];

                    var nearConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.LowerConnection : beam.HigherConnection;
                    var farConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.HigherConnection : beam.LowerConnection;

                    if (!(beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        topBX[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        topBX[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        topBX[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        topBX[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        topBX[i] = 2.0f / 3.0f;
                    else
                        topBX[i] = 1.0f;
                }

                Gtx = G(mainColumn, topColumns, topBeamsX, topBX, inertia);

                kx = K(Gbx, Gtx);
            }

            if (mainColumn.IsBracedY)
            {
                ky = 1.0f;
            }
            else
            {
                var inertia = 0.0f;
                if (mainColumn.IsBracedX)
                    inertia = Mathf.Max(ix, iy);
                else
                    inertia = iy;

                for (int i = 0; i < botBeamsY.Count; i++)
                {
                    var beam = botBeamsY[i];

                    var nearConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.LowerConnection : beam.HigherConnection;
                    var farConnection = beam.FirstPoint == mainColumn.FirstPoint ? beam.HigherConnection : beam.LowerConnection;


                    if (!(beam.FirstPoint == mainColumn.FirstPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        botBY[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        botBY[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        botBY[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        botBY[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        botBY[i] = 2.0f / 3.0f;
                    else
                        botBY[i] = 1.0f;
                }

                Gby = G(mainColumn, botColumns, botBeamsY, botBY, inertia);

                for (int i = 0; i < topBeamsY.Count; i++)
                {
                    var beam = topBeamsY[i];

                    var nearConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.LowerConnection : beam.HigherConnection;
                    var farConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.HigherConnection : beam.LowerConnection;

                    if (!(beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        topBY[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        topBY[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        topBY[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        topBY[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        topBY[i] = 2.0f / 3.0f;
                    else
                        topBX[i] = 1.0f;
                }

                Gty = G(mainColumn, topColumns, topBeamsY, topBY, inertia);


                ky = K(Gby, Gty);
            }

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


                    if (!(beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        topBX[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        topBX[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        topBX[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        topBX[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        topBX[i] = 2.0f / 3.0f;
                    else
                        topBX[i] = 1.0f;
                }

                var inertia = 0.0f;
                if (mainColumn.IsBracedY)
                    inertia = Mathf.Max(ix, iy);
                else
                    inertia = ix;


                Gtx = G(mainColumn, topColumns, topBeamsX, topBX, inertia);
                Gbx = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;

                kx = K(Gbx, Gtx);
            }
            else
                kx = 1.0f;

            if (!mainColumn.IsBracedY)
            {
                for (int i = 0; i < topBeamsY.Count; i++)
                {
                    var beam = topBeamsY[i];

                    var nearConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.LowerConnection : beam.HigherConnection;
                    var farConnection = beam.FirstPoint == mainColumn.EndPoint ? beam.HigherConnection : beam.LowerConnection;


                    if (!(beam.FirstPoint == mainColumn.EndPoint ? beam.EndPoint : beam.FirstPoint).HasColumn)
                        topBY[i] = 0.0f;
                    else if (nearConnection == Line.ConnectionType.PinConnection)
                        topBY[i] = 0.0f;
                    else if (farConnection == Line.ConnectionType.FixedConnection)
                        topBY[i] = 2.0f;
                    else if (farConnection == Line.ConnectionType.PinConnection)
                        topBY[i] = 1.5f;
                    else if (farConnection == Line.ConnectionType.RollerSupportConnection)
                        topBY[i] = 2.0f / 3.0f;
                    else
                        topBX[i] = 1.0f;
                }

                var inertia = 0.0f;
                if (mainColumn.IsBracedX)
                    inertia = Mathf.Max(ix, iy);
                else
                    inertia = iy;

                Gty = G(mainColumn, topColumns, topBeamsY, topBY, inertia);
                Gby = mainColumn.SuppType == Line.SupportType.Fixed ? 1.0f : 10.0f;

                ky = K(Gby, Gty);
            }
            else
                ky = 1.0f;
        }
    }

    private static float ChooseTb(float tf)
    {
        float res = tps[0];
        for (int i = 0; i < tps.Length; i++)
        {
            if (i == 0)
                res = tps[i];
            else if (tf > tps[i])
                res = tps[i];
            else if (tps[i - 1] < tf && tf < tps[i])
                res = tps[i];
            else
                break;
        }

        return res;
    }

    private static float G(Line mainC, List<Line> columns, List<Line> beams, float[] b, float mainInertia)
    {
        float res = 0.0f;

        float topTotal = mainInertia / mainC.Length;

        for (int j = 0; j < columns.Count; j++)
        {
            topTotal += (columns[j].Inertia / columns[j].Length);
        }

        float botTotal = 0;
        for (int j = 0; j < beams.Count; j++)
        {
            botTotal += b[j] * (beams[j].Inertia / beams[j].Length);
        }

        if (botTotal != 0)
            res = topTotal / botTotal;

        return res;
    }

    private static float K(float Gb, float Gt)
    {
        var k = Mathf.Sqrt((1.6f * Gt * Gb + 4 * (Gt + Gb) + 7.5f) / (Gt + Gb + 7.5f));

        if (k < 1)
            k = 1.0f;

        return k;
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

    private struct RoundHssDataStructure
    {
        public int Id;
        public string RoundHss;
        public float Ag;
        public float I;
        public float R;
        public float D;
        public float T;
        public float F;
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

    private enum FeState
    {
        Rotational,
        Declined
    }

}

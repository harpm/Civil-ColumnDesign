
public abstract class CalcResult
{
    protected CalcResult(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public float? A { get; set; }
    public float? S { get; set; }
    public float? Kx { get; set; }
    public float? Ky { get; set; }
    public float? Ix { get; set; }
    public float? Iy { get; set; }
    public float? LambdaX { get; set; }
    public float? LambdaY { get; set; }
    public float? Lambda { get; set; }
    public float? LambdaM { get; set; }
    public float? Fu { get; set; }
    public float? Fcr { get; set; }
    public float? Pn { get; set; }
    public float? Dpc { get; set; }
    public float? Tp { get; set; }
    public float? Tb { get; set; }
    public float? Ag { get; set; }
    public float? Rx { get; set; }
    public float? Ry { get; set; }
    public float? Rb { get; set; }
    public float? R { get; set; }
    public float? Cw { get; set; }
    public float? J { get; set; }
    public float? FeD { get; set; }
    public float? FeG { get; set; }
    public float? G { get; set; }
    public float? Fe { get; set; }
    public float? Vux { get; set; }
    public float? Pb { get; set; }
    public float? Vb { get; set; }
    public float? Mb { get; set; }
    public float? Bs { get; set; }
    public float? Ts { get; set; }
}

public class ProfileCalcResult : CalcResult
{
    public ProfileCalcResult(string name) : base(name)
    {
        
    }

    public bool IsAnswered { get; set; }
    public string Answer { get; set; }
}

namespace ThighPhysicsController;

/// <summary>Per-bone settings for one of the five thigh flesh bone classes.</summary>
public sealed class PerBoneAmount
{
    public bool Enabled = true;
    public float Amp = 1f;
    public float AxisX = 1f;
    public float AxisY = 1f;
    public float AxisZ = 1f;
    public float RotAmp = 0.25f;
    public bool RotCalc = true;
}

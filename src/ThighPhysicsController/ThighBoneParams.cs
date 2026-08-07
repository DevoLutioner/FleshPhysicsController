namespace ThighPhysicsController;

/// <summary>
/// Shared bone parameters. Legacy fields (CollisionRadius, LeverLength, ReflectSpeed,
/// SwayAmplitude, DriveGain, Spring, PendulumDamping) are kept in the card/preset schema
/// for backward compatibility; the current physics does not consume them.
/// </summary>
public sealed class ThighBoneParams
{
    public bool IsRotationCalc = true;
    public float Damping;
    public float Elasticity;
    public float Stiffness;
    public float Inert;
    public float CollisionRadius;
    public float LeverLength;
    public float ReflectSpeed = 1f;
    public float SwayAmplitude = 0.008f;
    public float DriveGain = 0.5f;
    public float Spring = 60f;
    public float PendulumDamping = 0.55f;

    public ThighBoneParams Clone()
    {
        return new ThighBoneParams
        {
            IsRotationCalc = IsRotationCalc,
            Damping = Damping,
            Elasticity = Elasticity,
            Stiffness = Stiffness,
            Inert = Inert,
            CollisionRadius = CollisionRadius,
            LeverLength = LeverLength,
            ReflectSpeed = ReflectSpeed,
            SwayAmplitude = SwayAmplitude,
            DriveGain = DriveGain,
            Spring = Spring,
            PendulumDamping = PendulumDamping,
        };
    }
}

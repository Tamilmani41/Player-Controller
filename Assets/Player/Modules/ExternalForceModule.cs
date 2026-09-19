using System.Diagnostics;

/// <summary>
/// This modules contributes the external forces that comes from the environment.
/// </summary>
public class ExternalForceModule
{
    public Vector2Data externalForceContribution{ get; private set;}
    public void Tick(Vector2Data _pVelocity)
    {
        externalForceContribution = MPlatformContribution(_pVelocity);
    }
    public Vector2Data MPlatformContribution(Vector2Data platformVelocity)
    {
        return platformVelocity;
    }
}

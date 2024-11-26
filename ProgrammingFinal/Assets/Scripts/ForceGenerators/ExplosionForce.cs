using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : ForceGenerator3D
{
    //Location of detonation
    public Vector3 detonation;
    //Radius of implosion where objects are effected
    public float implosionMaxRadius;
    //Radius of implosion where objects are too close to be effected
    public float implosionMinRadius;
    //Length of implosion
    public float implosionDuration;
    //The force of the implosion, which should be minimal
    public float implosionForce;

    public override void UpdateForce(Particle2D particle)
    {
        throw new System.NotImplementedException();
    }
    public override void UpdateForce(Particle3D particle)
    {
        Vector3 displacement = detonation - particle.transform.position;
        float inv_r2 = 1.0f / displacement.sqrMagnitude;
        Vector3 force = implosionForce * inv_r2 * displacement.normalized;

        particle.AddForce(force);
    }

    public bool CheckRadius(Particle3D particle)
    {
        Vector3 displacement = detonation - particle.transform.position;
        float distance = displacement.magnitude;
        if (Mathf.Abs(distance) < implosionMaxRadius && Mathf.Abs(distance) > implosionMinRadius)
        {
            return true;
        }
        return false;
    }
}

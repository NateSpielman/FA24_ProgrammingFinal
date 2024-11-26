using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : ForceGenerator3D
{
    [Header("Implosion")]
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

    [Header("Shockwave")]
    public float shockwaveSpeed;
    public float shockwaveThickness;
    public float peakConcussiveForce;
    public float concussionDuration;

    [Header("Convection Chimney")]
    public float peakConvectionForce;
    public float chimneyRadius;
    public float chimneyHeight;

    private float timeElapsed;

    public void setTime(float time)
    {
        timeElapsed = time;
    }

    public override void UpdateForce(Particle2D particle)
    {
        throw new System.NotImplementedException();
    }
    public override void UpdateForce(Particle3D particle)
    {
        Vector3 force = new Vector3();

        if (timeElapsed < implosionDuration)
        {
            Vector3 displacement = detonation - particle.transform.position;
            float inv_r2 = 1.0f / displacement.sqrMagnitude;
            force = implosionForce * inv_r2 * displacement.normalized;
        }
        else if (timeElapsed < implosionDuration + concussionDuration)
        {
            Vector3 direction = particle.transform.position - detonation;
            float distance = direction.magnitude;
            float shockwaveTravelDist = shockwaveSpeed * (timeElapsed - implosionDuration);

            //check if we are still in the shockwave
            if(distance > shockwaveTravelDist + shockwaveThickness || distance < shockwaveTravelDist - shockwaveThickness)
            {
                //not in shockwave
                force = new Vector3(0, 0, 0);
            }
            else
            {
                float power = peakConcussiveForce / (timeElapsed - implosionDuration);
                force = direction * power;
            }
        }

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

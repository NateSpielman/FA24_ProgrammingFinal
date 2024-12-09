using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

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
            force = implosionForce * displacement.normalized;
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
                //Find the power using the inverse square law
                float power = peakConcussiveForce * (1.0f / direction.sqrMagnitude);
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

    public void setImplosionRadius(Slider slider)
    {
       implosionMaxRadius = slider.value;
    }
    public void setImplosionDuration(Slider slider)
    {
        implosionDuration = slider.value;
    }
    public void setImplosionForce(Slider slider)
    {
        implosionForce = slider.value;
    }
    public void setExplosionSpeed(Slider slider)
    {
        shockwaveSpeed = slider.value;
    }
    public void setExplosionThickness(Slider slider)
    {
        shockwaveThickness = slider.value;
    }
    public void setExplosionForce(Slider slider)
    {
        peakConcussiveForce = slider.value;
    }
    public void setExplosionDuration(Slider slider)
    {
        concussionDuration = slider.value;
    }

    //set force
    //imp. max radius = (force/100) * 2
    //imp. min radius = 1.1
    //imp. duration 0.35
    //imp. force = max radius

    //shockwave speed = (force/10) / 2
    //shockwave thickness = (force/100) / 2. minimum 1.
    //concusion duration = (force/100) + 2. minimum 6
    public void setExplosion(TMP_InputField inputField)
    {
        float force = float.Parse(inputField.text);
        implosionMaxRadius = (force / 100f) * 2f;
        implosionMinRadius = 1.1f;
        implosionDuration = 0.35f;
        implosionForce = implosionMaxRadius;

        shockwaveSpeed = (force / 10f) / 2f;
        shockwaveThickness = (force / 100f) / 2;
        if (shockwaveThickness < 1)
            shockwaveThickness = 1;
        concussionDuration = (force / 100f) + 2f;
        if(concussionDuration < 6)
            concussionDuration = 6;
        peakConcussiveForce = force;
    }
}

using System .Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ExplosionManager : MonoBehaviour
{
    public ExplosionForce explosionForce;
    public List<Particle3D> particles = new List<Particle3D>();
    private float timeElapsed;
    private bool isRunning = false;
    
    void Start()
    {
        timeElapsed = 0f;
        GetParticlesInRadius();
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            explosionForce.setTime(timeElapsed);
            if (timeElapsed < explosionForce.implosionDuration)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    if (explosionForce.CheckRadius(particles[i]))
                        explosionForce.UpdateForce(particles[i]);
                }
            }
            else if (timeElapsed < explosionForce.concussionDuration + explosionForce.implosionDuration)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    explosionForce.UpdateForce(particles[i]);
                }
            }
        }
    }


    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(explosionForce.detonation, explosionForce.implosionMaxRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(explosionForce.detonation, explosionForce.implosionMinRadius);
    }

    private void GetParticlesInRadius()
    {
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        for (int i = 0; i < spheres.Length; i++)
        {
            Sphere s1 = spheres[i];
            if(s1.GetComponent<Particle3D>() != null)
            {
                Vector3 s2ToS1 = s1.Center - explosionForce.detonation;
                float dist = s2ToS1.magnitude;
                float sumOfRadii = (s1.Radius + explosionForce.implosionMaxRadius);
                float penetration = sumOfRadii - dist;

                if (penetration > 0)
                {
                    particles.Add(s1.GetComponent<Particle3D>());
                }
            }
        }
    }

    public void RunExplosion()
    {
        isRunning = true;
        timeElapsed = 0f;
    }
}

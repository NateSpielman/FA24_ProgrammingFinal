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
        GetParticles();
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

    private void GetParticles()
    {
        Particle3D[] particlesInScene = FindObjectsOfType<Particle3D>();
        foreach (Particle3D particle in particlesInScene)
        {
            particles.Add(particle);
        }
    }

    public void RunExplosion()
    {
        isRunning = true;
        timeElapsed = 0f;
    }
}

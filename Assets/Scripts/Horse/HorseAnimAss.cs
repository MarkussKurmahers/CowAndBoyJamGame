using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseAnimAss : MonoBehaviour
{
   [SerializeField] ParticleSystem dustparticles;
    [SerializeField] GameObject[] StepSound;
   public void PlayDustParticle()
    {
        dustparticles.Play();
        Instantiate(StepSound[Random.Range(0, StepSound.Length)], transform.position, Quaternion.identity);
    }
}

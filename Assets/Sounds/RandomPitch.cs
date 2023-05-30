using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    [SerializeField] float minpitch,maxpitch;
    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(minpitch, maxpitch);
    }

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAnimSound : MonoBehaviour
{
    [SerializeField] GameObject[] BoingSound;
    public void PlayBoingSound()
    {
        Instantiate(BoingSound[Random.Range(0, BoingSound.Length)], transform.position, Quaternion.identity);
    }
}

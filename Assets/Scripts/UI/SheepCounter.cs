using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SheepCounter : MonoBehaviour
{
    [SerializeField] Text text;
    // Update is called once per frame

    void Update()
    {
        GameObject[] wolfs = GameObject.FindGameObjectsWithTag("Sheep");
        text.text = "x" + wolfs.Length;

    }
}

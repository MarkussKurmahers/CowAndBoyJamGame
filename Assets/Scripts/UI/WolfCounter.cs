using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WolfCounter : MonoBehaviour
{

    [SerializeField]Text text;
    // Update is called once per frame

    void Update()
    {
        GameObject[] wolfs = GameObject.FindGameObjectsWithTag("Wolf");
        text.text = "x" + wolfs.Length;

    }
}

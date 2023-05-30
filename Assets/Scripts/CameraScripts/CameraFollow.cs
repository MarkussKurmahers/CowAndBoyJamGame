using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    private void Update()
    {
        transform.position = playerTransform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;

    private void Update()
    {
        transform.position = PlayerController.instance.transform.position + offset;
    }
}

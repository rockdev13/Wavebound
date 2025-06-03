using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        // for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            impulseSource.GenerateImpulse();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            impulseSource.GenerateImpulse();

            StartCoroutine(ScreenShakeEverySecond());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(ScreenShakeEverySecond());
        }
    }

    private IEnumerator ScreenShakeEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            impulseSource.GenerateImpulse();
        }
    }
}

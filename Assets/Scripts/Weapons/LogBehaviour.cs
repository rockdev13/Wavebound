using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBehaviour : MonoBehaviour
{
    public Transform enemyTransform;
    public float speed = 10f;
    public int destroyDelay = 5;
    private Vector3 direction;
    public int damage;

    [Range(-10f, 10f)]
    public float yRotationOffsetMin;

    [Range(-10f, 10f)]
    public float yRotationOffsetMax;

    private void Awake()
    {
        AudioSource rockSound = GetComponent<AudioSource>();
        rockSound.Play();
    }

    private void Start()
    {
        Vector3 point = enemyTransform.position;
        direction = (point - transform.position).normalized;

        float randomYRotation = Random.Range(yRotationOffsetMin, yRotationOffsetMax);
        direction = Quaternion.Euler(0f, randomYRotation, 0f) * direction;

        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = toRotation;

        Invoke(nameof(DestroyObj), destroyDelay);
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
            enemyAI.TakeDamage(damage);
        }
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    [Header("Vignette Effect")]
    [Range(0, 1)]
    public float minIntensity;

    [Range(0, 1)]
    public float maxIntensity;

    private float currentIntensity;
    private Vignette vignette;
    public VolumeProfile volumeProfile;

    [Header("Damage properties")]
    public float maxHealth;
    private float _currentHealth;

    [Header("Hit Effects")]
    public float knockbackForce = 10f;
    public ParticleSystem bloodSplatter;

    #region private variables
    private Rigidbody rb;
    #endregion

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            UpdateVignetteEffect();
        }
    }

    private Dictionary<GameObject, Coroutine> damageCoroutines = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Get the Vignette effect from the volume profile
        for (int i = 0; i < volumeProfile.components.Count; i++)
        {
            if (volumeProfile.components[i].name == "Vignette")
            {
                vignette = (Vignette)volumeProfile.components[i];
                vignette.intensity.value = 0;
                break;
            }
        }

        CurrentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(5, collision.gameObject);

            if (!damageCoroutines.ContainsKey(collision.gameObject))
            {
                Coroutine newCoroutine = StartCoroutine(DamageEverySecond(collision.gameObject));
                damageCoroutines.Add(collision.gameObject, newCoroutine);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Store the key to remove after the loop to avoid modifying the dictionary while iterating
        if (damageCoroutines.ContainsKey(collision.gameObject))
        {
            StopCoroutine(damageCoroutines[collision.gameObject]);
            damageCoroutines.Remove(collision.gameObject);
        }
    }

    private IEnumerator DamageEverySecond(GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            TakeDamage(5, enemy);
        }
    }

    private void TakeDamage(int damage, GameObject enemy)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        UpdateVignetteEffect();

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            bloodSplatter.Play();

            Vector3 knockbackDirection = enemy.transform.forward;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (knockbackDirection * knockbackForce), Mathf.Infinity);
        }
    }

    public void EnemyDeathCoroutineCheck(GameObject enemy)
    {
        if (damageCoroutines.ContainsKey(enemy))
        {
            // find correct elements
            for (int i = 0; i < damageCoroutines.Count; i++)
            {
                if (damageCoroutines.ElementAt(i).Key == enemy)
                {
                    StopCoroutine(damageCoroutines.ElementAt(i).Value);
                }
            }

            damageCoroutines.Remove(enemy);
        }
    }

    private void UpdateVignetteEffect()
    {
        float intensityPercentage = CurrentHealth / maxHealth;
        float difference = maxIntensity - minIntensity;
        float newIntensity = minIntensity + (difference * (1 - intensityPercentage));

        vignette.intensity.value = newIntensity;
        currentIntensity = newIntensity;
    }

    private void Die()
    {
        SceneManager.LoadScene("Game");
    }
}

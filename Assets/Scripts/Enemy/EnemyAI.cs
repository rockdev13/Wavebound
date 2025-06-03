using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private AIDestinationSetter aiDestinationSetter;
    public float maxHealth; // these 2 are floats because buffs are annoying to make
    private float currentHealth;
    private bool canTakeDamage = true;
    public Damage playerDamageScr;
    public EnemyWaveManager waveManager;

    private void Awake()
    {
        waveManager = FindAnyObjectByType<EnemyWaveManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth; // this is in start because the buff needs to be applied before the currentHealth variable is set

        playerDamageScr = PlayerController.instance.gameObject.GetComponent<Damage>();

        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiDestinationSetter.target = PlayerController.instance.transform;
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            waveManager.UpdateEnemyList();
            Destroy(gameObject);
        }

        StartCoroutine(TakeDamageCooldown());
    }

    private IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.05f);
        canTakeDamage = true;
    }

    private void OnDestroy()
    {
        playerDamageScr.EnemyDeathCoroutineCheck(gameObject);
    }
}

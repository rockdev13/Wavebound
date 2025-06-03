using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogs : MonoBehaviour
{
    #region public variables
    [Header("Instances")]
    public GameObject logPrefab;
    public GameObject logStartArea;

    [Header("Settings")]
    public float enemyDetectionRadius;
    public float shootingCooldown;

    [Header("Other")]
    public LayerMask everyLayer;
    #endregion

    #region private variables
    private bool _canShoot = true;
    #endregion

    private void FixedUpdate()
    {
        Collider[] colliders =  Physics.OverlapSphere(transform.position, enemyDetectionRadius, everyLayer, QueryTriggerInteraction.Ignore);

        
        GameObject closestEnemy = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) 
                continue;
            if (!colliders[i].gameObject.CompareTag("Enemy")) 
                continue;

            if (closestEnemy == null)
            {
                closestEnemy = colliders[i].gameObject;
            }
            else if (Vector3.Distance(transform.position, closestEnemy.transform.position) > Vector3.Distance(transform.position, colliders[i].transform.position))
            {
                closestEnemy = colliders[i].gameObject;
            }
        }

        if (closestEnemy != null && _canShoot)
        {
            StartCoroutine(ShootingCooldownCoroutine());
            ShootLog(closestEnemy);
        }
        
    }

    private IEnumerator ShootingCooldownCoroutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        _canShoot = true;
    }

    private void ShootLog(GameObject enemy)
    {
        GameObject log = Instantiate(logPrefab, logStartArea.transform.position, Quaternion.identity);
        LogBehaviour logBehaviour = log.GetComponent<LogBehaviour>();
        logBehaviour.enemyTransform = enemy.transform;
    }
}

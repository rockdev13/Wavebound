using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


public class EnemyWaveManager : MonoBehaviour
{
    #region settings
    [Header("Settings")]
    public GameObject enemyPrefab;
    public float enemyCapacity;
    public float spawnRate;
    public float waveDelay;
    public float spawningDistancePlayer;
    public float enemyAmountMultiplier;
    public float enemyHealthMultiplier = 1.3f;
    #endregion

    #region Components/Instances
    [Header("Components/Instances")]
    public Collider mapCollider; // for bounds
    public TextMeshProUGUI timeRemainingText;
    public CardManager cardManager;
    #endregion

    #region privates
    private int _waveAmount = 0;
    private List<GameObject> _enemyList = new();
    private Coroutine _waveCoroutine;
    private Coroutine _timerCoroutine;
    private bool spawningEnemies = false;
    private float currentHealthMultiplier = 1.0f;

    public int WaveAmount
    {
        get
        {
            return _waveAmount;
        }
        set
        {
            _waveAmount = value;

            if (_waveAmount > 1)
            {
                cardManager.NewWave();
            }
        }
    }
    #endregion

    #region publics
    [NonSerialized] public float currentTime = 0;
    #endregion

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Assign an enemy prefab");
        }

        _waveCoroutine = StartCoroutine(StartWaves());
    }

    public void UpdateEnemyList()
    {
        _enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (_enemyList.Count <= 1 && !spawningEnemies)
        {
            SkipWave();
        }
    }

    private void SkipWave()
    {
        StopCoroutine(_waveCoroutine);
        StopCoroutine(_timerCoroutine);
        enemyCapacity = enemyCapacity * enemyAmountMultiplier;
        _waveCoroutine = StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        while (true)
        {
            WaveAmount++;

            _timerCoroutine = StartCoroutine(TimeRemainingCounter());

            spawningEnemies = true;

            for (int i = 0; i < Mathf.FloorToInt(enemyCapacity); i++)
            {
                yield return new WaitForSeconds(spawnRate);

                SpawnEnemy();
            }

            spawningEnemies = false;

            yield return new WaitForSeconds(waveDelay);

            // make the game more difficult
            enemyCapacity = enemyCapacity * enemyAmountMultiplier;
            currentHealthMultiplier = enemyHealthMultiplier * currentHealthMultiplier;
        }
    }

    private IEnumerator TimeRemainingCounter()
    {
        for (int i = 0; i < waveDelay; i++)
        {
            UpdateTimeRemainingText(waveDelay - i);
            currentTime = waveDelay - i;

            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateTimeRemainingText(float time)
    {
        string minutes = Mathf.FloorToInt(time / 60).ToString();
        string seconds = Mathf.FloorToInt(time % 60).ToString();

        if (int.Parse(seconds) < 10)
        {
            seconds = "0" + seconds;
        }
        if (int.Parse(minutes) < 10)
        {
            minutes = "0" + minutes;
        }

        timeRemainingText.text = minutes + ":" + seconds;
    }

    private Vector3 GetPosition()
    {
        Bounds bounds = mapCollider.bounds;

        float x = UnityEngine.Random.Range(mapCollider.transform.position.x + bounds.min.x, mapCollider.transform.position.x + bounds.max.x);
        float z = UnityEngine.Random.Range(mapCollider.transform.position.z + bounds.min.z, mapCollider.transform.position.z + bounds.max.z);
        float y = 0f;

        Vector3 vector = new Vector3(x, y, z);
        int maxCount = 5;
        int currentCount = 0;

        while (true)
        {
            currentCount++;

            if (Vector3.Distance(vector, PlayerController.instance.transform.position) > spawningDistancePlayer)
            {
                break;
            }
            if (currentCount >= maxCount)
            {
                Debug.LogWarning("Max count exceeded");
                break;
            }
        }

        return vector;
    }

    private void SpawnEnemy()
    {
        Vector3 position = GetPosition();
        Quaternion rotation = Quaternion.identity;

        GameObject newEnemy = Instantiate(enemyPrefab, position, rotation);
        newEnemy.GetComponent<EnemyAI>().maxHealth *= currentHealthMultiplier;
        newEnemy.transform.parent = transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelBar : MonoBehaviour
{
    public RectTransform progressBar;
    private float maxWidth;

    public EnemyWaveManager enemyWaveManager;

    private void Start()
    {
        // Store the initial width of the progress bar
        maxWidth = progressBar.rect.width;
    }

    private void Update()
    {
        // Calculate percentage of wave completed
        float remainingTime = enemyWaveManager.currentTime;
        float minTime = 0;
        float maxTime = 60;
        float percentage = (remainingTime - minTime) / (maxTime - minTime);

        // Calculate the new width based on the percentage
        float newWidth = maxWidth * (1 - percentage);

        // Assign the new width to the progress bar
        float currentHeight = progressBar.sizeDelta.y;
        progressBar.sizeDelta = new Vector2(newWidth, currentHeight);
    }
}

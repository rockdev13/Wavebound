using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatIcons : MonoBehaviour
{
    // individual variables for each icon
    public RawImage damageIcon;
    public RawImage speedIcon;
    public RawImage accuracyIcon;
    public RawImage firerateIcon;

    public RawImage[] icons;
    public TextMeshProUGUI[] iconAmounts;

    private Dictionary<RawImage, TextMeshProUGUI> iconAmountDictionary = new();

    [Header("Necessary components for stats")]
    public ShootingLogs shootingLogs;
    public LogBehaviour logBehaviour;
    public PlayerController playerController;

    private void Start()
    {
        if (icons.Length != iconAmounts.Length)
        {
            Debug.LogError("The icon array and the icon amounts array needs to be the same length");
        }

        iconAmountDictionary.Clear();

        for (int i = 0; i < icons.Length; i++)
        {
            iconAmountDictionary.Add(icons[i], iconAmounts[i]);
        }
    }

    private void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            UpdateDamageIcon(logBehaviour.damage);
            UpdateSpeedIcon(playerController.speed);
            UpdateAccuracyIcon(10 - logBehaviour.yRotationOffsetMax);
            UpdateFirerateIcon(shootingLogs.shootingCooldown);
        }
    }

    private void UpdateDamageIcon(float amount)
    {
        iconAmountDictionary[damageIcon].text = amount.ToString();
    }

    private void UpdateSpeedIcon(float amount)
    {
        iconAmountDictionary[speedIcon].text = amount.ToString();
    }

    private void UpdateAccuracyIcon(float amount)
    {
        iconAmountDictionary[accuracyIcon].text = amount.ToString();
    }

    private void UpdateFirerateIcon(float amount)
    {
        iconAmountDictionary[firerateIcon].text = amount.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseButton : MonoBehaviour
{
    public CardManager cardManager;
    public Damage damageScr;
    public LogBehaviour logBehavour;
    public ShootingLogs shootingLogsScr;

    private void RemoveCards()
    {
        cardManager.RemoveCards();
    }

    public void HealToFull()
    {
        damageScr.CurrentHealth = damageScr.maxHealth;
        RemoveCards();
    }

    public void DecreaseCooldown()
    {
        shootingLogsScr.shootingCooldown = shootingLogsScr.shootingCooldown / 2;
        RemoveCards();
    }

    public void IncreaseLogDamage()
    {
        logBehavour.damage = logBehavour.damage * 2;
        RemoveCards();
    }

    public void IncreasePlayerSpeed()
    {
        PlayerController.instance.speed++;
        RemoveCards();
    }

    public void IncreaseLogAccuracy()
    {
        logBehavour.yRotationOffsetMin /= 2;
        logBehavour.yRotationOffsetMax /= 2;
        RemoveCards();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardLayout; // horizontal layout for the cards, when its enabled it randomizes 3 cards to enable.
    
    public void NewWave()
    {
        DisplayCards();
    }

    private void DisplayCards()
    {
        cardLayout.SetActive(true);
    }

    public void RemoveCards()
    {
        cardLayout.SetActive(false);
    }
}

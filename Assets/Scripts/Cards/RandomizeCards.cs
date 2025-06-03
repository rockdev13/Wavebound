using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomizeCards : MonoBehaviour
{
    private List<GameObject> cards = new();

    private void Awake()
    {
        cards.Clear();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            cards.Add(child.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            GameObject randomCard = cards[randomIndex];
            randomCard.SetActive(true);

            cards.RemoveAt(randomIndex);
        }
    }
}

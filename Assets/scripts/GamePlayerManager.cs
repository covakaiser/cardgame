using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    //?f?b?L
    public List<int> deck = new List<int>();
    //HP
    public int heroHp;
    //cost
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int> cardDeck)
    {
        deck = cardDeck;
        heroHp = 10;
        manaCost = 10;
        defaultManaCost = 10;
    }
    public void IncreaseManaCost()
    {
        defaultManaCost++;
        manaCost = defaultManaCost;

    }
}

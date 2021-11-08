using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayerManager : MonoBehaviour
{
   public List<int> deck = new List<int>();
           

    public int heroHp;
    public int enemyHp;
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int> carddeck)
    {
        this.deck = carddeck;
        heroHp = 0;
        manaCost = 1;
        defaultManaCost = 1;
        
    }
    public void IncreaseManaCost()
    {
     defaultManaCost++;
     manaCost = defaultManaCost;
    }
   
}

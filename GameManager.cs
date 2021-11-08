using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameplayerManager player;
    public GameplayerManager enemy;
    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;
    
    [SerializeField] CardController cardPrefab;
    public Transform playerHandTransform,
                                playerFieldTransform
                               , enemyHandTransform
                               , enemyFieldTransform;

    public bool isPlayerTurn;

   
    public Transform playerHero;
    public Transform enemyHero;
    System.Random rnd = new System.Random();
    // 時間管理

    int timeCount;

    // シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        uiManager.HideResultPanel();
        player.Init(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 ,17,18,19,20
        ,21,22,23,24,25,26,27,28});
        enemy.Init(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 ,17,18,19,20
        ,21,22,23,24,25,26,27,28});
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowManaCost(player.manaCost,enemy.manaCost);
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();

    }
    
    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;

        }
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }


    public void Restart()
    {
        // handとFieldのカードを削除
        foreach (Transform card in playerHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in playerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyFieldTransform)
        {
            Destroy(card.gameObject);
        }
        // デッキを生成
        player.deck = new List<int>() { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 };
        enemy.deck = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        StartGame();
    }
    void SettingInitHand()
    {
        //カードをそれぞれに5枚配る
        for (int i = 0; i < 5; i++)
        {
            GiveCardToHand(player.deck, playerHandTransform);
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }

    }
    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }
        int d=deck.Count();
        
        int r = rnd.Next(0, d);
        int cardID = deck[r];
        deck.RemoveAt(r);
        CreateCard(cardID, hand);
    }
    void CreateCard(int cardID, Transform hand)
    {
        // カードの生成とデータの受け渡し
        CardController card = Instantiate(cardPrefab, hand, false);
        if (hand.name == "PlayerHand")
        {
            card.Init(cardID,true);
        }
        else
        {
            card.Init(cardID, false);
        }
    }

    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(enemyAI.EnemyTurn());
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 20;
        uiManager.UpdateTime(timeCount);
        

        while (timeCount>0)
        {
            yield return new WaitForSeconds(1); //１秒待機
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }
        ChangeTurn();
    }

    public void OnClickTurnEndButton()
    {
        if (isPlayerTurn)
        {
            ChangeTurn();
        }
    }
    
    public CardController[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        
    }
    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }

    }


    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);
        if (isPlayerTurn)
        {
            player.IncreaseManaCost();
            GiveCardToHand(player.deck,playerHandTransform);
        }
        else
        {
            enemy.IncreaseManaCost();
            GiveCardToHand(enemy.deck,enemyHandTransform);
        }
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
         
        TurnCalc();
    }

    public void SettingCanAttackView(CardController[] fieldCardList,bool canAttack)
    {
        foreach (CardController card in fieldCardList)
        {
            // cardを攻撃可能にする。
            card.SetCanAttack(canAttack);//cardを攻撃可能にする。
        }
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");
        // フィールドのカードを攻撃可能にする
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList,true);
    }
    
    public void CardsBattle(CardController attacker,CardController defender)
    {
        Debug.Log("CardsBattle");
        Debug.Log("attacker HP:"+attacker.model.hp);
        Debug.Log("defender HP:" + defender.model.hp);
        attacker.Attack(defender);
        defender.Attack(attacker);
        Debug.Log("attacker HP:" + attacker.model.hp);
        Debug.Log("defender HP:" + defender.model.hp);
        attacker.CheckAlive();
        defender.CheckAlive();
    }

   
    public void AttackToHero(CardController attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.heroHp += attacker.model.at;
        }
        else
        {
            player.heroHp += attacker.model.at;
        }
        attacker.SetCanAttack(false);
        CheckHeroHP();
        uiManager.ShowHeroHP(player.heroHp,enemy.heroHp);
   
    }
    public void HealToHero(CardController healer)
    {
        if (healer.model.isPlayerCard)
        {
            player.heroHp += healer.model.at;
        }
        else
        {
            player.heroHp += healer.model.at;
        }
        
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);

    }
    public void CheckHeroHP()
    {
        if (player.heroHp >= 20 || enemy.heroHp >= 20)
        {
            ShowResultPanel(player.heroHp);
            
        }
    }
    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHp);
    }
   
}

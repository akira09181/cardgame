using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardController : MonoBehaviour
{
    CardView view;         //見かけ（view)に関することを操作
    public CardModel model;       //データ（model)に関することを操作
    public CardMovement movement; //移動（movement)に関することを操作
    GameManager gameManager;

    public bool IsSpell
    {
        get { return model.spell != SPELL.NONE;  }
    }
    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
        gameManager = GameManager.instance;
    }
    public void Init(int cardId,bool isPlayer)
    {
        model = new CardModel(cardId,isPlayer);
        view.SetCard(model);
    }

   public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        SetCanAttack(false);
    }
    public void Heal(CardController friendCard)
    {
        model.Heal(friendCard);
        friendCard.RefreshView();
    }
    public void Show()
    {
        view.Show();
    }
    public void RefreshView()
    {
        view.Refresh(model);
    }
    public void SetCanAttack(bool canAttack)
    {
        model.canAttack = canAttack;
        view.SetActiveSelectablePanel(canAttack);
    }

    public void OnField()
    {
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);
        model.isFieldCard = true;
        if(model.ability == ABILITY.INIT_ATTACKABLE)
        {
            SetCanAttack(true);
        }
    }
    public void CheckAlive()
    {
        if (model.isAlive)
        {
            RefreshView();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public bool CanUseSpell()
    {

        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                
            case SPELL.DAMAGE_ENEMY_CARDS:
                // 相手フィールドのすべてのカードにこうげきする。
                CardController[] enemyCards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                if (enemyCards.Length > 0)
                {
                    return true;
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:

                return true;
            case SPELL.HEAL_FRIEND_CARD:

                return true;
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                
                break;
            case SPELL.HEAL_FRIEND_HERO:
                break;
            case SPELL.NONE:   
                return false;


        }
        return false;
    }

    //敵がいないのに攻撃しようとしている　＝＞敵AIのチェックと同様にすればよい。

    public void UseSpellTo(CardController target)
    {
      
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                //特定の敵を攻撃する
                Attack(target);
                target.CheckAlive();
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                // 相手フィールドのすべてのカードにこうげきする。
                CardController[] enemyCards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                foreach(CardController enemyCard in enemyCards)
                {
                    Attack(enemyCard);
                   
                }
                foreach (CardController enemyCard in enemyCards)
                {      
                    enemyCard.CheckAlive();
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                gameManager.AttackToHero(this);
                break;
            case SPELL.HEAL_FRIEND_CARD:
                if(target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard != model.isPlayerCard )
                {
                    return;
                }
                Heal(target);
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                foreach(CardController friendCard in friendCards)
                {
                    Heal(friendCard);
                }
                break;
            case SPELL.HEAL_FRIEND_HERO:
                gameManager.HealToHero(this);
                break;
            case SPELL.NONE:
                return;
            

        }
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);
        Destroy(this.gameObject);
    }
}

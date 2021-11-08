using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//　カードデータとその処理
public class CardModel 
{
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public ABILITY ability;
    public SPELL spell;

    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;
    public bool isPlayerCard;

    public CardModel(int cardId,bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card"+cardId);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        ability = cardEntity.ability;
        spell = cardEntity.spell;

        isAlive = true;
        isPlayerCard = isPlayer;

    }
    void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }
    //自分を回復する。
    void RecoveryHP(int point)
    {
        hp += point;
    }

    public void Attack(CardController card)
    {
        card.model.Damage(at);
    }
    // cardを回復させる
    public void Heal(CardController card)
    {
        card.model.RecoveryHP(at);
    }

}

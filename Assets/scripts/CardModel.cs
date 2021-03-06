using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータと処理
public class CardModel
{
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;
    public bool isPlayerCard;

    public ABILITY ability;
    public SPELL spell;

    public CardModel(int cardID,bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDataList/Card"+cardID);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        isAlive = true;
        isPlayerCard = isPlayer;

        ability = cardEntity.ability;
        spell = cardEntity.spell;
    }
    void Damage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    public void Attack(CardController card) 
    {
        card.model.Damage(at);
    }
}

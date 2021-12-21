using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    //見かけに関すること
    CardView View;

    //データ関すること
    public CardModel model;
    //移動
    public cardMovement movement;

    GameManager gameManager;

    private void Awake()
    {
        View = GetComponent<CardView>();
        movement = GetComponent<cardMovement>();
        gameManager = GameManager.instance;
    }

    public void Init(int cardID,bool isPlayer)
    {
        model = new CardModel(cardID,isPlayer);
        View.Show(model);
    }

    public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        SetCanAttack(false);
    }

    public void SetCanAttack(bool canAttack)
    {
        model.canAttack = canAttack;
        View.ActiveSelect(canAttack);
    }

    public void CheckAlive()
    {
        if(model.isAlive == false)
        {
            Destroy(this.gameObject);
        }
        else
        {
            View.Reflesh(model);
        }
    }
    public void OnField(bool isPlayer)
    {
        gameManager.ReduceManaCost(model.cost, true);

        model.isFieldCard = true;

        if(model.ability == ABILITY.INIT_ATTACKABLE)
        {
            SetCanAttack(true);
        }
    }
    public void UseSpellTo(CardController target)
    {
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                //特定の敵を攻撃
                Attack(target);
                target.CheckAlive();
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                //すべてのカードにダメージを与える
                CardController[] enemyCards = gameManager.GetEnemyFieldCard(this.model.isPlayerCard);
                foreach(CardController enemyCard in enemyCards)
                {
                    Attack(target);
                }
                foreach (CardController enemyCard in enemyCards)
                {
                    target.CheckAlive();
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                gameManager.AttackToHero(this);
                break;
            case SPELL.HEAL_FRIEND_CARD:
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                break;
            case SPELL.HEAL_FRIEND_HERO:
                break;
            case SPELL.NONE:
                return;
        }

        Destroy(this.gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    GameManager manager;
    private void Start()
    {
        manager = GameManager.instance;
    }
    public IEnumerator EnemyTurn()
    {
        //攻撃可能
        CardController[] enemyfieldList = manager.EnemyField.GetComponentsInChildren<CardController>();
        manager.SettingCanAttackView(enemyfieldList, true);

        yield return new WaitForSeconds(1);

        //場にカードを出す処理
        //手札のカードを取得
        CardController[] handList = manager.EnemyHand.GetComponentsInChildren<CardController>();

        //コスト以下のカードがあればフィールドに出し続ける
        while (Array.Exists(handList, card => card.model.cost < manager.enemy.manaCost))
        {
            //コスト以下のカードリストを取得
            CardController[] selectableHandCardList = Array.FindAll(handList, card => card.model.cost < manager.enemy.manaCost);
            //場に出す
            CardController enemycard = selectableHandCardList[0];
            //移動
            StartCoroutine(enemycard.movement.MoveToField(manager.EnemyField));
            enemycard.OnField(false);

            handList = manager.EnemyHand.GetComponentsInChildren<CardController>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        //攻撃
        //フィールドカードを取得(enemy)
        CardController[] EnemyfieldList = manager.EnemyField.GetComponentsInChildren<CardController>();
        //攻撃可能カードがあれば攻撃を繰り返す
        while (Array.Exists(EnemyfieldList, card => card.model.canAttack))
        {
            //攻撃可能カードを取得
            CardController[] enemyCanAttackCardist = Array.FindAll(EnemyfieldList, card => card.model.canAttack);//検索：FindAll
                                                                                                                 //フィールドカードを取得(player)
            CardController[] PlayerfieldList = manager.PlayerField.GetComponentsInChildren<CardController>();

            //攻撃カードを選択
            CardController attacker = enemyCanAttackCardist[0];
            if (PlayerfieldList.Length > 0)
            {
                //ディフェンスカードを選択
                //シールドカードのみ攻撃対象に移る
                if (Array.Exists(PlayerfieldList, card => card.model.ability == ABILITY.SHIELD))
                {
                    PlayerfieldList = Array.FindAll(PlayerfieldList, card => card.model.ability == ABILITY.SHIELD);
                }
                CardController defender = PlayerfieldList[0];
                //戦わせる
                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.51f);
                manager.CardBattle(attacker, defender);
            }
            else
            {
                StartCoroutine(attacker.movement.MoveToTarget(manager.PlayerHero.transform));
                yield return new WaitForSeconds(0.3f);
                manager.AttackToHero(attacker);
                yield return new WaitForSeconds(0.3f);
                manager.CheckHeroHp();
            }
            yield return new WaitForSeconds(1);
        }

        EnemyfieldList = manager.EnemyField.GetComponentsInChildren<CardController>();
        yield return new WaitForSeconds(1);

        manager.ChangeTurn();
    }
}

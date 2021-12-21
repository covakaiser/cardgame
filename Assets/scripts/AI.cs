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
        //�U���\
        CardController[] enemyfieldList = manager.EnemyField.GetComponentsInChildren<CardController>();
        manager.SettingCanAttackView(enemyfieldList, true);

        yield return new WaitForSeconds(1);

        //��ɃJ�[�h���o������
        //��D�̃J�[�h���擾
        CardController[] handList = manager.EnemyHand.GetComponentsInChildren<CardController>();

        //�R�X�g�ȉ��̃J�[�h������΃t�B�[���h�ɏo��������
        while (Array.Exists(handList, card => card.model.cost < manager.enemy.manaCost))
        {
            //�R�X�g�ȉ��̃J�[�h���X�g���擾
            CardController[] selectableHandCardList = Array.FindAll(handList, card => card.model.cost < manager.enemy.manaCost);
            //��ɏo��
            CardController enemycard = selectableHandCardList[0];
            //�ړ�
            StartCoroutine(enemycard.movement.MoveToField(manager.EnemyField));
            enemycard.OnField(false);

            handList = manager.EnemyHand.GetComponentsInChildren<CardController>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        //�U��
        //�t�B�[���h�J�[�h���擾(enemy)
        CardController[] EnemyfieldList = manager.EnemyField.GetComponentsInChildren<CardController>();
        //�U���\�J�[�h������΍U�����J��Ԃ�
        while (Array.Exists(EnemyfieldList, card => card.model.canAttack))
        {
            //�U���\�J�[�h���擾
            CardController[] enemyCanAttackCardist = Array.FindAll(EnemyfieldList, card => card.model.canAttack);//�����FFindAll
                                                                                                                 //�t�B�[���h�J�[�h���擾(player)
            CardController[] PlayerfieldList = manager.PlayerField.GetComponentsInChildren<CardController>();

            //�U���J�[�h��I��
            CardController attacker = enemyCanAttackCardist[0];
            if (PlayerfieldList.Length > 0)
            {
                //�f�B�t�F���X�J�[�h��I��
                //�V�[���h�J�[�h�̂ݍU���ΏۂɈڂ�
                if (Array.Exists(PlayerfieldList, card => card.model.ability == ABILITY.SHIELD))
                {
                    PlayerfieldList = Array.FindAll(PlayerfieldList, card => card.model.ability == ABILITY.SHIELD);
                }
                CardController defender = PlayerfieldList[0];
                //��킹��
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

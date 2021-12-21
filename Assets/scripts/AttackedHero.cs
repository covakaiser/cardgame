using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�U������鑤
public class AttackedHero : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //�U��
        //�U���J�[�h��I��
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();

        if (attacker == null)
        {
            return;
        }

        //�t�B�[���h�ɃV�[���h�L�����������ꍇ�A�v���C���[�ɍU���ł��Ȃ�
        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCard(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD))
        {
            return;
        }

        if (attacker.model.canAttack == true)
        {
            //Hero�ɍU������
            GameManager.instance.AttackToHero(attacker);
        }
    }
}

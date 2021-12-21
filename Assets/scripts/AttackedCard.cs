using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�U������鑤
public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //�U��
        //�U���J�[�h��I��
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        //�f�B�t�F���X�J�[�h��I��
        CardController defender = GetComponent<CardController>();

        if (attacker == null || defender == null)
        {
            return;
        }

        //�t�B�[���h�ɃV�[���h�L�����������ꍇ�A���̃J�[�h�ɂɍU���ł��Ȃ�
        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCard(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD 
            && defender.model.ability != ABILITY.SHIELD))
        {
            return;
        }

        if (attacker.model.isPlayerCard == defender.model.isPlayerCard)
        {
            return;
        }

        if(attacker.model.canAttack == true)
        {
            //��킹��
            GameManager.instance.CardBattle(attacker, defender);
            GameManager.instance.CheckHeroHp();
        }
    }
}

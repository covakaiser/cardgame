
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�U������鑤
public class SpellDropManager : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //�U��
        //�U���J�[�h��I��
        CardController spellCard = eventData.pointerDrag.GetComponent<CardController>();
        //�f�B�t�F���X�J�[�h��I��
        CardController target = GetComponent<CardController>();

        if (spellCard == null)
        {
            return;
        }
        spellCard.UseSpellTo(target);
    }
}


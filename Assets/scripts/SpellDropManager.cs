
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃される側
public class SpellDropManager : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //攻撃
        //攻撃カードを選択
        CardController spellCard = eventData.pointerDrag.GetComponent<CardController>();
        //ディフェンスカードを選択
        CardController target = GetComponent<CardController>();

        if (spellCard == null)
        {
            return;
        }
        spellCard.UseSpellTo(target);
    }
}


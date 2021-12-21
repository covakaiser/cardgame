using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropField : MonoBehaviour, IDropHandler
{

    public TYPE type;
    public enum TYPE
    {
        HAND,
        FIELD
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(type == TYPE.HAND)
        {
            return;
        }
        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if(card != null)
        {
            if (!card.movement.isDraggable)
            {
                return;
            }
            card.movement.defaultParent = this.transform;
            if(card.model.isFieldCard)
            {
                return;
            }
            card.OnField(true);

        }
    }
}

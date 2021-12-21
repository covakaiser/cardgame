using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class cardMovement : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Transform defaultParent;

    public bool isDraggable;



    //�h���b�O�J�n���̏���
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�J�[�h�̃R�X�g�ƃv���C���[�̃}�i�R�X�g�̔�r
        CardController card = GetComponent<CardController>();
        if(card.model.isPlayerCard && GameManager.instance.isPlayerTurn && !card.model.isFieldCard && card.model.cost <= GameManager.instance.player.manaCost)
        {
            isDraggable = true;
        }
        else if(card.model.isPlayerCard && GameManager.instance.isPlayerTurn && card.model.isFieldCard && card.model.canAttack)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }
        if (!isDraggable)
        {
            return;
        }
        defaultParent = transform.parent;
        //���̏ꏊ�ɖ߂�悤�ɂȂ�
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    //�h���b�O���̏���

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        transform.position = eventData.position;
    }
    //�h���b�O�I�����̏���
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public IEnumerator MoveToField(Transform field)
    {
        //��x�e��canvas�ɕύX����
        transform.SetParent(defaultParent.parent);
        //DOTween�ŃJ�[�h���t�B�[���h�Ɉړ�
        transform.DOMove(field.position,0.25f);

        yield return new WaitForSeconds(0.5f);

        defaultParent = field;
        transform.SetParent(defaultParent);
    }

    public IEnumerator MoveToTarget(Transform target)
    {
        //���݂̈ʒu�ƕ��т��擾
        Vector2 currentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();
        //��x�e��canvas�ɕύX����
        transform.SetParent(defaultParent.parent);
        //DOTween�ŃJ�[�h��target�Ɉړ�
        transform.DOMove(target.position, 0.25f);

        yield return new WaitForSeconds(0.25f);
        //���̈ʒu�ɖ߂�
        transform.DOMove(currentPosition, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.SetParent(defaultParent);
        transform.SetSiblingIndex(siblingIndex);
    }

    void Start()
    {
        defaultParent = transform.parent;
    }
}

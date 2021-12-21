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



    //ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        //カードのコストとプレイヤーのマナコストの比較
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
        //元の場所に戻るようになる
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    //ドラッグ中の処理

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        transform.position = eventData.position;
    }
    //ドラッグ終了時の処理
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
        //一度親をcanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをフィールドに移動
        transform.DOMove(field.position,0.25f);

        yield return new WaitForSeconds(0.5f);

        defaultParent = field;
        transform.SetParent(defaultParent);
    }

    public IEnumerator MoveToTarget(Transform target)
    {
        //現在の位置と並びを取得
        Vector2 currentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();
        //一度親をcanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをtargetに移動
        transform.DOMove(target.position, 0.25f);

        yield return new WaitForSeconds(0.25f);
        //元の位置に戻る
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

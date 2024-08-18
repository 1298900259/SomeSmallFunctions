using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonoDragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField]
    private Transform mCorrectPutParent;//正确放置的父物体
    private Transform mParent;//初始父物体
    private Transform mPutParent;//放置父物体
    private Vector2 mRecordPos;//记录初始的位置
    private Vector2 mOffset;

    private void Awake()
    {
        mParent = transform.parent;
        mRecordPos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(mParent.parent);
        mOffset = (Vector2)transform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + mOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var collider2D = Physics2D.OverlapBox(transform.position, GetBoxSize(), 0);
        if (collider2D)
        {
            MonoDragItem mono = collider2D.GetComponentInChildren<MonoDragItem>();
            if (mono)
            {
                mono.SetParAndPos(mPutParent);
            }
            SetParAndPos(collider2D.transform);
        }
        else
        {
            OnReset();
        }
    }

    public void SetParAndPos(Transform parent)
    {
        if (parent == null)
        {
            OnReset();
        }
        else
        {
            mPutParent = parent;
            transform.SetParent(parent);
            transform.position = parent.position;
        }
    }

    public void OnReset()
    {
        transform.SetParent(mParent);
        transform.localPosition = mRecordPos;
        mPutParent = null;
    }

    public bool GetIsRight()
    {
        return mPutParent == mCorrectPutParent;
    }

    private Vector2 GetBoxSize()
    {
        Vector3[] fourCorner = new Vector3[4];
        transform.GetComponent<RectTransform>().GetWorldCorners(fourCorner);
        Vector2 rectRange = new Vector2(Mathf.Abs(fourCorner[2].x - fourCorner[0].x), Mathf.Abs(fourCorner[2].y - fourCorner[0].y));
        return rectRange;
    }
}

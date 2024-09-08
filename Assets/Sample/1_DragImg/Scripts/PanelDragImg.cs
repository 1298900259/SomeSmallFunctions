using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片拖拽面板
/// </summary>
public class PanelDragImg : MonoBehaviour
{
    Button m_Btn_Confirm;
    MonoDragItem[] mDragItems;
    Action<bool> onDragOver;

    private void Awake()
    {
        m_Btn_Confirm = transform.Find("Frame/Btn_Confirm").GetComponent<Button>();
        mDragItems = transform.GetComponentsInChildren<MonoDragItem>();

        m_Btn_Confirm.onClick.AddListener(OnClickBtnConfirm);

        //使用时外部调用这个
        Show((isRight) => { Debug.Log(isRight); });
    }

    public void Show(Action<bool> onDragOver = null)
    {
        OnReset();
        SetOnDragOver(onDragOver);
    }

    private void SetOnDragOver(Action<bool> onAc = null)
    {
        onDragOver = onAc;
    }

    private void OnReset()
    {
        foreach (var item in mDragItems)
        {
            item.OnReset();
        }
    }

    private bool IsRight()
    {
        foreach (var item in mDragItems)
        {
            if (!item.GetIsRight())
            {
                return false;
            }
        }
        return true;
    }

    private void OnClickBtnConfirm()
    {
        onDragOver?.Invoke(IsRight());
    }
}

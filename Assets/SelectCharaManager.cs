/**********************************************************
 *
 *  SelectCharaManager.cs
 *  選択キャラの管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/05
 *
 *********************************************************/
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択キャラの管理
/// </summary>
public class SelectCharaManager : MonoBehaviour
{
    // オブジェクト
    [SerializeField]
    private List<GameObject> m_charaObjects;

    // 現在のキャラ
    private GameObject m_current;

    /*--------------------------------------------------------------------------------
     || キャラ変更処理
     --------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラ変更処理
    /// </summary>
    /// <param name="index">キャラのIndex</param>
    public void ChangeChara(int index)
    {
        // 前のキャラを解除
        if (m_current != null)
        {
            m_current.gameObject.SetActive(false);
        }

        // 新しいキャラ
        m_current = m_charaObjects[index];
        m_current.gameObject.SetActive(true);
        // キャラ変更SE
        SoundManager.Instance.PlaySE("Slide");
    }

    /*--------------------------------------------------------------------------------
     || キャラ決定処理
     --------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラ決定処理
    /// </summary>
    public void DecideChara()
    {
        m_current.GetComponent<SelectCharaController>().Decide();
        SoundManager.Instance.PlaySE("Decide");
    }

    /*--------------------------------------------------------------------------------
     || キャラキャンセル処理
     --------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラキャンセル処理
    /// </summary>
    public void CancelChara()
    {
        m_current.GetComponent<SelectCharaController>().Cancel();
        SoundManager.Instance.PlaySE("Cancel");
    }
}
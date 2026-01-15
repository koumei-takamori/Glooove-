/**********************************************************
 *
 *  SelectCharaManager.cs
 *  選択キャラの管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/05
 *
 *********************************************************/
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択キャラの管理
/// </summary>
public class SelectCharaManager : MonoBehaviour
{
    // カメラ
    [SerializeField]
    private SelectCharaCamera m_cameraController;

    // オブジェクト
    [SerializeField]
    private List<GameObject> m_charaObjects;

    // カメラの基準位置
    private Vector3 m_cameraPos;

    // 現在のキャラ
    private GameObject m_current;
    private int m_currentIndex;

    /*--------------------------------------------------------------------------------
     || 実行前初期化処理
     --------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        // カメラの初期位置を記憶
        m_cameraPos = m_cameraController.transform.position;
    }

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
        m_currentIndex = index;
        m_current = m_charaObjects[m_currentIndex];
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
    public void DecideChara(int index)
    {
        // 前のキャラを解除
        if (m_current != null)
        {
            m_current.gameObject.SetActive(false);
        }

        // 新しいキャラ
        m_currentIndex = index;
        m_current = m_charaObjects[m_currentIndex];
        m_current.gameObject.SetActive(true);
    
        m_current.GetComponent<SelectCharaController>().Decide();
        m_cameraController.MoveToTargetX(m_current.GetComponent<SelectCharaController>().Target.position);
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
        m_cameraController.MoveToTargetX(m_cameraPos);
        m_current.GetComponent<SelectCharaController>().Cancel();
        SoundManager.Instance.PlaySE("Cancel");
    }
}
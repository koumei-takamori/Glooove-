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
    // プレイヤー
    private SelectPlayer m_player;

    // 各キャラのオブジェクト
    [SerializeField]
    private List<GameObject> m_charaObjects = new List<GameObject>();

    // 操作可能フラグ
    private bool m_canControll = false;

    // プロパティ
    public bool CanControll { get { return m_canControll; } set { m_canControll = value; } }

    /*--------------------------------------------------------------------------------
    || 初期化処理
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // Join前（Player未Bind）の場合は全非表示
        SetAllCharaInactive();
    }

    /*--------------------------------------------------------------------------------
    || 更新処理
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        UpdateCharaView();
    }

    /*--------------------------------------------------------------------------------
    || UIとPlayerを結び付ける
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// UIとPlayerを結び付ける
    /// </summary>
    public void Bind(SelectPlayer player)
    {
        m_player = player;
        m_canControll = true;

        UpdateCharaView();
    }

    /*--------------------------------------------------------------------------------
    || キャラ表示更新
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 選択中キャラのみを表示
    /// </summary>
    public void UpdateCharaView()
    {
        if (m_player == null) return;

        int index = (int)m_player.CharaIndex;

        for (int i = 0; i < m_charaObjects.Count; i++)
        {
            m_charaObjects[i].SetActive(i == index);
        }
    }

    /*--------------------------------------------------------------------------------
    || 全キャラ非表示
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 全キャラ非表示 
    /// </summary>
    private void SetAllCharaInactive()
    {
        for (int i = 0; i < m_charaObjects.Count; i++)
        {
            m_charaObjects[i].SetActive(false);
        }
    }
}
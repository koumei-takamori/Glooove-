/**********************************************************
 *
 *  CharacterCursorUI.cs
 *  キャラ選択カーソルUI
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// キャラ選択カーソルUI
/// </summary>
public class CharacterCursorUI : MonoBehaviour
{
    // セレクトのプレイヤー
    [SerializeField] 
    private SelectPlayer m_player;

    // カーソル
    [SerializeField] 
    private RectTransform m_cursor;

    // キャラアイコンの位置
    [SerializeField] 
    private RectTransform[] m_charaIcons;

    // 選択中のキャラモデルの制御
    SelectCharaController m_charaController;　

    // 操作可能フラグ
    private bool m_canControll = false;

    // プロパティ
    public　SelectPlayer　Player { get { return m_player; } }
    public　bool　CanControll { get { return m_canControll; } set { m_canControll = value; } }　

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // 操作不能なら処理しない
        if(!m_canControll) { return; }

        // キャラのindexを取得
        int index = m_player.CharaIndex;

        // カーソルを対象アイコンの位置へ
        m_cursor.position = m_charaIcons[index].position;
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
    }
}
/**********************************************************
 *
 *  CharacterCursorUI.cs
 *  キャラ選択カーソルUI
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
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

    [Header("キャラアイコン")]
    [SerializeField] 
    private RectTransform[] m_charaIcons;

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // キャラのindexを取得
        int index = m_player.GetCharaIndex();

        // カーソルを対象アイコンの位置へ
        m_cursor.position = m_charaIcons[index].position;
    }
}
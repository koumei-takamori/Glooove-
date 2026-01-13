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
    // カーソル
    [SerializeField] 
    private UIElement m_cursor;

    // キャラアイコンの位置
    [SerializeField] 
    private RectTransform[] m_charaIcons;

    /*--------------------------------------------------------------------------------
　　|| 有効化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// アクティブ化処理
    /// </summary>
    /// <param name="index"></param>
    public void Active()
    {
        // カーソルを対象アイコンの位置へ
        m_cursor.gameObject.SetActive(true);
    }

    /*--------------------------------------------------------------------------------
　　|| 移動処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="index"></param>
    public void MoveCharaCursor(int index) 
    {
        // カーソルを対象アイコンの位置へ
        m_cursor.Rect.position = m_charaIcons[index].position;
    }

    /*--------------------------------------------------------------------------------
　　|| 決定処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 決定処理
    /// </summary>
    /// <param name="index"></param>
    public void DecideCharaCursor()
    {
        m_cursor.Animator.SetBool("Decide", true);
    }

    /*--------------------------------------------------------------------------------
　　|| キャンセル処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 決定処理
    /// </summary>
    /// <param name="index"></param>
    public void CancelCharaCursor()
    {
        m_cursor.Animator.SetBool("Decide", false);
    }
}
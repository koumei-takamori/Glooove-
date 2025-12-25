/**********************************************************
 *
 *  PlayerGloveData.cs
 *  プレイヤーが使うグローブ情報
 *  ScriptableObjectに情報を格納
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/08/06
 *
 *********************************************************/
using UnityEngine;

/// <summary>
/// プレイヤーが使うグローブ情報
/// </summary>
[System.Serializable]
public class PlayerGloveData
{
    // 左のグローブ
    [SerializeField]
    private GameObject m_leftGlove;

    // 右のグローブ
    [SerializeField]
    private GameObject m_rightGlove;

    /*--------------------------------------------------------------------------------
　　|| コンストラクタ
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="left">左のグローブ</param>
    /// <param name="right">右のグローブ</param>
    public PlayerGloveData(GameObject left, GameObject right)
    {
        this.m_leftGlove = left;
        this.m_rightGlove = right;
    }

    // プロパティ
    public GameObject LeftGlove { get { return m_leftGlove; } set { m_leftGlove = value; } }

    public GameObject RightGlove { get { return m_rightGlove; } set { m_rightGlove = value; } }
}

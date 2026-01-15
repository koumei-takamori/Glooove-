/**********************************************************
 *
 *  SelectCharaController.cs
 *  選択キャラ制御
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using UnityEngine;

/// <summary>
/// 選択キャラ制御
/// </summary>
[RequireComponent(typeof(Animator))]
public class SelectCharaController : MonoBehaviour
{
    // アニメーター
    private Animator m_animator;

    [SerializeField]
    private Transform m_target;

    public Transform Target
    {
        get { return m_target; }
    }

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }

    /*--------------------------------------------------------------------------------
　　|| キャラ決定処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラ決定処理
    /// </summary>
    public void Decide()
    {
        m_animator.SetBool("Select",true);
    }

    /*--------------------------------------------------------------------------------
　　|| キャラキャンセル処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラキャンセル処理
    /// </summary>
    public void Cancel()
    {
        m_animator.SetBool("Select", false);
    }
}

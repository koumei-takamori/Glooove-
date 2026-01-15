using UnityEngine;

/// <summary>
/// ステージセレクトUI制御
/// 左右入力でステージを切り替え、Inspectorで設定したAnimatorを再生する
/// </summary>
public class StageSelectUI : MonoBehaviour
{
    // ================================
    // Inspector設定
    // ================================

    [Header("Stage Animators (Left → Right)")]
    [SerializeField]
    private Animator[] m_stageAnimators;

    // ================================
    // 内部変数
    // ================================

    // 現在選択中のステージindex
    private int m_currentIndex = 0;

    // ステージ数
    private int m_stageCount = 0;

    // ================================
    // Unity Events
    // ================================

    private void Start()
    {
        // ステージ数取得
        m_stageCount = m_stageAnimators.Length;

        if (m_stageCount == 0)
        {
            Debug.LogError("StageSelectUI : StageAnimator が設定されていません");
            enabled = false;
            return;
        }

        // 初期ステージを選択状態に
        PlayInAnimation(m_currentIndex);
    }

    // ================================
    // ステージ切り替え
    // ================================

    public void ChangeStage(int direction)
    {
        int prevIndex = m_currentIndex;
        m_currentIndex = GetNextIndex(m_currentIndex, direction);

        // 同じなら何もしない
        if (prevIndex == m_currentIndex) return;

        // アニメーション切り替え
        PlayOutAnimation(prevIndex);
        PlayInAnimation(m_currentIndex);
    }

    /// <summary>
    /// ループ込みで次のindexを取得
    /// </summary>
    private int GetNextIndex(int current, int direction)
    {
        return (current + direction + m_stageCount) % m_stageCount;
    }

    // ================================
    // アニメーション制御
    // ================================

    private void PlayInAnimation(int index)
    {
        Animator anim = m_stageAnimators[index];
        if (anim != null)
        {
            anim.Play("StageSelect_in");
        }
    }

    private void PlayOutAnimation(int index)
    {
        Animator anim = m_stageAnimators[index];
        if (anim != null)
        {
            anim.Play("StageSelect_out");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 腕の末端（最も深いボーン）に手のTransformを追従させるスクリプト
/// </summary>
public class GloveFollow : MonoBehaviour
{
    [SerializeField] private Transform armRoot;       ///< 腕のルートボーン（バネの根元）
    [SerializeField] private Transform handTransform; ///< 手のTransform

    private Transform tip; ///< 腕の末端ボーン（自動取得される）

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        if (armRoot == null)
        {
            Debug.LogError("armRoot が設定されていません。");
            return;
        }

        // 腕のルート以下で一番深い階層のボーンを取得
        tip = GetDeepestChild(armRoot);

        if (tip == null)
        {
            Debug.LogError("腕の末端ボーンが見つかりません。");
        }
    }

    /// <summary>
    /// 指定されたTransform以下で、最も深い階層の子Transformを取得する
    /// </summary>
    Transform GetDeepestChild(Transform root)
    {
        // 子がいなければ自分自身が末端
        if (root.childCount == 0)
            return root;

        Transform deepest = null;
        int maxDepth = -1;

        foreach (Transform child in root)
        {
            // 再帰的に探索
            Transform target = GetDeepestChild(child);

            // 深さを計算
            int depth = GetDepth(root, target);

            if (depth > maxDepth)
            {
                maxDepth = depth;
                deepest = target;
            }
        }

        return deepest;
    }

    /// <summary>
    /// rootからtargetまでの階層の深さを求める
    /// </summary>
    int GetDepth(Transform root, Transform target)
    {
        int depth = 0;
        Transform current = target;

        while (current != null && current != root)
        {
            current = current.parent;
            depth++;
        }

        return depth;
    }

    /// <summary>
    /// 手のTransformを末端ボーンに追従させる
    /// </summary>
    void LateUpdate()
    {
        if (tip == null || handTransform == null)
            return;

        // 末端ボーンの位置と回転に追従
        handTransform.position = tip.position;
        handTransform.rotation = tip.rotation;
    }
}

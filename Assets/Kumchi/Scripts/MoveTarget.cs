using System.Collections;
using UnityEngine;

/// <summary>
/// ターゲットを一定間隔でランダムに動かすユーティリティ。
/// Inspector で範囲・待機時間・移動時間・イージングカーブなどを調整できます。
/// StartMoving()/StopMoving() を外部から呼んで制御できます。
/// </summary>
[DisallowMultipleComponent]
public class MoveTarget : MonoBehaviour
{

    [Header("移動範囲 (中心は areaCenter、サイズは areaSize)")]
    [Tooltip("範囲中心（ワールド座標）。useAreaLocal=true のときは Transform のローカル空間基準になります。")]
    public Vector3 areaCenter = Vector3.zero;
    [Tooltip("範囲サイズ（X,Y,Z）。X/Z が水平範囲、Y が高さ範囲です。")]
    public Vector3 areaSize = new Vector3(6f, 0f, 6f);
    [Tooltip("areaCenter/areaSize をこのオブジェクトのローカル空間で扱うか（true）／ワールド空間で扱うか（false）。")]
    public bool useAreaLocal = true;

    [Header("タイミング")]
    [Tooltip("移動開始前の待機時間の最小値（秒）。")]
    public float minWait = 0.5f;
    [Tooltip("移動開始前の待機時間の最大値（秒）。")]
    public float maxWait = 2.0f;
    [Tooltip("各移動アクションの所要時間（秒）。0 の場合は瞬間移動になります。")]
    public float moveDuration = 1.0f;
    [Tooltip("移動のイージングカーブ（0->1 の t に対して掛ける）。")]
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("オプション")]
    [Tooltip("開始時に自動で動き始めるかどうか。")]
    public bool startOnAwake = true;
    [Tooltip("繰り返し続けるかどうか。false の場合は一回だけ移動します（1 回目の wait の後）。")]
    public bool loop = true;
    [Tooltip("高さ（Y）もランダムにするか。false の場合 Y は現在の Y を使う。")]
    public bool randomizeY = false;
    [Tooltip("randomizeY=true のときの Y の最小値（ワールド座標）。")]
    public float minY = 0f;
    [Tooltip("randomizeY=true のときの Y の最大値（ワールド座標）。")]
    public float maxY = 2f;

    // 内部
    Coroutine moveCoroutine;

    void Start()
    {
        if (startOnAwake) StartMoving();
    }


    /// <summary>動作を開始します（既に動作中なら無視）。</summary>
    public void StartMoving()
    {
        if (moveCoroutine == null)
            moveCoroutine = StartCoroutine(MoveLoop());
    }

    /// <summary>動作を停止します（途中の移動はキャンセルされます）。</summary>
    public void StopMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    IEnumerator MoveLoop()
    {
        // 最初は少し待ってから動かしたい場合があるので待ちを入れる（ランダム）
        while (true)
        {
            float wait = Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(wait);

            // 目標点を決める
            Vector3 targetWorld = PickRandomPointInArea();

            if (moveDuration <= 0f)
            {
                transform.position = targetWorld;
            }
            else
            {
                Vector3 from = transform.position;
                float elapsed = 0f;
                float dur = Mathf.Max(0.0001f, moveDuration);
                while (elapsed < dur)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / dur);
                    float k = moveCurve.Evaluate(t);
                    transform.position = Vector3.LerpUnclamped(from, targetWorld, k);
                    yield return null;
                }
                transform.position = targetWorld;
            }

            if (!loop) break;
        }

        moveCoroutine = null;
    }

    Vector3 PickRandomPointInArea()
    {
        // ランダムポイントを areaCenter ± areaSize/2 の範囲で取る（local or world）
        Vector3 localOffset = new Vector3(
            Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
            Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f),
            Random.Range(-areaSize.z * 0.5f, areaSize.z * 0.5f)
        );

        Vector3 worldPoint;
        if (useAreaLocal)
        {
            worldPoint = transform.TransformPoint(areaCenter + localOffset);
        }
        else
        {
            worldPoint = areaCenter + localOffset;
        }

        if (!randomizeY)
        {
            worldPoint.y = transform.position.y;
        }
        else
        {
            worldPoint.y = Random.Range(minY, maxY);
        }

        return worldPoint;
    }

#if UNITY_EDITOR
    // Scene 上に範囲を可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.1f, 0.7f, 0.1f, 0.2f);
        Vector3 center = useAreaLocal ? transform.TransformPoint(areaCenter) : areaCenter;
        Gizmos.DrawCube(center, areaSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, areaSize);
    }
#endif
}
// ------------------------------------------------
//
// 名前；GloveData.cs
// 内容：Gloveのデータを格納するクラス
// 格納：カーブ挙動　攻撃力 どちらのプレイヤーに付いているか
// 作者：池田桜輔
// 日付：2025/12/12
// ------------------------------------------------

using UnityEngine;

public class GloveObject : MonoBehaviour
{
    // ------------------------------
    // 変数
    // ------------------------------

    // パラメーター
    [SerializeField] private GloveData parameterData;

    // グローブの回転
    [SerializeField]
    private Vector3 m_gloveRotation = Vector3.zero;

    // 攻撃中かのフラグ
    [SerializeField] private bool isAttacking = false;

    // 所有者プレイヤー
    [SerializeField] private GameObject owner;

    // ------------------------------
    // アクセサ
    // ------------------------------
    public GloveData ParameterData
    {
        get { return parameterData; }
        set { parameterData = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public Quaternion GloveRotation
    { get { return Quaternion.Euler(m_gloveRotation); } }



    // ------------------------------
    // Mono関数
    // ------------------------------

    /// <summary>
    /// オブジェクトが当たった瞬間の処理
    /// </summary>
    /// <param name="collision">ヒット相手の衝突判定データ</param>
    private void OnTriggerEnter(Collider collider)
    {
        // 攻撃中でなければ処理なし
        if (!isAttacking) return;

        // 相手プレイヤーかを判別
        if (collider.gameObject.CompareTag("Player") && collider.gameObject != owner)
        {

            // オーナーからダメージ量を実数で受け取る

            // グローブのダメージ倍率を計算


            // ダメージを与える
            collider.gameObject.GetComponent<PlayerHP>().Damaged(10);

            // フラグをリセット
            isAttacking = false;

            // 効果音を再生
        }
    }


    // ------------------------------
    // 関数
    // ------------------------------

    public void Initialize(GameObject owner)
    {
        this.owner = owner;

        NullCheck();
    }

    /// <summary>
    /// 登録されたデータのヌルチェック
    /// </summary>
    void NullCheck()
    {
        // ぬるちぇっく
        if (parameterData == null)
        {
            Debug.LogError("GloveObjectData: カーブデータが登録されていません" + gameObject);
        }

        if (owner == null)
        {
            Debug.LogError("GloveObjectData: 所有者(Owner)が存在しません" + gameObject);
        }
    }
}
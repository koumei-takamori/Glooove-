// ------------------------------------------------
//
// 名前；GloveData.cs
// 内容：Gloveのデータを格納するクラス
// 格納：カーブ挙動　攻撃力 どちらのプレイヤーに付いているか
// 作者：池田桜輔
// 日付：2025/12/12
// ------------------------------------------------

using Nakashi.Player;
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
    // どの攻撃を行っているのか
    [SerializeField] private GloveActionType currentAttackType;

    // 所有者プレイヤー
    [SerializeField] private GameObject owner;

    // 所有者プレイヤーデータ
    [SerializeField] private ArmPlayerData armPlayerData;

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

    public void OnPlayerAttack(GloveActionType type = GloveActionType.NORMAL_ATTACK)
    {
        isAttacking = true;
        currentAttackType = type;
    }

    public void OnPlayerAttackEnd()
    {
        isAttacking = false;
        currentAttackType = GloveActionType.NONE;
    }


    public Quaternion GloveRotation
    { get { return Quaternion.Euler(m_gloveRotation); } }

    public void RegisterArmPlayerData(ArmPlayerData data)
    {
        // null が渡されてきた場合はエラーを出して処理を中断
        if (data == null)
        {
            Debug.LogError("Gloveに登録されようとしたArmPlayerDataがnullです。");
            return;
        }

        // 正常なデータなので保持する
        armPlayerData = data;
    }

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
            // プレイヤーのデータ初回取得
            if (armPlayerData == null)
            {
                Debug.LogError("GloveObject: ArmPlayerDataが登録されていません。" + gameObject);
                return;
            }

            // ダメージ量を実数で受け取る
            int damage = (int)armPlayerData.GetAttackPower();


            // グローブが持つダメージ倍率を取得
            float multiplier = parameterData.GetAttackMultiplierByType(currentAttackType);


            // ヒット相手がパリィを行っているかを確認する必要がある
            ArmPlayerController enemyController = collider.GetComponent<ArmPlayerController>();
            bool isParry = enemyController.GetPlayerStatus().GetSetParry;

            // グローブのダメージ倍率を計算
            damage = (int)(damage * multiplier);

            if (isParry)
            {
                damage = (int)(damage * 0.1f);
                // ガード成功時の効果音再生

            }

            // ダメージを与える
            collider.gameObject.GetComponent<PlayerHP>().Damaged(damage);

            // フラグをリセット
            isAttacking = false;

            // ダメージを食らったときの効果音を再生
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
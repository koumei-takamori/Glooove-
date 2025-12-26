/**********************************************************
 *
 *  GloveData.cs
 *  グローブデータ
 *  ScriptableObjectに情報を格納
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/08/06
 *  
 *  変更　: 池田
 *  日付　: 2025/12/14
 *  内容　: 曲線挙動も保存できるよう追加
 *
 *********************************************************/
using System;
using UnityEngine;


/*--------------------------------------------------------------------------------
|| グローブ種類を識別するID
--------------------------------------------------------------------------------*/
/// <summary>
/// グローブ種類を識別するID
/// </summary>
public enum GloveID : int
{
    NORMAL = 0, // 通常グローブ
    BIG = 1  // ビッググローブ
}

/*--------------------------------------------------------------------------------
|| 攻撃の種類
--------------------------------------------------------------------------------*/
/// <summary>
/// 攻撃の種類
/// </summary>
public enum GloveActionType
{
    NORMAL_ATTACK,　 // 通常攻撃
    SPECIAL_ATTACK,  // 強化攻撃
    GRAP             // つかみ
}

public enum GloveWeight
{
    LIGHT,      // 軽い
    MEDIUM,     // 普通
    HEAVY       // 重い
}

/*--------------------------------------------------------------------------------
|| StretchArm 用パラメータ
--------------------------------------------------------------------------------*/
/// <summary>
/// StretchArm の再生に必要な「挙動のみ」をまとめたパラメータ
/// ※ Transform や Scene 依存情報は一切持たない
/// </summary>
[Serializable]
public class StretchArmParams
{
    [Header("伸縮速度")]
    [SerializeField]
    private float m_extendSpeed = 6f;
    [SerializeField]
    private float m_retractSpeed = 3f;

    [Header("挙動パラメータ")]
    [SerializeField]
    private float m_coilFrequency = 0f;
    [SerializeField]
    private float m_coilAmplitude = 0f;
    [SerializeField]
    private float m_hitWaitTime = 0.5f;

    [Header("ゆらぎ")]
    [SerializeField]
    private float m_swayAmplitude = 0.3f;
    [SerializeField]
    private float m_swaySpeed = 2.0f;


    [Header("共通")]
    [SerializeField]
    private float m_maxDistance = 6f;

    [Header("動作オプション")]
    [SerializeField]
    private bool m_usePlayerForward = true;
    [SerializeField]
    private float m_forwardForceDistance = 100f;

    [Header("曲線基準")]
    [SerializeField]
    private bool m_useWorldForwardAsReference = true;

    // -------- accessor --------
    public float ExtendSpeed => m_extendSpeed;
    public float RetractSpeed => m_retractSpeed;
    public float CoilFrequency => m_coilFrequency;
    public float CoilAmplitude => m_coilAmplitude;
    public float HitWaitTime => m_hitWaitTime;
    public float SwayAmplitude => m_swayAmplitude;
    public float SwaySpeed => m_swaySpeed;
    public float MaxDistance => m_maxDistance;
    public bool UsePlayerForward => m_usePlayerForward;
    public float ForwardForceDistance => m_forwardForceDistance;
    public bool UseWorldForwardAsReference => m_useWorldForwardAsReference;
}


/*--------------------------------------------------------------------------------
|| アクション用パラメーター
--------------------------------------------------------------------------------*/
/// <summary>
/// アクション用パラメーター
/// </summary> 
[Serializable]
public class GloveActionParams
{
    [Header("--------------------------")]
    // 攻撃の識別
    [Header("識別"), SerializeField]
    private GloveActionType m_type = GloveActionType.NORMAL_ATTACK;

    // 基礎パラメーター
    [Header("基礎")]
    // アームの挙動
    [SerializeField]
    private StretchArmParams m_stretchArmParams = new StretchArmParams();
    // 重さの種類
    [SerializeField]
    private GloveWeight gloveWeight = GloveWeight.MEDIUM;
    // クールタイム
    [SerializeField]
    private float m_coolTime = 0.5f;
    // 攻撃の倍率
    [SerializeField]
    private float m_attackMultiplier = 1.2f;

    // プロパティ
    public GloveActionType Type { get { return m_type; } private set { m_type = value; } }
    public StretchArmParams StretchArmParams { get { return m_stretchArmParams; } }
    public float CoolTime { get { return m_coolTime; } }
    public float AttackMultiplier { get { return m_attackMultiplier; } }

    /*--------------------------------------------------------------------------------
    || コンストラクタ
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="type">グローブアクション種類</param>
    public GloveActionParams(GloveActionType type)
    {
        Type = type;
    }

}

/*--------------------------------------------------------------------------------
|| 攻撃用パラメーター
--------------------------------------------------------------------------------*/
/// <summary>
/// 攻撃用パラメーター
/// </summary> 
[Serializable]
public class GloveAttackParams : GloveActionParams
{
    // コンストラクタ
    public GloveAttackParams(GloveActionType type) : base(type)
    {
    }
}

/*--------------------------------------------------------------------------------
|| 強化攻撃用パラメーター
--------------------------------------------------------------------------------*/
/// <summary>
/// 強化攻撃用パラメーター
/// </summary>
[Serializable]
public class SpecialAttackParams : GloveActionParams
{
    [Header("強化攻撃用")]
    // 消費するゲージ量
    private float m_gaugeCost;

    /*--------------------------------------------------------------------------------
    || コンストラクタ
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="type">グローブアクション種類</param>
    public SpecialAttackParams(GloveActionType type) : base(type)
    {
    }

    // プロパティ
    public float GaugeCost { get { return m_gaugeCost; } }
}

/*--------------------------------------------------------------------------------
|| グローブ種類を識別するID
--------------------------------------------------------------------------------*/
/// <summary>
/// つかみ用のパラメーター
/// </summary>
[Serializable]
public class GrapParams : GloveActionParams
{
    [Header("つかみ用")]
    // つかみの範囲
    private float m_grapRenge;


    /*--------------------------------------------------------------------------------
    || コンストラクタ
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="type">グローブアクション種類</param>
    public GrapParams(GloveActionType type) : base(type)
    {
    }

    // プロパティ
    public float GrapRenge { get { return m_grapRenge; } }
}

/*--------------------------------------------------------------------------------
|| グローブ種類を識別するID
--------------------------------------------------------------------------------*/
/// <summary> 
/// グローブデータ
/// </summary> 
[CreateAssetMenu(menuName = "Glove/GloveData", fileName = "GloveData")]
[System.Serializable]
public class GloveData : ScriptableObject
{
    [Header("グローブ情報")]
    // グローブの名前
    [SerializeField]
    private string m_gloveName = "Glove";
    // グローブの種類
    [SerializeField]
    private GloveID m_gloveID = GloveID.NORMAL;
    // 曲線挙動
    [SerializeField]
    private BezierCurveData m_curveData;


    [Header("攻撃の種類別データ")]
    // 通常攻撃
    [SerializeField]
    private GloveAttackParams m_normalAttackParam = new GloveAttackParams(GloveActionType.NORMAL_ATTACK);
    // 強化攻撃
    [SerializeField]
    private SpecialAttackParams m_specialAttackParam = new SpecialAttackParams(GloveActionType.SPECIAL_ATTACK);
    // つかみ
    [SerializeField]
    private GrapParams m_grabParam = new GrapParams(GloveActionType.GRAP);

    // プロパティ
    public string GloveName { get { return m_gloveName; } }
    public GloveID GloveID { get { return m_gloveID; } }
    public BezierCurveData CurveData { get { return m_curveData; } }
    public GloveActionParams NormalAttack { get { return m_normalAttackParam; } }
    public GloveActionParams SpecialAttack { get { return m_specialAttackParam; } }
    public GloveActionParams Grap { get { return m_grabParam; } }

    // タイプ別のアームパラメーター取得
    public StretchArmParams GetStretchArmParamsByType(GloveActionType type)
    {
        switch (type)
        {
            case GloveActionType.NORMAL_ATTACK:
                return m_normalAttackParam.StretchArmParams;
            case GloveActionType.SPECIAL_ATTACK:
                return m_specialAttackParam.StretchArmParams;
            case GloveActionType.GRAP:
                return m_grabParam.StretchArmParams;
            default:
                Debug.LogWarning($"{nameof(GloveData)}.GetStretchArmParamsByType: 未知の GloveActionType {type} が指定されました。");
                return null;
        }
    }
}

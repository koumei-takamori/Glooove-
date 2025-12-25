/**********************************************************
 *
 *  GloveData.cs
 *  グローブデータ
 *  ScriptableObjectに情報を格納
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/08/06
 *
 *********************************************************/
using System;
using System.Collections.Generic;
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
    BIG    = 1  // ビッググローブ
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
|| アクション用パラメーター
--------------------------------------------------------------------------------*/
/// <summary>
/// アクション用パラメーター
/// </summary> 
[Serializable]
public class GloveActionParams
{
    // 攻撃の識別
    [Header("識別"),SerializeField]
    private GloveActionType m_type = GloveActionType.NORMAL_ATTACK;

    // 基礎パラメーター
    [Header("基礎")]
    // 重さの種類
    [SerializeField]
    private GloveWeight gloveWeight = GloveWeight.MEDIUM;
    // クールタイム
    [SerializeField]
    private float m_coolTime = 0.5f;
    // 攻撃の倍率
    [SerializeField]
    private float m_attackMultiplier = 1.2f;
    //　最大攻撃距離
    [SerializeField]
    private float m_maxAttackRange = 30.0f;
    // 伸びきるまでの秒数
    [SerializeField]
    private float m_extendDuration = 1.0f;
    // 戻るまでの秒数
    [SerializeField]
    private float m_returnDuration = 1.0f;

    // プロパティ
    public GloveActionType Type { get { return m_type; } private set { m_type = value; } }
    public float CoolTime { get { return m_coolTime; } }
    public float AttackMultiplier { get { return m_attackMultiplier; } }
    public float MaxAttackRange { get { return m_maxAttackRange; } }
    public float ExtendDuration { get { return m_extendDuration; } }
    public float ReturnDuration { get { return m_returnDuration; } }

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

    [Header("攻撃の種類別データ")]
    // 通常攻撃
    [SerializeField] 
    private GloveAttackParams m_normalAttackParam = new GloveAttackParams(GloveActionType.NORMAL_ATTACK);
    // 強化攻撃
    [SerializeField] 
    private SpecialAttackParams m_specialAttackParam = new SpecialAttackParams (GloveActionType.SPECIAL_ATTACK);
    // つかみ
    [SerializeField]
    private GrapParams m_grabParam = new GrapParams(GloveActionType.GRAP);

    // プロパティ
    public string GloveName { get { return m_gloveName; } }
    public GloveID GloveID { get { return m_gloveID; } }
    public GloveActionParams NormalAttack { get { return m_normalAttackParam; } }
    public GloveActionParams SpecialAttack { get { return m_specialAttackParam; } }
    public GloveActionParams Grap { get { return m_grabParam; } }
}

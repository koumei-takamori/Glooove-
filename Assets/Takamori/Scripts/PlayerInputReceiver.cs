/**********************************************
 * 
 *  PlayerInputReceiver.cs 
 *  プレイヤーの入力を取得する
 * 
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/07/10
 * 
 **********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReceiver : MonoBehaviour
{
    // アクションマップ名
    private static readonly string ACTION_MAP_NAME = "Player";

    // アクション
    public enum Actions : uint
    {
        MOVE = 0,    //	移動
        JUMP,        //	ジャンプ
        L_ATTACK,      //	左攻撃
        R_ATTACK,      //	右攻撃
        DASH,        //	ダッシュ
        PARRY,       //  パリィ

        OVER_ID      //	最大数
    };
    // アクションの最大数
    public static readonly uint ACTION_COUNT = (int)Actions.OVER_ID;

    // アクション名（上記アクションのインデックスに依存）
    private static readonly string[] ACTION_NAME =
    {
        "Move",			// 移動
        "Jump",		// ジャンプ
        "LAttack",    // 左攻撃
        "RAttack",    // 右攻撃
        "Dash",          // ダッシュ
        "Parry"          // パリィ

	};

    // 入力タイプ
    public enum InputType
    {
        PRESSED,        //	入力された瞬間
        HOLD,           //	入力されている間
        RELEASED,       //	入力がなくなった瞬間
    }

    // コンポーネント
    private PlayerInput m_playerInput;

    // アクションマップ
    private InputActionMap m_gameActionMap;

    // 各アクション
    private InputAction[] m_actions;

    /*--------------------------------------------------------------------------------
	|| 実行前初期化処理
	--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        //	コンポーネントの取得
        m_playerInput = GetComponent<PlayerInput>();

        //	アクションマップの取得
        m_gameActionMap = m_playerInput.actions.FindActionMap(ACTION_MAP_NAME);

        //	各アクションの取得
        m_actions = new InputAction[ACTION_COUNT];
        for (int i = 0; i < ACTION_COUNT; i++)
        {
            m_actions[i] = m_gameActionMap.FindAction(ACTION_NAME[i], true);
        }
    }

    /*--------------------------------------------------------------------------------
	|| 入力の取得
	--------------------------------------------------------------------------------*/
    /// <summary>
    /// 入力の取得
    /// </summary>
    /// <param name="action">アクション</param>
    /// <param name="type">入力タイプ</param>
    /// <returns></returns>
    public bool GetInputButton(Actions action, InputType type)
    {
        switch (type)
        {
            //	入力された瞬間
            case InputType.PRESSED:
                return m_actions[(uint)action].WasPressedThisFrame();

            //	入力されている間
            case InputType.HOLD:
                return m_actions[(uint)action].IsPressed();

            //	入力がなくなった瞬間
            case InputType.RELEASED:
                return m_actions[(uint)action].WasReleasedThisFrame();

            //	未指定
            default:
                return false;
        }
    }

    /*--------------------------------------------------------------------------------
	|| 任意の型での入力を取得
	--------------------------------------------------------------------------------*/
    /// <summary>
    /// 任意の型での入力を取得
    /// </summary>
    /// <typeparam name="T">取得したい型</typeparam>
    /// <param name="action">アクション</param>
    /// <returns></returns>
    public T GetInputValue<T>(Actions action)
        where T : struct
    {
        return m_actions[(uint)action].ReadValue<T>();
    }
}

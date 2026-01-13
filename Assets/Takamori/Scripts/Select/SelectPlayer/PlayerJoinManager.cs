/**********************************************************
 *
 *  PlayerJoinManager.cs
 *  プレイヤーの接続を管理する
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/04
 *
 *********************************************************/
using Nakashi.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの接続を管理する
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class PlayerJoinManager : MonoBehaviour
{
    // JoinするInputActionのあるInputActionAsset 
    [SerializeField]
    private InputActionAsset m_joinInputActionasset = default;

    // PlayerInputがアタッチされているプレイヤーオブジェクト
    [SerializeField]
    private PlayerInput m_playerPrefab = default;

    // 最大参加人数
    [SerializeField]
    private int m_maxPlayerCount = default;

    // プレイヤーがゲームにJoinするためのInputAction
    private InputAction m_playerJoinInputAction = default;

    // Join済みのデバイス情報
    private InputDevice[] m_joinedDevices = default;

    // 現在のプレイヤー数
    private int m_currentPlayerCount = 0;

    // プロパティ
    public InputDevice[] JoinedDevices { get { return m_joinedDevices; } }

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        // 最大参加可能数で配列を初期化
        m_joinedDevices = new InputDevice[m_maxPlayerCount];

        // InputActionを取得
        m_playerJoinInputAction = m_joinInputActionasset.FindAction("Join", true);

        // InputActionを有効化し、コールバックを設定
        m_playerJoinInputAction.Enable();
        m_playerJoinInputAction.performed += OnJoin;
    }

    /// <summary>
    /// デバイスによってJoin要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // プレイヤー数が最大数に達していたら、処理を終了
        if (m_currentPlayerCount >= m_maxPlayerCount)
        {
            return;
        }

        // Join要求元のデバイスが既に参加済みのとき、処理を終了
        foreach (var device in m_joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // PlayerInputを所持した仮想のプレイヤーをインスタンス化
        var playerInput = PlayerInput.Instantiate(
            prefab: m_playerPrefab.gameObject,
            playerIndex: m_currentPlayerCount,
            pairWithDevice: context.control.device
            );

        // Joinしたデバイス情報を保存
        m_joinedDevices[m_currentPlayerCount] = context.control.device;

        // プレイヤー管理クラスにプレイヤーを追加
        SelectPlayerManager.Instance.OnPlayerJoined(playerInput);

        // コントローラーの場合、振動させる
        VibrateGamepad();

        // 追加：Join効果音再生
        SoundManager.Instance.PlaySE("Join");

        // プレイヤー数を加算
        m_currentPlayerCount++;
    }
    // 振動させるメソッド
    private void VibrateGamepad(float duration = 0.4f, float power = 1.0f)
    {
        if (m_joinedDevices[m_currentPlayerCount] is Gamepad gamepad)
        {
            // 振動開始
            gamepad.SetMotorSpeeds(power, power);

            // 一定時間後に停止
            StartCoroutine(StopVibration(gamepad, duration));
        }
    }
    private IEnumerator StopVibration(Gamepad gamepad, float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}

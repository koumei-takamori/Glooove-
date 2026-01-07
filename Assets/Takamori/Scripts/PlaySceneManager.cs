/**********************************************************
 *
 *  PlayeSceneManager.cs
 *  プレイシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using Nakashi.Player;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// プレイシーンを管理するクラス
/// </summary>
public class PlaySceneManager : SingletonMonoBehaviour<PlaySceneManager>
{
    // プレイヤー
    [SerializeField]
    private ArmPlayerController m_1pPlayer;
    [SerializeField]
    private ArmPlayerController m_2pPlayer;

    // カメラコントローラー
    [SerializeField]
    private CameraContoller m_cameraContorller;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    override protected void Awake()
    {
        base.Awake();
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // プレイヤーを取得
        m_1pPlayer = PlayerRegistry.Instance.GetPlayer(0).GetComponent<ArmPlayerController>();
        m_2pPlayer = PlayerRegistry.Instance.GetPlayer(1).GetComponent<ArmPlayerController>();

        // お互いをターゲット設定
        m_1pPlayer.Target = m_2pPlayer.AttackPoint;
        m_2pPlayer.Target = m_1pPlayer.AttackPoint;

        // プレイヤーのカメラの設定
        SetupPlayerCameras();
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーのカメラの設定
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーのカメラの設定
    /// </summary>
    private void SetupPlayerCameras()
    {
        // Transform取得
        Transform player1 = PlayerRegistry.Instance.GetPlayer(0).transform;
        Transform player2 = PlayerRegistry.Instance.GetPlayer(1).transform;

        m_cameraContorller.Player1 = player1;
        m_cameraContorller.Player2 = player2;

        m_cameraContorller.InitCameraTargets();
    }
}



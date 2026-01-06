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
    private ArmPlayerController m_1pPlayer;
    private ArmPlayerController m_2pPlayer;

    // カメラコントローラー1
    [SerializeField]
    private CameraContoller m_cameraContoroller1;

    //// カメラコントローラー1
    //[SerializeField]
    //private CameraContoller m_cameraContoroller2;


    // 仮のTarget（パンチング）
    [SerializeField]
    private　Transform m_transform;

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
        // m_2pPlayer = PlayerRegistry.Instance.GetPlayer(1).GetComponent<ArmPlayerController>();

        // お互いをターゲット設定
        m_1pPlayer.Target = m_transform;
        // m_2pPlayer.Target = m_1pPlayer.transform;

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
        Transform player1 = m_1pPlayer.CameraPivot;
        // Transform player2 = PlayerRegistry.Instance.GetPlayer(1).transform;

        // 1Pカメラ
        m_cameraContoroller1.Owner = player1;
        m_cameraContoroller1.Target = m_1pPlayer.Target;

        //// 2Pカメラ
        //m_cameraContoroller2.Owner = player2;
        //m_cameraContoroller2.Target = player1;

        m_cameraContoroller1.InitCameraTargets();
    }
}



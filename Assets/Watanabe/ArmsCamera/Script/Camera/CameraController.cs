/**********************************************
 * 
 *  CameraContoller.cs 
 *  カメラの管理クラス
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/

using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CameraContoller : MonoBehaviour
{
    /// <summary>
    /// 状態
    /// </summary>
    public enum State
    {
        None    　　　　//
        , IDEL   　　　　//通常
        , MOVE   　　　　//動き
        , JUMP   　　　　//ジャンプ
        , ATTACK 　　　　//攻撃
        , DASH
    }

    /// <summary>
    /// カメラの種類
    /// </summary>
    public enum CameraType : int 
    {
        NORMAL             //通常カメラ
        , GRAP             //つかみ時カメラ
        , GRABBED          //掴まれた時カメラ
    }

    //所有者
    [SerializeField]
    private Transform m_owner;
    //ターゲット
    [SerializeField]
    private Transform m_target;

    [SerializeField ,Header("パラメーター")]
    private CameraData m_data;

    [SerializeField, Header("ターゲットグループ")]
    private CinemachineTargetGroup m_targetGroup;

    //カメラのTypeとバーチャルカメラの構造体
    [Serializable]
    private struct CameraEntry
    {
        public CameraType type;
        public CinemachineVirtualCamera virtualCamera;
    }

    [SerializeField,Header("カメラの配列")]
    private List<CameraEntry> CameraEntries = new List<CameraEntry>();


    //プレイアブルディレクター
    private PlayableDirector m_playableDirector;

    //トランスフォーム
    private Transform m_transform;
    //ステータス
    private CameraStateMachine m_stateMachine;
    //ステータス
    private CameraStatus m_status;


    //実行カメラの種類
    CameraType m_currentCameraType;

    //カメラリストの保存配列       ListからDictionaryに変換して保存する用
    private Dictionary<CameraType, CinemachineVirtualCamera> m_cameraDict;

    private void Awake()
    {

        // List を Dictionary に変換 
        m_cameraDict = new Dictionary<CameraType, CinemachineVirtualCamera>();
        foreach (var entry in CameraEntries)
        {
            if (!m_cameraDict.ContainsKey(entry.type) && entry.virtualCamera != null)
            {
                m_cameraDict.Add(entry.type, entry.virtualCamera);
            }
        }

        //コンポーネントの取得
        //トランスフォームの取得
        m_transform = GetComponent<Transform>();
        //Playableの取得
        m_playableDirector = GetComponent<PlayableDirector>();

        //ステータスの作成
        m_status = new CameraStatus(this);

        //ステートマシーンの作成
        m_stateMachine = new CameraStateMachine(this);
        
        //ステートマシーンの初期化
        m_stateMachine.Initialize(m_stateMachine.GetIdelState);

    //初期カメラ状態の設定  AWakeでやる　やらないと起動時の挙動が変化する為
        //通常カメラの設定
        var normalCamera = GetCamera(CameraType.NORMAL);
        normalCamera.m_Follow = m_owner;
        normalCamera.m_LookAt = m_targetGroup.transform;
        


        //つかみカメラの設定
        var grapCamera = GetCamera(CameraType.GRAP);
        
        grapCamera.m_LookAt = m_targetGroup.transform;
        //grapCamera.m_LookAt = m_owner;



        //カメラの優先度の設定
        normalCamera.Priority = 10;
        grapCamera.Priority = 0; // 無効にする

        //カメラの位置の設定
        normalCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = m_status.GetEye;
      



    }



    // Start is called before the first frame update
    void Start()
    {

        //グループのターゲットの追加
        m_targetGroup.m_Targets[0].target = m_owner;
        m_targetGroup.m_Targets[1].target = m_target;
        
        //イベントの追加
        EventManager.Instance.Subscribe<PlayerEvent>(ReceiveData);

    }

    // Update is called once per frame
    void Update()
    {

        //ステートマシーンの更新
        m_stateMachine.Update();

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(m_currentCameraType == CameraType.NORMAL)
            {
                //ChangeCamera(CameraType.CATCH);
                m_playableDirector.Play();
            }
            else
            {
                //ChangeCamera(CameraType.NORMAL);
            }

            
        }
       

    }

    /// <summary>
    /// ステートの切り替え
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.None:
                break;
            case State.IDEL:
                m_stateMachine.ChangeState(m_stateMachine.GetIdelState);
                break;
            case State.MOVE:
                m_stateMachine.ChangeState(m_stateMachine.GetMoveState);
                break;
            case State.JUMP:
                m_stateMachine.ChangeState(m_stateMachine.GetJumpState);
                break;
            case State.ATTACK:
                m_stateMachine.ChangeState(m_stateMachine.GetAttackState);
                break;
            case State.DASH:
                m_stateMachine.ChangeState(m_stateMachine.GetDashState);
                break;
            default: 
                break;
        }
    }

    /// <summary>
    /// カメラの種類の変更
    /// </summary>
    /// <param name="type">変更したいカメラタイプ</param>
    public void ChangeCamera(CameraType type)
    {

        if(m_currentCameraType == type) return;

        //現在のカメラの優先度を下げる
        var currentCamera = GetCamera(m_currentCameraType);

        currentCamera.Priority = 0;

        switch(type)
        {
            case CameraType.NORMAL:
                m_currentCameraType = CameraType.NORMAL;
                break;
            case CameraType.GRAP:
                m_currentCameraType= CameraType.GRAP;
                break;
            case CameraType.GRABBED:
                m_currentCameraType = CameraType.GRABBED;
                break;
            default:
                break;
        }


        //変更後のカメラの優先度を上げる
        var nextCamera = GetCamera(m_currentCameraType);
        nextCamera.Priority = 10;

    }

        
    /// <summary>
    /// メッセージ受信
    /// </summary>
    /// <param name="evets">通知タイプの構造体</param>
    public void ReceiveData(PlayerEvent evets)
    {

        switch (evets.Message)
        {
            case "Idel":
                ChangeState(State.IDEL);
                break;
            case "Move":
                ChangeState(State.MOVE);
                break;
            case "Jump":
                ChangeState(State.JUMP);
                break;
            case "Dash":
                ChangeState(State.DASH);
                break;
            default: 
                break;
        }
    }

    /// <summary>
    /// CameraType から VirtualCamera を取得
    /// </summary>
    public CinemachineVirtualCamera GetCamera(CameraType type)
    {
        if (m_cameraDict.TryGetValue(type, out var cam))
        {
            return cam;
        }


        Debug.LogWarning($"Camera not found for type: {type}");
        return null;
    }


    /// <summary>
    /// つかんだとき
    /// </summary>
    public void OnGrap()
    {

        ////コントローラーをプレイヤにあわせる
        m_transform.position = m_owner.transform.position;
        m_transform.rotation = m_owner.transform.rotation;


        //通常カメラ
        var normalCamera = GetCamera(CameraType.NORMAL);
        //つかみカメラ
        var grapCamera = GetCamera(CameraType.GRAP);
        //var grapCamera = GetCamera(CameraType.GRABBED);

        grapCamera.transform.position = normalCamera.transform.position;
        grapCamera.transform.rotation = normalCamera.transform.rotation;


    }

    /// <summary>
    /// つかみの終了
    /// </summary>
    public void EndGrap()
    {



    }


    //カメラのトランスフォームの取得
    public Transform GetTransform { get { return m_transform; } }
    //所有者の取得
    public Transform GetOwner { get { return m_owner; } }
    //ターゲットの取得
    public Transform GetTarget { get { return m_target; } }
    //ステートマシーン
    public CameraStateMachine GetStateMachine { get { return m_stateMachine; } }
    //ステータス
    public CameraStatus GetStatus { get {  return m_status; } }
    //データ
    public CameraData GetData { get { return m_data;} }

}

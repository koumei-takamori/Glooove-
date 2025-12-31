/**********************************************************
 *
 *  ResultSceneManager.cs
 *  リザルトシーンの管理クラス
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2025/12/26
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private UIFade m_fade;

    [SerializeField,Header("勝ったキャラクター")]
    private WinnerCharacter m_character;

    [SerializeField, Header("使用したステージ")]
    private ResultStage m_resultStage;

    [SerializeField, Header("勝ったプレイヤー　１Pか２Pか")]
    private WinnerPlayer m_winnerPlayer;


    //勝利キャラクター
    private int m_winnerCharacterId;
    //勝利プレイヤー 1Pか2P
    private int m_winnerPlayerId;
    //ステージID
    private int m_stageId;

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    void Awake()
    {
        //デバック
        m_winnerCharacterId = 1;
        m_stageId = 2;
        m_winnerPlayerId = 1;
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //表示キャラクターの設定・変更
        m_character.winnerId = m_winnerCharacterId;
        m_character.ChangeCharacter();

        //ステージの変更
        m_resultStage.ChangeStage(m_stageId);

        //UI変更
        m_winnerPlayer.winnerPlayer = m_winnerPlayerId;
        m_winnerPlayer.ChangeTextUI();

    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeOutWithCallback(() =>
            {
                // セレクトシーンに移行
                SceneLoader.Load("TitleScene");
            });

        }
    }

    //勝利プレイヤーのゲットセット　１Pか２Pか
    public int WinerPlayerID { get {  return m_winnerPlayerId; } set { m_winnerPlayerId = value; } }
    //ステージIDのゲットセット
    public int StageID { get { return m_stageId; } set { m_stageId = value; } }
    //勝利キャラクターのゲットセット
    public int WinnerCharacterID { get { return m_winnerCharacterId; } set { m_winnerCharacterId= value; } }  

}

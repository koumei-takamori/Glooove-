/**********************************************************
 *
 *  ResultSceneManager.cs
 *  リザルトシーンの管理クラス
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2024/12/26
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private UIFade m_fade;

    [SerializeField,Header("CanvasのWinnerCharactor参照")]
    private WinnerCharancterImg m_winnerCharancterImg;
    


    //スコア
    private int m_score;

    //勝利キャラクター
    private int m_winnerCharacterId;


    //勝利プレイヤー 1Pか2P
    private int m_winnerPlayer;

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    private void Awake()
    {
        m_winnerCharacterId = 1;
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        ////もらったキャラクターIDを表示画像管理のスクリプトのIDを渡す
        //m_winnerCharancterImg.CharacterId = m_winnerCharacterId;


        ////キャラクターの表示変更
        //m_winnerCharancterImg.ChangeImage();

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

    //スコアのゲットセット
    public int Score { get { return m_score; } set { m_score = value; } }

    //勝利プレイヤーのゲットセット
    public int winerPlayer { get {  return m_winnerPlayer; } set { m_winnerPlayer = value; } }



}

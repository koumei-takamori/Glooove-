using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private UIFade m_fade;


    //スコア
    private int m_score;

    //勝利キャラクター
    private int m_winerCharactor;


    //勝利プレイヤー 1Pか2P
    private int m_winerPlayer;

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    private void Awake()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
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
    public int winerPlayer { get {  return m_winerPlayer; } set { m_winerPlayer = value; } }



}

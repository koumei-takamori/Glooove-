/**********************************************************
 *
 *  WinnerCharacter.cs
 *  リザルトシーンの表示キャラ管理クラス
 *  ID  バランス　＝０
 *      スピード　＝１
 *      タンク　　＝２
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2025/12/31
 *
 *********************************************************/
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerCharacter : MonoBehaviour
{

    [SerializeField, Header("バランスタイプ")]
    private GameObject m_balance;
    [SerializeField, Header("スピードタイプ")]
    private GameObject m_speed;
    [SerializeField, Header("タンクタイプ")]
    private GameObject m_tank;

    //選ばれているキャラクター
    private GameObject m_selectChracter;

    //勝利キャラクターID
    private int m_winnerId;

    void Awake()
    {
        //アニメーションを無効か
        m_balance.GetComponent<Animator>().enabled = false;
        m_speed.GetComponent<Animator>().enabled = false;
        m_tank.GetComponent<Animator>().enabled = false;
        //無効
        m_balance.SetActive(false);
        m_speed.SetActive(false);
        m_tank.SetActive(false);

        m_winnerId = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //表示キャラの変更
    public void ChangeCharacter()
    {
        switch (m_winnerId)
        {
            case 0:  //バランス
                m_balance.SetActive(true);
                m_selectChracter = m_balance;
                break;
            case 1:  //スピード
                m_speed.SetActive(true);
                m_selectChracter = m_speed;

                break;
            case 2:  //タンク
                m_tank.SetActive(true);
                m_selectChracter = m_tank;

                break;
            default:
                break;

        }
    }

    //アニメーション再生
    public void StartAnimation()
    {

        if (m_selectChracter == null)
        {
            return;
        }


        m_selectChracter.GetComponent<Animator>().enabled = true;

    }

    //勝利キャラクターIDのゲットセット
    public int winnerId { get { return m_winnerId; } set { m_winnerId = value; } }
}

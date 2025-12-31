/**********************************************************
 *
 *  WinnerCharacter.cs
 *  リザルトシーンの表示キャラ管理クラス
 *  ID  ストリートステージ　　　　＝０
 *      ライブステージ　　　　　　＝１
 *      ジャンクフードステージ　　＝２
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2025/12/31
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultStage : MonoBehaviour
{
    [SerializeField, Header("ストリートステージ")]
    private GameObject m_street;
    [SerializeField, Header("ライブステージ")]
    private GameObject m_live;
    [SerializeField, Header("ジャンクフードステージ")]
    private GameObject m_junkFood;


    void Awake()
    {
        //ステージの無効
        m_street.SetActive(false);
        m_live.SetActive(false);
        m_junkFood.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステージの変更
    /// </summary>
    /// <param name="stageId">ステージID</param>
    public void ChangeStage(int stageId)
    {
        switch (stageId)
        {
            case 0:
                m_street.SetActive(true);
                break;
            case 1:
                m_live.SetActive(true);
                break;
            case 2:
                m_junkFood.SetActive(true);
                break;
            default: break;
        }
    }

}

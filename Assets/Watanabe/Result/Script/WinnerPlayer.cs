/**********************************************************
 *
 *  WinnerPlayer.cs
 *  1P 2Pどちらが勝ったのかUIに反映させる
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2024/12/31
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerPlayer : MonoBehaviour
{

    //1P 2Pどちらが勝ったか 1か２のみ
    private int m_winnerPlayer;

    private TextMeshProUGUI m_textMeshPro;
    // Start is called before the first frame update

    private void Awake()
    {
        m_textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //UIの文字の変更
    public void ChangeTextUI()
    {
        Debug.Log(m_textMeshPro);

        if (m_textMeshPro == null) { return; }


        //１・２以外なら
        if (m_winnerPlayer != 1 && m_winnerPlayer != 2) { return; }


        m_textMeshPro.text = $"{m_winnerPlayer}P";


    }

    //勝利プレイヤーのセットゲット
    public int winnerPlayer { get { return m_winnerPlayer; } set { m_winnerPlayer = value; } }

}

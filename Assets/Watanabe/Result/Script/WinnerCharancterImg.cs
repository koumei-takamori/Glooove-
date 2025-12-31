/**********************************************************
 *
 *  WinnerCharancterImg.cs
 *  リザルトの勝利キャラクターの画像管理クラス
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2025/12/26
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerCharancterImg :MonoBehaviour
{
    //イメージ
    private Image m_image; 


    //キャラクターID
    private int m_characterId;

    //IDと画像の関連づけ
    private Dictionary<int, string> m_imagePathTable;




    private void Awake()
    {
        m_image = GetComponent<Image>();
        //テーブル作成
        InitializeTable();
    }

    private void Start()
    {

    }

    void InitializeTable()
    {
        m_imagePathTable = new Dictionary<int, string>
        {
            { 1, "Assets/Watanabe/Result/UI/Character/balance" },
            { 2, "Assets/Watanabe/Result/UI/Character/power" },
            { 3, "Assets/Watanabe/Result/UI/Character/speed" }
        };
    }

    //画像の切り替え
    public void ChangeImage()
    {


        if (!m_imagePathTable.TryGetValue(m_characterId, out var path))
        {
            Debug.LogWarning($"ID {m_characterId} に対応する画像パスがありません");
            return;
        }

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
        {
            Debug.LogWarning($"画像が見つかりません: {path}");
            return;
        }

        m_image.sprite = sprite;
    }
    

    //キャラクターIDのセットゲット
    public int CharacterId {get{ return m_characterId; } set { m_characterId = value; } }

}

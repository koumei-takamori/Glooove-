/*
*       DemoPunchingBox.cs
*       試遊用サンドバッグスクリプト
*       単純に、体力の管理、ダメージの受け取り、死亡処理を行う
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPunchingBox : SingletonMonoBehaviour<DemoPunchingBox>
{
    // 体力
    [Header("体力")] public int hp = 100;
    // 最大体力
    [Header("最大体力")] public int maxhp = 100;
    // フェード管理
    [SerializeField]
    private UIFade m_fade;

    override protected void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 体力を最大体力に設定
        hp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // ダメージを受け取る
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("サンドバッグが" + damage + "のダメージを受けた。残り体力:" + hp);
        if (hp <= 0)
        {
            // 何かしらの処理
            // awaitしてシーンロード処理とPlayerManagerを取得
            m_fade.FadeOutWithCallback(() =>
            {
                // タイトルシーンに移行
                SceneLoader.Load("TitleScene");
            });
        }
    }

    // 死亡処理
    void Die()
    {
        // サンドバッグを消す
        Destroy(gameObject);
    }


}

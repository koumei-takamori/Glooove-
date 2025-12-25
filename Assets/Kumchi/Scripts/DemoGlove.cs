/*
*       DemoGlove.cs
*       グローブとサンドバッグの当たり判定をし、
*       サンドバッグにダメージを与えるスクリプト
*/

using Ikeda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGlove : MonoBehaviour
{
    // HPゲージシステム
    private HPGaugeSystem hPGaugeSystem;

    // Start is called before the first frame update
    void Start()
    {
        // HPGaugeSystemコンポーネントをシーンから取得
        hPGaugeSystem = FindObjectOfType<HPGaugeSystem>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // サンドバッグに当たったらダメージを与える
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("サンドバッグに当たった");
            // サンドバッグにダメージを与える
            DemoPunchingBox.Instance.TakeDamage(10);
            // HPGaugeSystemのDamageを呼ぶ
            if (hPGaugeSystem != null)
            {
                hPGaugeSystem.Damage(10);
            }
        }
    }

}

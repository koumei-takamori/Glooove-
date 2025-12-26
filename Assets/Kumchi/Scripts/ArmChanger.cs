/*
*       @file ArmChanger.cs
*       @brief 攻撃用ののびる腕と通常時の腕の表示の切り替えを行うスクリプト
*       @author 熊澤圭祐
*/

using Nakashi.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmChanger : MonoBehaviour
{
    // 腕の通常時のモデル
    [SerializeField]
    private GameObject m_normalRArm;
    [SerializeField]
    private GameObject m_normalLArm;
    // 腕の攻撃用のモデルの親オブジェクト
    [SerializeField]
    private GameObject m_attackRArmParent;
    [SerializeField]
    private GameObject m_attackLArmParent;

    // 腕コントローラー
    private ArmPlayerController m_armPlayerController;

    // 右腕が伸びているか
    [SerializeField] private bool m_isRArmExtend = false;
    // 左腕が伸びているか
    [SerializeField] private bool m_isLArmExtend = false;


    // Start is called before the first frame update
    void Start()
    {
        // 腕コントローラーを取得
        m_armPlayerController = GetComponent<ArmPlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        // 左腕攻撃ステートなら
        if (m_isLArmExtend)
        {
            m_attackLArmParent.SetActive(true);
            m_normalLArm.SetActive(false);
            Debug.Log("左腕攻撃");
        }
        else
        {
            m_attackLArmParent.SetActive(false);
            m_normalLArm.SetActive(true);
            //Debug.Log("左腕通常");
        }


        // 右腕攻撃ステートなら
        if (m_isRArmExtend || m_armPlayerController.RigthGlove.IsActionActive(GloveActionType.NORMAL_ATTACK))
        {
            m_attackRArmParent.SetActive(true);
            m_normalRArm.SetActive(false);
            Debug.Log("右腕攻撃");
        }
        else
        {
            m_attackRArmParent.SetActive(false);
            m_normalRArm.SetActive(true);
            //Debug.Log("右腕通常");
        }

        //Debug.Log($"右{m_isRArmExtend}：左{m_isLArmExtend}");

    }
}

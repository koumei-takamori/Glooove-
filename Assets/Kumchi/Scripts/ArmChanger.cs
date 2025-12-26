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
    // 腕の攻撃用のモデル
    [SerializeField]
    private GameObject m_attackRArm;
    [SerializeField]
    private GameObject m_attackLArm;
    // ステートマシン
    private ArmPlayerStateMachine m_stateMachine;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

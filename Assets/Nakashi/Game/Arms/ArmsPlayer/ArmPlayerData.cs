//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			NakashiPlayerData.h
// <概要>		　　プレイヤーの固有データ
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        [CreateAssetMenu(menuName = "CreateArmPlayerData/PlayerData" , fileName = "ArmPlayerData")]
        public class ArmPlayerData : ScriptableObject
        {
            [Header("移動速度"), SerializeField] private float m_walkSpeed;
            [Header("ダッシュ速度"), SerializeField] private float m_dashSpeed;
            [Header("ジャンプ量"), SerializeField] private float m_jumpForce;
            [Header("Ray伸ばす距離"), SerializeField] private float m_rayDistance;
            [Header("ダッシュクールタイム"), SerializeField] private float m_dashCoolTime;
            [Header("最大HP量"), SerializeField] private float m_maxHp;
            [Header("攻撃力"), SerializeField] private float m_attackPower;
            [Header("防御力"), SerializeField] private float m_defensePower;
            [Header("必殺技ゲージ最大値"), SerializeField] private float m_maxSpecialGauge;

            public float GetWalkSpeed() => m_walkSpeed;
            public float GetDashSpeed() => m_dashSpeed;
            public float GetJumpForce() => m_jumpForce;
            public float GetRayDistance() => m_rayDistance;
            public float GetDashCoolTime() => m_dashCoolTime;
            public float GetMaxHP() => m_maxHp;
            public float GetAttackPower() => m_attackPower;
            public float GetDefensePower() => m_defensePower;
            public float GetMaxSpecialGauge() => m_maxSpecialGauge;
        }
    }
}



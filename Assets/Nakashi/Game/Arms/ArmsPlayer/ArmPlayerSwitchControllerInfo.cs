//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/11/18
// <file>			SwitchControllerInfo.h
// <概要>		　　スイッチコントローラーの角度情報
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerSwitchControllerInfo
        {
            // 右手左手オイラー角
            private Vector3 m_rightEuler;
            private Vector3 m_leftEuler;

            // 右手オイラー角の取得
            public Vector3 GetRightEuler() { return m_rightEuler; }
            // 左手オイラー角の取得
            public Vector3 GetLeftEuler() { return m_leftEuler; }

            // 閾値
            float m_threshold = 35f;
            // ポーズの基準点
            private Vector3 rightPoseR = new Vector3(320, 75, 15);
            private Vector3 rightPoseL = new Vector3(320, 65, 180);
            private Vector3 leftPoseR = new Vector3(315, 278, 166);
            private Vector3 leftPoseL = new Vector3(319, 265, 350);
            private Vector3 frontPoseR = new Vector3(320, 0, 80);
            private Vector3 frontPoseL = new Vector3(320, 330, 290);
            private Vector3 backPoseR = new Vector3(310, 190, 260);
            private Vector3 backPoseL = new Vector3(315, 135, 100);

            // 右手コントローラーが各ポーズに入っているかの判定
            public bool RightControllerRightPose() { return IsPoseMatch(m_rightEuler, rightPoseR); }
            public bool RightControllerLeftPose() { return IsPoseMatch(m_rightEuler, leftPoseR); }
            public bool RightControllerFrontPose() { return IsPoseMatch(m_rightEuler, frontPoseR); }
            public bool RightControllerBackPose() { return IsPoseMatch(m_rightEuler, backPoseR); }

            // 左手コントローラーが各ポーズに入っているかの判定
            public bool LeftControllerRightPose() { return IsPoseMatch(m_leftEuler, rightPoseL); }
            public bool LeftControllerLeftPose() { return IsPoseMatch(m_leftEuler, leftPoseL); }
            public bool LeftControllerFrontPose() { return IsPoseMatch(m_leftEuler, frontPoseL); }
            public bool LeftControllerBackPose() { return IsPoseMatch(m_leftEuler, backPoseL); }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ArmPlayerSwitchControllerInfo()
            {
                //m_rightEuler = Nakashi.Framework.AxisSystem.Instance.GetRightQuaternion().eulerAngles;
                //m_leftEuler = Nakashi.Framework.AxisSystem.Instance.GetLeftQuaternion().eulerAngles;
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            public void Update()
            {
                // オイラー角の更新を行う
                UpdateEuler();
            }

            /// <summary>
            /// コントローラーのオイラー角同期
            /// </summary>
            private void UpdateEuler()
            {
                //m_rightEuler = Nakashi.Framework.AxisSystem.Instance.GetRightQuaternion().eulerAngles;
                //m_leftEuler = Nakashi.Framework.AxisSystem.Instance.GetLeftQuaternion().eulerAngles;
            }

            /// <summary>
            /// 現在のポーズがマッチしているかどうかの判定用
            /// </summary>
            /// <param name="current"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            private bool IsPoseMatch(Vector3 current, Vector3 target)
            {
                return
                    AngleDiff(current.x, target.x) < m_threshold &&
                    AngleDiff(current.z, target.z) < m_threshold;

            }


            /// <summary>
            /// Unityのオイラー角は360度のラップされるので、それを越した場合の変化時も対応するための関数
            /// </summary>
            /// <returns></returns>
            private float AngleDiff(float a, float b)
            {
                float diff = Mathf.Abs(a - b) % 360;
                return diff > 180 ? 360 - diff : diff;
            }
        }
    }
}


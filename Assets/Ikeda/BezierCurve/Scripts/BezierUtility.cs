/**********************************************************
 *  BezierUtility.cs
 *  ベジェ曲線の評価関数をまとめたユーティリティ
 *
 *  制作者 : 池田桜輔
 *  改良日 : 2025/09/04
 **********************************************************/
using UnityEngine;


namespace Ikeda.Bezier
{
    public static class BezierUtility
    {
        /// <summary>
        /// 4点ベジェ曲線の座標を計算
        /// </summary>
        public static Vector3 Evaluate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            return u * u * u * p0 +
                   3f * u * u * t * p1 +
                   3f * u * t * t * p2 +
                   t * t * t * p3;
        }
    }
}

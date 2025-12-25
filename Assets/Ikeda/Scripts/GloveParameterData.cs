// -------------------------------------------------------------
// GloveParameterData.cs
// グローブが持つデータをまとめて保存する ScriptableObject
// 制作者 : 池田桜輔
// 改良日 : 2025/12/14
// -------------------------------------------------------------

using UnityEngine;

namespace Ikeda
{
    [CreateAssetMenu(menuName = "Ikeda/GloveParamData", fileName = "GloveParamData")]
    public class GloveParameterData : ScriptableObject
    {
        [Header("曲線挙動")]
        [SerializeField] private BezierCurveData curveData;

        [Header("攻撃力")]
        [SerializeField] private int nomalAttackPower = 10;
        [SerializeField] private int strongAttackPower = 15;

        [Header("移動速度(倍率)")]
        [SerializeField] private float nomalAttackSpeed = 1.0f;
        [SerializeField] private float strongAttackSpeed = 1.2f;
    }
}
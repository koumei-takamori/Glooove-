/* ----------------------------------------------------------------
 * 
 * CharactorGloveData.cs
 * キャラクターが選択できる3種類のグローブを格納する
 * スクリプタブルオブジェクト
 * 
 * 池田桜輔
 * 2025/08/25
 * 
 * ------------------------------------------------------------------*/

using UnityEngine;

[CreateAssetMenu(menuName = "Ikeda/CharacterGloveData", order = 0)]
public class CharacterGloveData : ScriptableObject
{
    [Header("左手グローブ")]
    public GloveData leftGlove1;
    public GloveData leftGlove2;
    public GloveData leftGlove3;

    [Header("右手グローブ")]
    public GloveData rightGlove1;
    public GloveData rightGlove2;
    public GloveData rightGlove3;
}

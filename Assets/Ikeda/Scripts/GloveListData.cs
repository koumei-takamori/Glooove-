// ------------------------------------------------------------------------
// 制作物：GloveListData.cs
// 内容　：グローブのリストをScriptableObjectで管理するクラス
// 制作者；池田桜輔
// 作成日：2025/12/16
// ------------------------------------------------------------------------

using UnityEngine;


/// <summary>
/// グローブリストエントリー
/// </summary>

[System.Serializable]
public struct GloveListEntry
{
    // グローブ名
    public string gloveName;
    // グローブオブジェクト
    public GameObject gloveObject;
}

/// <summary>
/// ScriptableObject ：　グローブのリストを管理するクラス
/// </summary>
[CreateAssetMenu(menuName = "Glove/GloveListData", fileName = "GloveListData")]
[System.Serializable]
public class GloveListData : ScriptableObject
{
    // ------------
    // 変数
    // ------------
    public GloveListEntry[] gloveEntries;

    // ------------
    // ﾌﾟﾛﾊﾟﾃｨ
    // ------------
    public GloveListEntry[] AllGloveList { get { return gloveEntries; } }
    public int GloveCount { get { return gloveEntries.Length; } }

    // ------------
    // メソッド
    // ------------
    public GameObject GetGloveByName(string name)
    {
        foreach (var entry in gloveEntries)
        {
            if (entry.gloveName == name)
            {
                return entry.gloveObject;
            }
        }
        return null; // 見つからなかった場合
    }
}

using UnityEngine;


public enum BGMType
{
    Title,
    Select,
    Play,
    Result
}

[System.Serializable]
public struct SceneBGMData
{
    public BGMType bgmType;
    // くまちゃんのSoundDatasを参照
    public string bgmName;
}

public class BGMPlayer : MonoBehaviour
{
    // シーン
    [SerializeField] private BGMType sceneType;

    [SerializeField] private SceneBGMData[] datas = null;

    private void Start()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManagerが存在しません。");
            return;
        }

        if (datas == null || datas.Length == 0)
        {
            Debug.LogError("BGMデータが設定されていません。");
            return;
        }

        // datasの中に一致するものを探索
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].bgmType == sceneType)
            {
                SoundManager.Instance.PlayBGM(datas[i].bgmName, true);
                return;
            }
        }

        // ここを実行 = 一致するものがなかった
        Debug.LogError("指定されたシーンタイプに対応するBGMデータが見つかりません。");
    }
}

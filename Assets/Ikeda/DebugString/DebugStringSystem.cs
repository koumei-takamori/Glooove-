using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// デバッグストリング用
/// 使用にはCanvas/DebugStringが必要
/// </summary>
public class DebugStringSystem
{
    // シングルトンクラス
    private static DebugStringSystem instance;
    public static DebugStringSystem Instance => instance ??= new DebugStringSystem();

    private DebugStringSystem() { }


    [SerializeField] private Text text;

    // 登録された変数（getterで毎フレーム値を取得）
    private Dictionary<string, Func<string>> variables = new();


    public void Start(string objectHierarchyPath)
    {
        // 取得
        text = GameObject.Find(objectHierarchyPath).GetComponent<Text>();

        if (text == null)
        {
            Debug.Log("DebuString：取得失敗");
        }
        else
        {
            Debug.Log("DebugString：成功");
        }
    }


    /// <summary>
    /// 任意の型の変数を登録（Startで一度だけ呼び出せばOK）
    /// </summary>
    public void AddVariable<T>(string key, Func<T> getter)
    {
        // 引数なしのゲッターのラムダ式を受け取る
        variables[key] = () => FormatValue(getter());
    }

    /// <summary>
    /// 型ごとの文字列変換
    /// </summary>
    private string FormatValue<T>(T value)
    {
        return value switch
        {
            Vector3 v3 => $"({v3.x:F2}, {v3.y:F2}, {v3.z:F2})",
            Vector4 v4 => $"({v4.x:F2}, {v4.y:F2}, {v4.z:F2}, {v4.w:F2})",
            Quaternion q => $"({q.x:F2}, {q.y:F2}, {q.z:F2}, {q.w:F2})",
            float f => f.ToString("F2"),
            _ => value?.ToString()
        };
    }

    /// <summary>
    /// テキストにどんどん追加していく
    /// </summary>
    public void FixedUpdate()
    {
        if (text == null) return;

        string output = "";

        foreach (var pair in variables)
        {
            output += $"{pair.Key}: {pair.Value()}\n";
        }

        text.text = output;
    }
}

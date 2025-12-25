using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // ステートの辞書
    private Dictionary<PlayUIType, IUIState> stateDict;

    // 現在のステート
    private IUIState currentState;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        CreateStates();

        // 初期ステートを設定
        ChangeState(PlayUIType.StartCall);
    }

    /// <summary>
    /// ステート作成
    /// </summary>
    private void CreateStates()
    {
        stateDict = new Dictionary<PlayUIType, IUIState>
        {
            { PlayUIType.SelectWeapon, new SelectWeaponState(this)},
            { PlayUIType.StartCall, new StartCallState(this) },
            { PlayUIType.InPlay, new InPlayState(this) },
            { PlayUIType.KO, new KOState(this) },
            { PlayUIType.TimeUp, new TimeUpState(this) }
        };
    }


    /// <summary>
    /// ステートを切り替える
    /// </summary>
    public void ChangeState(PlayUIType type)
    {
        if (!stateDict.TryGetValue(type, out var newState))
        {
            Debug.LogError($"UIController: {type} のステートが見つかりません");
            return;
        }

        // 前のステート終了
        currentState?.Exit();

        // 新しいステートに切り替え
        currentState = newState;
        currentState.Enter();
    }


    private void Update()
    {
        currentState?.Update();
    }

    // ----------------------------------------------
    // 
    // ----------------------------------------------
}

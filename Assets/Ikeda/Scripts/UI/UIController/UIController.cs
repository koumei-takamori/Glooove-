using Nakashi.Player;
using System.Collections;
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
    private void Start()
    {
        CreateStates();

        // いきなり State 切り替えない
        StartCoroutine(WaitPlayersAndStartCall());
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


    private IEnumerator WaitPlayersAndStartCall()
    {
        yield return null;

        while (true)
        {
            var players = PlayerRegistry.Instance.GetAllPlayers();

            if (players.Count >= 2)
            {
                bool allReady = true;

                foreach (var player in players)
                {
                    if (!player.TryGetComponent(out ArmPlayerController controller) ||
                        !controller.IsInitialized)
                    {
                        allReady = false;
                        break;
                    }
                }

                if (allReady)
                {
                    break;
                }
            }
            else
            {
                Debug.Log("UIController: プレイヤー2人の初期化完了を待機中...");
            }

            yield return null;
        }

        Debug.Log("UIController: プレイヤー2人 & Start完了を確認");

        ChangeState(PlayUIType.StartCall);
    }



    private void Update()
    {
        currentState?.Update();
    }

}

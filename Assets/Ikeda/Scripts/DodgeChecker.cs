using Nakashi.Player;
using System.Collections;
using UnityEngine;

[System.Serializable]
struct dodgePlayerData
{
    public ArmPlayerController playerController;
    public StretchArm[] arms;
}

public class DodgeChecker : MonoBehaviour
{
    [SerializeField]
    private dodgePlayerData[] dodgePlayerDatas = new dodgePlayerData[2];

    private void Start()
    {
        StartCoroutine(InitializeCoroutine());
    }

    /// <summary>
    /// 初期化全体を管理するコルーチン
    /// </summary>
    private IEnumerator InitializeCoroutine()
    {
        // ① プレイヤー登録待ち
        yield return WaitUntilPlayersRegistered();

        // ② PlayerController取得
        if (!SetupPlayerControllers())
            yield break;

        // ③ StretchArm取得
        if (!SetupStretchArms())
            yield break;

        Debug.Log("DodgeChecker : 初期化完了");
    }

    /// <summary>
    /// PlayerRegistry に2人登録されるまで待機
    /// </summary>
    private IEnumerator WaitUntilPlayersRegistered()
    {
        yield return new WaitUntil(() =>
            PlayerRegistry.Instance.GetAllPlayers().Count >= 2
        );

        Debug.Log("DodgeChecker : プレイヤー2人登録確認");
    }

    /// <summary>
    /// PlayerRegistry から ArmPlayerController を取得
    /// </summary>
    private bool SetupPlayerControllers()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject playerObj = PlayerRegistry.Instance.GetPlayer(i);

            if (playerObj == null)
            {
                Debug.LogError($"DodgeChecker : Player[{i}] が取得できません");
                return false;
            }

            dodgePlayerDatas[i].playerController =
                playerObj.GetComponent<ArmPlayerController>();

            if (dodgePlayerDatas[i].playerController == null)
            {
                Debug.LogError($"DodgeChecker : Player[{i}] に ArmPlayerController がありません");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 各プレイヤーから StretchArm を取得
    /// </summary>
    private bool SetupStretchArms()
    {
        for (int i = 0; i < 2; i++)
        {
            dodgePlayerDatas[i].arms =
                dodgePlayerDatas[i].playerController.GetStretchArms();

            if (dodgePlayerDatas[i].arms == null ||
                dodgePlayerDatas[i].arms.Length < 2)
            {
                Debug.LogError($"DodgeChecker : Player[{i}] の StretchArm が不足しています");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 回避行動を行った時の処理
    /// </summary>
    public void IsDodgeCheckerAction(
        ArmPlayerController armPlayerController,
        Vector3 dodgePoint)
    {
        if (dodgePlayerDatas[0].playerController == armPlayerController)
        {
            NotifyEnemy(1, dodgePoint);
        }
        else if (dodgePlayerDatas[1].playerController == armPlayerController)
        {
            NotifyEnemy(0, dodgePoint);
        }
    }

    /// <summary>
    /// 敵プレイヤーの全腕に回避地点を通知
    /// </summary>
    private void NotifyEnemy(int enemyIndex, Vector3 dodgePoint)
    {
        foreach (var arm in dodgePlayerDatas[enemyIndex].arms)
        {
            arm.SetEnemyDodgePoint(dodgePoint);
        }
    }
}

// -------------------------------------------
// DodgeChecker.cs
// 両プレイヤーの回避を検知　相手に通知する
// Scene状にikeda/Prepfab/DodgeCheckerプレハブを配置して使用する
// 池田桜輔
// 2925/12/29
// -------------------------------------------

using Nakashi.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct dodgePlayerData
{
    public ArmPlayerController playerController;
    public StretchArm[] arms;

}

public class DodgeChecker : MonoBehaviour
{
    // プレイヤー2人分の情報
    [SerializeField] dodgePlayerData[] dodgePlayerDatas = new dodgePlayerData[2];


    void Start()
    {
        // プレイヤーの情報を取得
        for (int i = 0; i < 2; i++)
            {
            GameObject playerObj = GameObject.FindGameObjectsWithTag("Player")[i];
            if(playerObj == null)
            {
                Debug.LogError("DodgeChecker : Player[" + i + "]を取得できませんでした。　Playerに[Player]Tagをつけ忘れてる可能性があります。");
                continue;
            }
            dodgePlayerDatas[i].playerController = playerObj.GetComponent<ArmPlayerController>();
            if(dodgePlayerDatas[i].playerController == null)
            {
                Debug.LogError("DodgeChecker : Player[" + i + "]のArmPlayerControllerを取得できませんでした");
            }
        }



        // プレイヤーからグローブの情報を抽出
        

        for (int i = 0; i < 2; i++)
        {
            // prefabを取得しちゃうパターン
            // gloveData = dodgePlayerDatas[i].playerController.GetPlayerGloveData();
            //if (gloveData == null)
            //{
            //    Debug.LogError("DodgeChecker : Player[" + i + "]を取得できませんでした" );
            //    continue;
            //}
            //dodgePlayerDatas[i].Larm = gloveData.LeftGlove .GetComponent<StretchArm>();
            //dodgePlayerDatas[i].Rarm = gloveData.RightGlove.GetComponent<StretchArm>();

            // ArmPlayerControllerから直接取得
            dodgePlayerDatas[i].arms = dodgePlayerDatas[i].playerController.GetStretchArms();

            if (dodgePlayerDatas[i].arms[0] == null || dodgePlayerDatas[i].arms[1] == null)
            {
                Debug.LogError("DodgeChecker : Player[" + i + "]のStretchArmを取得できませんでした");
            }
        }

        if (dodgePlayerDatas.Length != 2)
        {
            Debug.LogError("DodgeChecker : 2プレイヤー検知できませんでした");
            return;
        }
    }

    /// <summary>
    /// 回避行動を行った時の処理
    /// </summary>
    /// <param name="armPlayerController"></param>
    public void IsDodgeCheckerAction(ArmPlayerController armPlayerController, Vector3 dodgePoint)
    {
        if (dodgePlayerDatas[0].playerController == armPlayerController)
        {
            foreach (var arm in dodgePlayerDatas[1].arms)
            {
                arm.SetEnemyDodgePoint(dodgePoint);
            }
        }
        else if (dodgePlayerDatas[1].playerController == armPlayerController)
        {
            foreach (var arm in dodgePlayerDatas[0].arms)
            {
                arm.SetEnemyDodgePoint(dodgePoint);
            }
        }

        Debug.Log("回避行動を通知 " + dodgePoint);
    }

}

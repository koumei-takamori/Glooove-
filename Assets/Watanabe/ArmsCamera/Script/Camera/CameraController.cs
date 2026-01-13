/**********************************************
 * 
 *  CameraContoller.cs 
 *  カメラの管理クラス
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CameraContoller : MonoBehaviour
{
    [SerializeField, Header("1PDeath時のタイムライン")]
    private PlayableDirector m_1PdeathTimelineData;
    [SerializeField, Header("2PDeath時のタイムライン")]
    private PlayableDirector m_2PdeathTimelineData;
    [SerializeField]
    private Camera m_1PCamera;
    [SerializeField]
    private Camera m_2PCamera;

    [SerializeField, Header("1Pのトランスフォーム")]
    private Transform m_1PTransform;
    [SerializeField, Header("2Pのトランスフォーム")]
    private Transform m_2PTransform;


    //1P用カメラオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private CinemachineCamera m_1PCmCamera;

    //２P用カメラオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private CinemachineCamera m_2PCmCamera;

    //ターゲットグループオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private GameObject m_targetGroupOb;

    //ターゲットグループスクリプト
    private CinemachineTargetGroup m_targetGroup;
    [SerializeField]
    private CinemachineCamera m_deathCamera;

    //追加 負けたプレイや番号
    private int m_losePlayerNumber = 0;



    private void Awake()
    {
        //オブジェクトからスクリプトを取得
        m_targetGroup = m_targetGroupOb.GetComponent<CinemachineTargetGroup>();
    }

    /// <summary>
    /// ターゲットグループの設定
    /// </summary>
    private void TargetGroupSetting()
    {

        m_targetGroup.Targets.Add(
            new CinemachineTargetGroup.Target
            {
                Object = m_1PTransform,
                Weight = 1.0f,
                Radius = 0.5f
            }
        );

        m_targetGroup.Targets.Add(
            new CinemachineTargetGroup.Target
            {
                Object = m_2PTransform,
                Weight = 1.0f,
                Radius = 0.5f
            }
);

    }

    public void InitCameraTargets()
    {
        //ターゲットグループの設定
        TargetGroupSetting();

        //１Pカメラの設定
        m_1PCmCamera.Follow = m_1PTransform;
        m_1PCmCamera.LookAt = m_targetGroupOb.transform;

        //２Pカメラの設定
        m_2PCmCamera.Follow = m_2PTransform;
        m_2PCmCamera.LookAt = m_targetGroupOb.transform;
    }

    public void StartDeathCamera()
    {

        if (m_losePlayerNumber == 0)
        {
            UnityEngine.Debug.Log("負けキャラなし");
        }

        //ターゲットのセット
        switch (m_losePlayerNumber)
        {
            //1PWin
            case 1:
                m_deathCamera.Target.TrackingTarget = m_2PTransform;
                //1Pカメラを広げる
                StartCoroutine(ExpandCamera(m_1PCamera, m_2PCamera, true));

                break;
            //2PWin
            case 0:
                m_deathCamera.Target.TrackingTarget = m_1PTransform;
                //２Pカメラを広げる
                StartCoroutine(ExpandCamera(m_2PCamera, m_1PCamera, false));

                break;
            default: break;
        }



    }

    /// <summary>
    /// 追加　プレイや番号のセット　＆　アニメーションのスタート
    /// </summary>
    /// <param name="playerNUmber"></param>
    public void SetLosePlayerNumber(int playerNUmber)
    {
        UnityEngine.Debug.Log(playerNUmber);

        m_losePlayerNumber = playerNUmber;

        StartDeathCamera();



    }

    // カメラの拡大
    IEnumerator ExpandCamera(Camera winner, Camera loser, bool winnerIsLeft)
    {
        float duration = 0.1f;
        float time = 0f;

        Rect winnerEnd = new Rect(0f, 0f, 1f, 1f);

        Rect loserEnd = winnerIsLeft
            ? new Rect(1f, 0f, 0f, 1f) // 右へ押し出す
            : new Rect(0f, 0f, 0f, 1f); // 左へ押し出す

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 中央固定感が出るイージング
            t = Mathf.SmoothStep(0f, 1f, t);

            if (winnerIsLeft)
            {
                // 左勝ち：左端固定で右へ広がる
                winner.rect = new Rect(
                    0f,
                    0f,
                    Mathf.Lerp(0.5f, 1f, t),
                    1f
                );

                // 敗者：中央から右へ押し出される
                loser.rect = new Rect(
                    Mathf.Lerp(0.5f, 1f, t),
                    0f,
                    Mathf.Lerp(0.5f, 0f, t),
                    1f
                );
            }
            else
            {
                // 右勝ち：右端固定で左へ広がる
                winner.rect = new Rect(
                    Mathf.Lerp(0.5f, 0f, t),
                    0f,
                    Mathf.Lerp(0.5f, 1f, t),
                    1f
                );

                // 敗者：中央から左へ押し込まれる
                loser.rect = new Rect(
                    0f,
                    0f,
                    Mathf.Lerp(0.5f, 0f, t),
                    1f
                );
            }



            yield return null;
        }


        switch (m_losePlayerNumber)
        {
            case 1:
                //タイムラインの再生
                m_1PdeathTimelineData.Play();

                break;
            case 0:
                //タイムラインの再生
                m_2PdeathTimelineData.Play();
                break;
            default:
                break;
        }

        Time.timeScale = 0.2f;
        // 最終状態
        winner.rect = winnerEnd;
        loser.enabled = false;
    }

    //所有者の取得
    public Transform Player1 { get { return m_1PTransform; } set { m_1PTransform = value; } }
    //ターゲットの取得
    public Transform Player2 { get { return m_2PTransform; } set { m_2PTransform = value; } }

}

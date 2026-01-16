/*
*   SkyChanger.cs
*   ステージに応じて背景を変更するクラス
*   制作者：熊澤圭祐
*   制作日：2026/01/14
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyChanger : MonoBehaviour
{
    // 各ステージのスカイボックス
    [SerializeField, Header("ストリートステージのスカイボックス")] private Material m_streetSkybox;
    [SerializeField, Header("ライブステージのスカイボックス")] private Material m_liveSkybox;
    [SerializeField, Header("ジャンクフードステージのスカイボックス")] private Material m_junkFoodSkybox;


    /// <summary>
    /// スカイボックスの変更
    /// </summary>
    /// <param name="stageId">ステージID</param>
    public void ChangeSkyBox(int stageId)
    {
        switch (stageId)
        {
            case 0:// ストリートステージ
                RenderSettings.skybox = m_liveSkybox;
                break;
            case 1:// ライブステージ
                RenderSettings.skybox = m_junkFoodSkybox;
                break;
            case 2:// ジャンクフードステージ
                RenderSettings.skybox = m_streetSkybox;
                break;
            default: break;
        }



    }
}
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			ChangeBlackBackGround.h
// <概要>		　　背景を完全に暗くする演出用
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBlackBackGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 環境光を黒にする
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;

        // 空の反射光をゼロにする
        RenderSettings.ambientIntensity = 0f;
        // スカイボックスの無効
        RenderSettings.skybox = null;

        // カメラの背景色を黒に
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;

        //// フォグを有効にする
        //RenderSettings.fog = true;
        //RenderSettings.fogMode = FogMode.ExponentialSquared;
        //RenderSettings.fogColor = Color.black;
        //RenderSettings.fogDensity = 0.01f;
    }
}

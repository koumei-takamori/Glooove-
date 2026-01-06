//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2026/01/06
// <file>			TitleMoveBlack
// <概要>		　　タイトルシーンの黒いやつを動かす
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleMoveBlack : MonoBehaviour
{
    RectTransform rect;

    [Header("速さ"), SerializeField] float speed = 2f;
    [Header("α最小"), SerializeField] float minScale = 0.9f;
    [Header("α最大"), SerializeField] float maxScale = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();  
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time * speed);
        float t = (sin + 1f) * 0.5f;
        float scaleY = Mathf.Lerp(minScale, maxScale, t);

        rect.localScale = new Vector3(
            rect.localScale.x,
            scaleY,
            rect.localScale.z);

    }
}

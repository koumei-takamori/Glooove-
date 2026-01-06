//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/11/18
// <file>			TitleAlphaChangeTextMeshPro
// <概要>		　　タイトルシーンのテキストメッシュプロのα値を変更するスクリプト
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleAlphaChangeTextMeshPro : MonoBehaviour
{
    TMP_Text text;
    Color baseColor;

    [Header("速さ"), SerializeField] float speed = 2f;
    [Header("α最小"), SerializeField] float minAlpha = 0.3f;
    [Header("α最大"), SerializeField] float maxAlpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        baseColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time * speed);
        float t = (sin + 1f) * 0.5f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        text.color = new Color(
            baseColor.r,
            baseColor.g,
            baseColor.b,
            alpha);
        
    }
}

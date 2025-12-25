//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			VolumetricLightSetup.h
// <概要>		　　光るライトの道筋シェーダの、変数設定用。頂点位置からz軸（Blenderの上軸)の位置によって、光の掛かり具合を変更する
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter) , typeof(MeshRenderer))]
public class VolumetricLightSetup : MonoBehaviour
{
    public Material mat;

    private void Start()
    {
        if(mat == null) { mat = this.GetComponent<MeshRenderer>().material; }

        MeshFilter mf = GetComponent<MeshFilter>();
        if(mf)
        {
            Mesh mesh = mf.sharedMesh;
            Vector3[] verts = mesh.vertices;

            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach(var v in verts)
            {
                if(v.z < minY) { minY = v.z; }
                if(v.z > maxY) { maxY = v.z; }
            }
            float height = maxY - minY;

            float fadeScaleY = 1.0f / height;
            mat.SetFloat("_FadeScaleY", fadeScaleY);
        }
    }
}

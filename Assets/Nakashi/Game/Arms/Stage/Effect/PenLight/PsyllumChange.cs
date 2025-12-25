//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/25
// <file>			PsyllumChange.h
// <概要>		　　サイリウムのアニメーション変更
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PsyllumChange : MonoBehaviour
{
    public VisualEffect vfx;

    void Start()
    {
        vfx = this.GetComponent<VisualEffect>();
    }

    public void SetParameter()
    {
        if(vfx != null)
        {
            if(vfx.GetInt("PsyliumMotionSwitch") == 0)
            {
                vfx.SetInt("PsyliumMotionSwitch", 1);
            }
            else
            {
                vfx.SetInt("PsyliumMotionSwitch", 0);
            }

        }
    }
}

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			INakashiPlayerState.h
// <概要>		　　ステートマシーンのインターフェース
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public interface INakashiPlayerState
        {
            // 入出時
            public void Enter();
            // 退出時
            public void Exit();
            // 更新
            public void Update();
            // 物理演算系との更新
            public void FixedUpdate();
        }
    }
}


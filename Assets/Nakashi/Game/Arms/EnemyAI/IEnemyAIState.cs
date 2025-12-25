//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			IEnemyAIState.h
// <概要>		　　敵AIのステートマシーンインターフェース
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace EnemyAI
    {
        public interface IEnemyAIState
        {
            // 入出時
            public void Enter();
            // 退出時
            public void Exit();
            // 更新
            public void Update();
        }

    }

}



//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			IGameState.h
// <概要>		　　ゲームステートのインターフェースクラス
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

public interface IGameState
{
    // 入出時
    public void Enter();
    // 退出時
    public void Exit();
    // 更新
    public void Update();
}


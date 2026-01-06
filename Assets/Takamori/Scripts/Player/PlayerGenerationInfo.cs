/**********************************************************
 *
 *  PlayerGenerationInfo.cs
 *  プレイヤーの生成情報
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// キャラの種類
public enum CharacterType
{
    Balance,
    Speed,
    Tank
}

/// <summary>
/// グローブの左右
/// </summary>
public enum GloveSide 
{
    Left = 0,
    Right = 1,
}

/// <summary>
/// グローブの種類
/// </summary>
public enum GloveType
{
    Normal,
    Boomerang,
    Power,
}

/// <summary>
/// プレイヤーの生成情報
/// </summary>
[System.Serializable]
public class PlayerGenerationInfo
{
    // プレイヤーID
    public int PlayerId { get; private set; } = default;
    public InputDevice PairWithDevice { get; private set; } = default;
    public CharacterType SelectedCharacter { get; private set; } = default;

    public PlayerGenerationInfo(
        InputDevice pairWithDevice, 
        CharacterType selectedCharacter)
    {
        PairWithDevice = pairWithDevice;
        SelectedCharacter = selectedCharacter;
    }
}

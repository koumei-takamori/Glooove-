using UnityEngine;

public class SelectWeaponState : IUIState
{

    private readonly UIController controller;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">UIコントローラー(stateを持つ物)</param>
    public SelectWeaponState(UIController controller)
    {
        this.controller = controller;
    }


    /// <summary>
    /// ステートin
    /// </summary>
    public void Enter()
    {
        // ステート開始時の処理
        Debug.Log("SelectWeaponState: Enter");
    }

    /// <summary>
    /// ステート更新
    /// </summary>
    public void Update()
    {
        // ステート開始時の処理
        Debug.Log("SelectWeaponState: Update");
    }

    /// <summary>
    /// ステートout
    /// </summary>
    public void Exit()
    {
        // ステート開始時の処理
        Debug.Log("SelectWeaponState: Exit");
    }
}

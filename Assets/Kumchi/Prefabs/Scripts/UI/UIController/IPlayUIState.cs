
public enum PlayUIType
{
    SelectWeapon,
    StartCall,
    InPlay,
    TimeUp,
    KO
}


public interface IUIState
{

    /// <summary>
    /// ステートin
    /// </summary>
    void Enter();


    /// <summary>
    /// ステート更新
    /// </summary>
    void Update();


    /// <summary>
    /// ステートout
    /// </summary>
    void Exit();
}


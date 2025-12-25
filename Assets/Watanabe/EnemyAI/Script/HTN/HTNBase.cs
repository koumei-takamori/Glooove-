using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// ワールドステートの基底クラス
/// </summary>
public interface IWorldState
{

}


/// <summary>
/// タスクの基底クラス
/// </summary>
public interface TaskBase
{



}

/// <summary>
/// コンパウンドタスク（分解可能タスク）
/// </summary>
public class CompoundTask :TaskBase
{

    struct Method
    {

       // private 
        private List<TaskBase> subTasks; //サブタスクリスト

    }

    //Method　　優先度が高いものほど先頭に持ってくる
    //Preconditions
    //SubTasks　優先度が高いものを先頭に


    //全てのメソッドの取得


}



/// <summary>
/// プリミティブタスク（分解不可能タスク）
/// </summary>
public class PrimitiveTask : TaskBase
{
    //Preconditions
    //Efects
    //Action
}


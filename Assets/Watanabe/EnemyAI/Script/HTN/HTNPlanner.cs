using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTNPlanner
{
    //実行タスク保管リスト
    private List<TaskBase> m_executeTask = new List<TaskBase>();
    private List<TaskBase> m_serchTaskList = new List<TaskBase>();

    /// <summary>
    /// 計画実行
    /// </summary>
    /// <param name="worldState">ワールドステート</param>
    /// <param name="rootTask">タスクリスト</param>
    /// <returns>実行タスクリスト</returns>
    public List<TaskBase> Planning(IWorldState worldState,TaskBase rootTask)
    {
        //クリア
        m_executeTask.Clear();
        m_serchTaskList.Clear();


        //ルートタスクのサブタスクを検査タスクリストに追加
        m_serchTaskList.Add(rootTask);


        //検査タスクリストがなくなるまでループ　追加順に調べる
         while (m_serchTaskList.Count > 0) {

            //検査タスクリストの先頭を取得（検査タスク）
            var serchTask = m_serchTaskList[0];
        //検査タスクがコンパウンドタスクかプリミティブタスクかの判定
            if(serchTask is CompoundTask compoundTask)
            {
                //CompoundTaskならcompoundTaskにキャスト


            
                //コンパウンドタスクの場合
                //nullチェック
                if(serchTask != null) { 

                //メソッドの取得

                //メソッドのループ
                    //前提条件の判定
                    //Trueなら
                        //サブタスクを検査タスクリストに追加　
                        //メソッドループを抜ける
                    //Falseなら
                        //次のメソッドに移る
                }
            }

        //プリミティブタスクの場合
            //nullチェック
            //前提条件の判定
            //Trueなら
                //実行タスクに追加
                
            //Falseなら
                    //特になし

        
         }




        return m_executeTask;
    }

}

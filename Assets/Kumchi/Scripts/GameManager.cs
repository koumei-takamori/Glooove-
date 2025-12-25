using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ESCキーが押された場合、アプリを終了する
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // エディタ上では実行を停止する
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドしたexeならアプリを終了する
        Application.Quit();
#endif      
        }

    }
}

/*
*   SoundTester.cs
*   適当なキーを押したときに音を鳴らすテスト用スクリプト
*   シングルトンのSoundManagerを使用する
*/
using UnityEngine;

public class SoundTester : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoundManager.Instance.PlaySE("Damage");
        }
    }
}

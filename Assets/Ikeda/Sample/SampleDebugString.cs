using UnityEngine;
// ネームスペース必要なし


public class SampleDebugString : MonoBehaviour
{
    // 対応している型式
    // [bool] [float] [int] [Vector3] [Vector4]  [Quaternion]

    bool        DebugFlag       = false;
    float       DebugFloat      = 0.0f;
    int         DebugInt        = 0;
    Vector3     DebugVector3    = new Vector3(0f, 0f, 0f);
    Vector4     DebugVector4    = new Vector4(0f, 0f, 0f, 0f);
    Quaternion  DebugQuaternion = new Quaternion(0f, 0f, 0f, 0f);


    void Start()
    {
        // 第一引数：key　第二引数：Getterのラムダ式
        DebugStringSystem.Instance.AddVariable("bool",      () => DebugFlag);
        DebugStringSystem.Instance.AddVariable("float",     () => DebugFloat);
        DebugStringSystem.Instance.AddVariable("int",       () => DebugInt);
        DebugStringSystem.Instance.AddVariable("Vector3",   () => DebugVector3);
        DebugStringSystem.Instance.AddVariable("Vector4",   () => DebugVector4);
        DebugStringSystem.Instance.AddVariable("Quaternion",() => DebugQuaternion);
    }


    void Update()
    {
        DebugFloat += Time.deltaTime;

        DebugInt = (int)DebugFloat;

        DebugFlag = DebugInt % 2 == 0;
    }
}

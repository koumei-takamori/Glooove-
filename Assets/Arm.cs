using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField]
    GameObject armObj;

    // 可変長配列
    [SerializeField]
    Vector3[] Obj;

    // 生成する個数
    [SerializeField]
    int objNum = 10;


    private void Start()
    {
        Obj = new Vector3[objNum];

        // 連立する子オブジェクトとして生成。一つ目が親。それ以降は前のオブジェクトの子として生成
        for (int i = 0; i < objNum; i++)
        {
            GameObject obj = Instantiate(armObj);
            obj.transform.parent = (i == 0) ? this.transform : armObj.transform.GetChild(i - 1);
            obj.name = "Arm_" + i;
            obj.transform.localPosition = new Vector3(0f, 0f, 1f);
            obj.transform.localRotation = Quaternion.identity;
            armObj = obj;
        }
    }

}

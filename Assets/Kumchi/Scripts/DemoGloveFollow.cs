/*
 *      DemoGloveFollow.cs
 *      伸縮用アームにアタッチし、gloveを先端に追従させるスクリプト
 *      
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DemoGloveFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject glove;    // 追従させるgloveオブジェクト
    private Transform endArm; // 伸縮用アームの先端Transform

    // Start is called before the first frame update
    void Start()
    {

        endArm = transform.GetChild(transform.childCount - 1);
        // 一番下の子オブジェクトを取得
        GetChildTransform(endArm);
        // gloveを生成
        glove = Instantiate(glove);





    }

    // 最も深い子オブジェクトを探す
    void GetChildTransform(Transform parent)
    {
        Transform deepestChildTransform = parent;
        if (parent.childCount == 0)
        {
            endArm = deepestChildTransform;
        }
        else
        {
            deepestChildTransform = deepestChildTransform.GetChild(0);
            GetChildTransform(deepestChildTransform);
        }

    }



    // Update is called once per frame
    void Update()
    {
        // gloveを先端に追従させる
        glove.transform.position = endArm.position;
        // gloveを先端の回転に追従させる
        glove.transform.rotation = endArm.rotation;

    }
}

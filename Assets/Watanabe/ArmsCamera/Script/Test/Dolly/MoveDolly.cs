using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDolly : MonoBehaviour
{
    [SerializeField]
    private CinemachineDollyCart dollyCart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // キー操作でカートを動かす
        if (Input.GetKey(KeyCode.Alpha1))
        {
            dollyCart.m_Position += Time.deltaTime * 2f; // 右で進む
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            dollyCart.m_Position -= Time.deltaTime * 2f; // 左で戻る
        }
    }
}

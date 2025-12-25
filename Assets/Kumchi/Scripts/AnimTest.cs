/*
*   AnimTest.cs
*   アニメーションテスト用スクリプト
*   BoxColliderとRigidbodyがアタッチされていることを前提とする
*   アニメーションはwaitから始まり、WASDキーで移動、スペースキーでジャンプする
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private BoxCollider boxCollider;
    // 座標
    private Vector3 pos;
    // 回転
    private Quaternion rot;
    // 移動速度
    private float moveSpeed = 7.5f;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // 座標と回転の初期値を保存
        pos = transform.position;
        rot = transform.rotation;
        // waitアニメーションに設定
        animator.SetBool("isWalk", false);
    }

    // Update is called once per frame
    void Update()
    {
        // WASDキーで移動
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, 0, moveZ);
        rb.MovePosition(transform.position + movement * Time.deltaTime * moveSpeed);
        // walkに移行
        if (movement.magnitude > 0.1f)
        {
            animator.SetBool("isWalk", true);
        }
        else
        {
            // waitに移行
            animator.SetBool("isWalk", false);
        }
        // スペースキーでジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }



    }
}

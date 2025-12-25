using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameDebugCamera : MonoBehaviour
{
    // ホイールスピード
    [SerializeField, Range(0.1f, 10f)] private float m_wheelSpeed = 1.0f;

    // 動く速度
    [SerializeField, Range(0.1f, 10f)] private float m_moveSpeed = 0.3f;

    // 回転速度
    [SerializeField, Range(0.1f, 10f)] private float m_rotateSpeed = 0.3f;

    private Vector3 m_preMousePos;

    private void Update()
    {
        MouseUpdate();
        return;
    }

    private void MouseUpdate()
    {
        float screenWheel = Input.GetAxis("Mouse ScrollWheel");
        if(screenWheel != 0.1f)
        {
            MouseWheel(screenWheel);
        }

        if(Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1) ||
            Input.GetMouseButtonDown(2))
        {
            m_preMousePos = Input.mousePosition;
        }

        MouseDrag(Input.mousePosition);
    }

    private void MouseWheel(float delta)
    {
        transform.position += transform.forward * delta * m_wheelSpeed;
        return;
    }

    private void MouseDrag(Vector3 mousePos)
    {
        Vector3 diff = mousePos - m_preMousePos;

        if(diff.magnitude < Vector3.kEpsilon) { return; }

        if(Input.GetMouseButton(2))
        {
            transform.Translate(-diff * Time.deltaTime * m_moveSpeed);
        }
        else if(Input.GetMouseButton(1))
        {
            CameraRotate(new Vector3(-diff.x, diff.x) * m_rotateSpeed);
        }

        m_preMousePos = mousePos;
    }

    public void CameraRotate(Vector2 angle)
    {
        transform.RotateAround(transform.position, transform.right, angle.x);
        transform.RotateAround(transform.position, Vector3.up, angle.y);
    }
}

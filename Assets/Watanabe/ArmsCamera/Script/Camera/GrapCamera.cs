using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapCamera : MonoBehaviour
{

    [SerializeField, Header("í èÌÉJÉÅÉâ")]
    private CinemachineVirtualCamera m_normalCamera;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrap()
    {

        Debug.Log("Grap");

        transform.position = m_normalCamera.transform.position;



    }

}

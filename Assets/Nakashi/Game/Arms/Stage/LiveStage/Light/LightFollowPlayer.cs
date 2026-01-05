using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    [Header("ÉvÉåÉCÉÑÅ["),SerializeField]
    private GameObject m_playerObj;
    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(m_playerObj.transform.position);
    }
}

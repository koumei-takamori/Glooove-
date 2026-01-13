using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTester : MonoBehaviour
{
    [SerializeField]
    private VisualEffect vfx;

    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        vfx.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("VFX Play");
            vfx.SendEvent("OnPlay");
        }
    }
}

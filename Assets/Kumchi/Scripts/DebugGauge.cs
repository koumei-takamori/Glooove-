using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGauge : MonoBehaviour
{
    public HPGaugeSystem hPGaugeSystem;

    // Update is called once per frame
    void Update()
    {
        // スペースを押すごとにHPを10ずつ減らす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hPGaugeSystem.Damage(10);
        }
    }
}

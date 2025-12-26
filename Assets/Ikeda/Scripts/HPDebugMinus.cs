using UnityEngine;

public class HPDebugMinus : MonoBehaviour
{
    public HPGaugeSystem hPGaugeSystem;

    // Update is called once per frame
    void Update()
    {
        // スペースを押すごとにHPを10ずつ減らす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hPGaugeSystem.Damage(10);
            Debug.Log("残りHP: " + hPGaugeSystem.HP);
        }
    }
}
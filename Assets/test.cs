using UnityEngine;

public class test : MonoBehaviour
{
    Transform thisT;

    // âÒì]
    [SerializeField]
    float rotSpeed = 1f;

    // êiçsë¨ìx
    [SerializeField]
    float moveSpeed = 1f;


    void Start()
    {
        thisT = this.transform;
    }


    void Update()
    {
        thisT.Rotate(new Vector3(1f, 0f, 0f) * rotSpeed * Time.deltaTime);
        thisT.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}

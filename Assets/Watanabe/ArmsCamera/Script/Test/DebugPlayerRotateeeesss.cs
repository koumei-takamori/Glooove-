using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerRotateeeesss : MonoBehaviour
{
    [SerializeField]
    Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0; // è„â∫Ç…åXÇØÇΩÇ≠Ç»Ç¢èÍçá
        transform.rotation = Quaternion.LookRotation(lookPos);
    }
}

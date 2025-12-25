using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAtTarget : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Yé≤âÒì]ÇÃÇ›Ç…êßå¿Åiè„â∫Ç…åXÇ©Ç»Ç¢Åj

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Transform m_transform;
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0; // è„â∫Ç…åXÇØÇΩÇ≠Ç»Ç¢èÍçá
        transform.rotation = Quaternion.LookRotation(lookPos);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
        }

    }

    

}

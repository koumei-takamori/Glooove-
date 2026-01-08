using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public Vector2 moveDir = new Vector2(1 ,1);
    [Header("‘¬“x"), SerializeField] private float speed = 2.0f;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(moveDir.normalized * speed * Time.deltaTime);

        // ˆê’è‹——£‚ÅŒ³‚É–ß‚·
        if (Vector3.Distance(transform.position, startPos) > 10f)
        {
            transform.position = startPos;
        }
    }
}

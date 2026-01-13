using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankUpDownObject : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float speed = 2.0f;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = startPos + new Vector3(0f, y, 0f);
    }
}

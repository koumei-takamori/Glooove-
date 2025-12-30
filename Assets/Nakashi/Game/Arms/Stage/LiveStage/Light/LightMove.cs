using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    public float angle = 30.0f;
    public float noiseSpeed = 0.2f;

    private Vector2 seed;

    private void Start()
    {
        seed.x = Random.Range(0, 1000);
        seed.y = Random.Range(0, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time * noiseSpeed;
        float x = Mathf.PerlinNoise(t + seed.x, 0f) * 2f - 1f;
        float z = Mathf.PerlinNoise(0f, t + seed.y) * 2f - 1f;

        this.transform.localRotation = Quaternion.Euler(
            x * angle,
            0f,
            z * angle);
    }
}

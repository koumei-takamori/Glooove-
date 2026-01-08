using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateXTime : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] float duration = 90f;

    [Header("Rotation X")]
    [SerializeField] float startX = 0f;
    [SerializeField] float endX = 60f;

    [Header("Fog Density")]
    [SerializeField] float startFogDensity = 0.05f;
    [SerializeField] float endFogDensity = 0.01f;

    private float elapsed = 0f;
    void Start()
    {
        // Fog ‚ð—LŒø‰»
        RenderSettings.fog = true;

        // ‰Šúó‘Ô
        Vector3 euler = transform.localEulerAngles;
        euler.x = startX;
        transform.localEulerAngles = euler;

        RenderSettings.fogDensity = startFogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsed >= duration) return;

        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);
        t = Mathf.SmoothStep(0f, 1f, t); // ’©‚Á‚Û‚­

        // ‰ñ“]
        float x = Mathf.Lerp(startX, endX, t);
        Vector3 euler = transform.localEulerAngles;
        euler.x = x;
        transform.localEulerAngles = euler;

        // Fog Density
        RenderSettings.fogDensity =
            Mathf.Lerp(startFogDensity, endFogDensity, t);
    }
}

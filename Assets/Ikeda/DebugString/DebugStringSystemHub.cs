using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStringSystemHub : MonoBehaviour
{
    [SerializeField] private string DebugStringHierarchyPath = "/Canvas/DebugString";

    /// <summary>
    /// ‰Šú‰»ˆ—
    /// </summary>
    void Start()
    {
        if (DebugStringHierarchyPath == null) DebugStringHierarchyPath = "/Canvas/DebusString";


        DebugStringSystem.Instance.Start(DebugStringHierarchyPath);
    }

    /// <summary>
    /// 60FPSXVˆ—
    /// </summary>
    private void FixedUpdate()
    {
        DebugStringSystem.Instance.FixedUpdate();
    }
}

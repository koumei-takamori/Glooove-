using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestt : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string name;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetBool(name, false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) { animator.SetBool(name, true); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState
{
    //ó‘Ô‚É“ü‚Á‚½‚ÉŒÄ‚Î‚ê‚é
    public void Enter();
    //ó‘Ô‚ğ”²‚¯‚é‚Æ‚«‚ÉŒÄ‚Î‚ê‚é
    public void Exit();
    public void Update();
}

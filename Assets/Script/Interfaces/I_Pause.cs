using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Pause
{
    public void Pause(bool state);
    public void SubscribeToPause(I_Pause ipause);
}

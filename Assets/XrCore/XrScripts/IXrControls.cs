using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IXrControls
{
    public void MoveDelta(Vector2 delta);
    public void RightDelta(Vector2 delta);

    public void TriggerRight(float value);

    public void TriggerLeft(float value);

    public void GripLeft(float value);
    public void GripRight(float value);

    

    public void OnJumpKey();

    
}

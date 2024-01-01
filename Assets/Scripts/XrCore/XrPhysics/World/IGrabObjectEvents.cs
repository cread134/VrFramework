using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabObjectEvents 
{
    public void OnGripChange(float oldValue, float newValue);
    public void OnTriggerChange(float oldValue, float newValue);
    public void OnTriggerDown();
    public void OnTriggerUp();
    public void OnMainDown();
    public void OnMainUp();
}

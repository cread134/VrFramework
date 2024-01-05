using UnityEngine;
using UnityEngine.InputSystem;

namespace XrCore.Interaction.Control
{
    [CreateAssetMenu]
    public class ControllerConfiguration : ScriptableObject
    {
        public InputActionReference mainButtonAction;
        public InputActionReference secondaryButtonAction;

        public InputActionProperty gripAction;
        public InputActionProperty triggerActionProperty;
        [Range(0f, 1f)]public float gripThreshold = 0.8f;
        [Range(0f, 1f)] public float triggerThreshold = 0.8f;
    }
}
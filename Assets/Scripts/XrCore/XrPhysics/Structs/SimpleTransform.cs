using UnityEngine;

namespace XrCore.XrPhysics.Structs
{
    public struct SimpleTransform
    {
        public SimpleTransform(Vector3 _up, Vector3 _forward, Vector3 position)
        {
            Up = _up;
            Forward = _forward;
            Position = position;
        }

        public Vector3 Up { get; }
        public Vector3 Forward { get; }
        public Vector3 Position { get; }
    }
}
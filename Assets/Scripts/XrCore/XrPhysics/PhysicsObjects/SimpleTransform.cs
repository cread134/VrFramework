using UnityEngine;

namespace XrCore.XrPhysics.PhysicsObjects
{
    public class SimpleTransform
    {
        public SimpleTransform(Vector3 _up, Vector3 _forward, Vector3 position)
        {
            Up = _up;
            Forward = _forward;
            Position = position;
            _rotation = Quaternion.identity;
        }

        public Vector3 Up { get; }
        public Vector3 Forward { get; }
        public Vector3 Position { get; }

        Quaternion _rotation;
        public Quaternion Rotation
        {
            get
            {
                return _rotation == Quaternion.identity ? 
                    Quaternion.LookRotation(Forward, Up)
                    : _rotation;
            }
        }
    }
}
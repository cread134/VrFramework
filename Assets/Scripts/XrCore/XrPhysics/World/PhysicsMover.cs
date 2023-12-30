using UnityEngine;

namespace XrCore.XrPhysics.World
{
    public class PhysicsMover
    {
        private readonly XrObjectPhysicsConfig physicsConfiguration;
        private readonly Rigidbody _rigidbody;

        public PhysicsMover(XrObjectPhysicsConfig physicsConfiguration, Rigidbody rigidbody)
        {
            this.physicsConfiguration = physicsConfiguration;
            this._rigidbody = rigidbody;
        }

        Vector3 positionStoredIntegration;
        Vector3 positionError;
        Vector3 lastPositionError;
        public void PhysicsMatchHandPosition(Vector3 targetPosition)
        {
            positionError = targetPosition - _rigidbody.position;

            Vector3 positionProportion = positionError * physicsConfiguration.positionIntegrationCompenstation;

            Vector3 derivativeGain = (positionError - lastPositionError) / Time.fixedDeltaTime;
            Vector3 positionDerivative = derivativeGain * physicsConfiguration.positionSmoothing;

            lastPositionError = positionError;

            positionStoredIntegration += (positionError * Time.fixedDeltaTime);

            Vector3 force = positionProportion + positionStoredIntegration + positionDerivative;
            // Debug.Log("pid force " + force);
            _rigidbody.AddForce(force);
        }

        private Quaternion rotationError;
        private Quaternion lastRotation;
        private float angleError;
        private Vector3 errorAxis;
        public void PhysicsMatchHandRotation(Quaternion targetRotation)
        {
            rotationError = targetRotation * Quaternion.Inverse(_rigidbody.rotation);

            rotationError.ToAngleAxis(out angleError, out errorAxis);
            errorAxis.Normalize();
            if (angleError > 180f)
            {
                angleError -= 360f;
            }

            if (Quaternion.Angle(targetRotation, _rigidbody.rotation) < physicsConfiguration.angleAllowance)
            {
                _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.deltaTime * physicsConfiguration.rotationalSmoothing));
                _rigidbody.angularVelocity = Vector3.zero;
            }
            else
            {
                Quaternion rotationImpulse = lastRotation * Quaternion.Inverse(targetRotation);
                rotationImpulse.ToAngleAxis(out float angleImpluse, out Vector3 impulseAxis);

                _rigidbody.angularVelocity *= physicsConfiguration.anglularSlowdown;

                Vector3 angularVelocity = (errorAxis * angleError) * Time.deltaTime * physicsConfiguration.rotationMultiplier;
                Vector3 angularImpulse = (impulseAxis * angleImpluse) * Time.fixedDeltaTime * physicsConfiguration.rotationImpulseCompensation;

                angularVelocity += (_rigidbody.angularVelocity - angularVelocity) * physicsConfiguration.rotationProportionalGain * Time.deltaTime;
                angularVelocity += angularImpulse;
                _rigidbody.AddTorque(angularVelocity, ForceMode.VelocityChange);
            }

            lastRotation = targetRotation;
        }
    }
}
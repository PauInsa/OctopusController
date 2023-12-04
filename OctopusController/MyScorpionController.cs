using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


namespace OctopusController
{
  
    public class MyScorpionController
    {
        //TAIL
        Transform tailTarget;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;
        bool startedMovement = false;
        Transform[] virtualTail;
        Transform virtualEndEffector;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6];

        

        #region public
        public void InitLegs(Transform[] LegRoots,Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for(int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                //TODO: initialize anything needed for the FABRIK implementation

                legTargets = LegTargets;
                legFutureBases = LegFutureBases;

                
            }

        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);

            //TODO: Initialize anything needed for the Gradient Descent implementation
            
            tailEndEffector = _tail.getEndEffector();

            for(int i = 0; i<_tail.Bones.Length; i++)
            {
                virtualTail[i].position = _tail.Bones[i].position;
                virtualTail[i].rotation = _tail.Bones[i].rotation;
            }

            virtualEndEffector.position = tailEndEffector.position;
            virtualEndEffector.rotation = tailEndEffector.rotation;
            
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
            tailTarget = target;

        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {
            startedMovement = true;
        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            updateTail();




            for (int i = 0; i < _tail.Bones.Length; i++)
            {
                _tail.Bones[i].position = virtualTail[i].position;
                _tail.Bones[i].rotation = virtualTail[i].rotation;
            }

            tailEndEffector.position = virtualEndEffector.position;
            tailEndEffector.rotation = virtualEndEffector.rotation;            
 
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position


        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {
            if(startedMovement)
            {
                float minDist = checkFuturePos(0);
                int index = 0;

                for (int i = 1; i < _tail.Bones.Length; i++)
                {
                    virtualEndEffector = tailEndEffector;
                    virtualTail = _tail.Bones;

                    float newDist = checkFuturePos(i);

                    if (minDist >= newDist)
                    {
                        minDist = newDist;
                        index = i;
                    }

                }

                virtualEndEffector = tailEndEffector;
                virtualTail = _tail.Bones;

                checkFuturePos(index);

            }

        }

        private float checkFuturePos(int i)
        {

            _tail.Bones[i].rotation += animationRange;

            Vector3 vectorToEffector = tailEndEffector.position - _tail.Bones[i].position;
            Vector3 vectorToTarget = tailTarget.position - _tail.Bones[i].position;

            Vector3 rotationAxis = Vector3.Cross(vectorToEffector, vectorToTarget).normalized;

            float angle = Vector3.Angle(vectorToEffector, vectorToTarget);

            _tail.Bones[i].Rotate(rotationAxis, angle, Space.World);

            simulateKinematics();

            float newDist = (tailEndEffector.position - tailTarget.position).magnitude;

            return newDist;
        }

        private void simulateKinematics()
        {

            Quaternion rotations = virtualTail[0].rotation;

            for (int i = 1; i < virtualTail.Length; i++)
            {
                virtualTail[i].position = virtualTail[i-1].position + rotations * virtualTail[i].position;

                rotations *= virtualTail[i].rotation;
            }

            virtualEndEffector.position = virtualTail[virtualTail.Length].position + rotations * virtualEndEffector.position;

        }

        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {
            for(int i =0; i < _legs.Length; i+=2)
            {
                //legFutureBases[i];
                //legTargets[i];

                //_legs[i].Bones[0];

            }

            for (int i = 1; i < _legs.Length; i += 2)
            {

            }

        }
        #endregion
    }
}

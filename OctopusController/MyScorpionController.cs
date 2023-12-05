using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;


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
        Vector3[] virtualTailPos;
        Quaternion[] virtualTailRot;
        Vector3 virtualEndEffectorPos;
        Quaternion virtualEndEffectorRot;

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


            virtualTailPos = new Vector3[_tail.Bones.Length];
            virtualTailRot = new Quaternion[_tail.Bones.Length];

            virtualEndEffectorPos = new Vector3();
            virtualEndEffectorRot = new Quaternion();

            for (int i = 0; i < _tail.Bones.Length; i++)
            {
                virtualTailPos[i] = _tail.Bones[i].position;
                virtualTailRot[i] = _tail.Bones[i].rotation;
            }

            virtualEndEffectorPos = tailEndEffector.position;
            virtualEndEffectorRot = tailEndEffector.rotation;
            
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
                _tail.Bones[i].position = virtualTailPos[i];
                _tail.Bones[i].rotation = virtualTailRot[i];
            }

            tailEndEffector.position = virtualEndEffectorPos;         
            tailEndEffector.rotation = virtualEndEffectorRot;         
 
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
                Vector3[] temporalTailPos = new Vector3[_tail.Bones.Length];
                Quaternion[] temporalTailRot = new Quaternion[_tail.Bones.Length];

                Vector3 temporalEndEffectorPos = new Vector3();
                Quaternion temporalEndEffectorRot = new Quaternion();


                for (int i = 0; i < _tail.Bones.Length; i++)
                {
                    for (int j = 0; j < _tail.Bones.Length; j++)
                    {
                        temporalTailPos[j] = virtualTailPos[j];
                        temporalTailRot[j] = virtualTailRot[j];
                    }

                    temporalEndEffectorPos = virtualEndEffectorPos;
                    temporalEndEffectorRot = virtualEndEffectorRot;


                    float leftDist = checkFuturePos(i, 1);

                    for (int j = 0; j < _tail.Bones.Length; j++)
                    {
                        virtualTailPos[j] = temporalTailPos[j];
                        virtualTailRot[j] = temporalTailRot[j];
                    }

                    virtualEndEffectorPos = temporalEndEffectorPos;
                    virtualEndEffectorRot = temporalEndEffectorRot;

                    float rightDist = checkFuturePos(i, -1);

                    if (leftDist < rightDist)
                    {
                        for (int j = 0; j < _tail.Bones.Length; j++)
                        {
                            virtualTailPos[j] = temporalTailPos[j];
                            virtualTailRot[j] = temporalTailRot[j];
                        }

                        virtualEndEffectorPos = temporalEndEffectorPos;
                        virtualEndEffectorRot = temporalEndEffectorRot;

                        checkFuturePos(i, 1);
                    }
                }
            }
        }

        private float checkFuturePos(int i, int r)
        {

            Vector3 vectorToTarget = virtualTailPos[i] - _tail.Bones[i].position;

            float angle = Vector3.Angle(vectorToTarget, virtualTailRot[i].eulerAngles);

            Vector3 rotationAxis = Vector3.Cross(vectorToTarget, virtualTailRot[i].eulerAngles);

            float finalAngle = angle / (virtualEndEffectorPos - tailTarget.position).magnitude;

            _tail.Bones[i].Rotate(rotationAxis, finalAngle * r, Space.World);

            simulateKinematics();

            float newDist = (virtualEndEffectorPos - tailTarget.position).magnitude;

            return newDist;
        }

        private void simulateKinematics()
        {
            Quaternion rotations = virtualTailRot[0];

            for (int i = 1; i < virtualTailPos.Length; i++)
            {
                virtualTailPos[i] = virtualTailPos[i-1] + rotations * virtualTailPos[i];

                rotations *= virtualTailRot[i];
            }

            virtualEndEffectorPos = virtualTailPos[virtualTailPos.Length-1] + rotations * virtualEndEffectorPos;

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

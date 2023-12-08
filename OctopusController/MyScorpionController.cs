using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEngine.GraphicsBuffer;


namespace OctopusController
{

    public class MyScorpionController
    {
        //TAIL
        Transform tailTarget;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6];

        bool startWalking = false;
        double timer = 0;


        #region public
        public void InitLegs(Transform[] LegRoots, Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for (int i = 0; i < LegRoots.Length; i++)
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
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {

        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {
            startWalking = true;
        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            updateLegPos();
            updateLegs();
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            if (timer > 6 || !startWalking)
            {
                return;
            }

            timer += Time.deltaTime;

            for (int i = 0; i < legFutureBases.Length; i++)
            {
                float moveAmount = 0;
                if ((i % 2) == 0)
                {
                    moveAmount = Mathf.Sin(Time.time * 10) * Time.deltaTime;
                }
                else
                {
                    moveAmount = Mathf.Cos(Time.time * 10) * Time.deltaTime;
                }

                legFutureBases[i].Translate(Vector3.forward * moveAmount);
            }
        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {

        }
        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {
            for (int i = 0; i < _legs.Length; i++)
            {
                backwardsIteration(i);
                forwardIteration(i);
            }
        }

        private void forwardIteration(int i)
        {
            _legs[i].Bones[3].position = legTargets[i].position; // Assign Effector to Leg Targets

            for (int j = 1; j < _legs[i].Bones.Length - 1; j++)
            {
                Vector3 p = _legs[i].Bones[j].position - _legs[i].Bones[j + 1].position;

                float v = _legs[i].LegDistances[j] / p.magnitude;

                Vector3 newPos = (1 - v) * _legs[i].Bones[j + 1].position + v * _legs[i].Bones[j].position;

                _legs[i].Bones[j].position = newPos;
            }
        }

        // Bones.length = 4, [3] -> EndEffector

        private void backwardsIteration(int i)
        {
            _legs[i].Bones[0].position = legFutureBases[i].position; // Assign Joint0 to Future Base

            for (int j = _legs[i].Bones.Length - 1; j > 1; j--)
            {
                Vector3 p = _legs[i].Bones[j - 1].position - _legs[i].Bones[j].position;

                float v = _legs[i].LegDistances[j - 1] / p.magnitude;

                Vector3 newPos = (1 - v) * _legs[i].Bones[j - 1].position + v * _legs[i].Bones[j].position;

                _legs[i].Bones[j].position = newPos;
            }
        }

        #endregion
    }
}

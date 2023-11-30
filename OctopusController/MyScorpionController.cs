using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;

        MyTentacleController[] _legs = new MyTentacleController[6];

        Vector3[,] temPos;

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

            temPos = new Vector3[_legs.Length, _legs[0].Bones.Length];

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

        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            updateLegs();
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

        }
        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {
            
            for(int i = 0; i < _legs.Length; i++)
            {
                //if (_legs[i].LegDistances[2] < _legs[i].LegTotalDistance)
                //{
                    //Debug.Log("ENTERING BACKWARDS");
                    
                    backwardsIteration(i);
                    //Debug.Log("ENTERING FORWARDS");

                    forwardIteration(i);
                //}
               // else
                //{
                   // Debug.Log(i+ "_LEG NOT IN REACH");
               // }
                   

            }


            for (int i = 0; i < _legs.Length; i++)
            {

                _legs[i].Bones[0] = legFutureBases[i];

                for (int j = 1; j < _legs[0].Bones.Length-1; j++)
                {

                    _legs[i].Bones[j].position = temPos[i, j];

                    Debug.DrawRay(_legs[i].Bones[j].position, (_legs[i].Bones[j].position - _legs[i].Bones[j+1].position));

                }
                _legs[i].Bones[3] = legTargets[i];
            }

            


        }

        private void forwardIteration(int i)
        {

            for (int j = 1; j < _legs[i].Bones.Length-1; j++)
            {
                //Debug.Log(j);

                Vector3 p = _legs[i].Bones[j+1].position - _legs[i].Bones[j].position;

                Vector3 v = _legs[i].LegDistances[j] * p.normalized;

                Vector3 newPos = _legs[i].Bones[j].position + v ;

                temPos[i, j] = newPos;
            }


        }

        // Bones.length = 4, [3] -> EndEffector

        private void backwardsIteration(int i)
        {

            for (int j = _legs[i].Bones.Length-1; j > 1; j--)
            {
                //Debug.Log(j);

                Vector3 p = _legs[i].Bones[j - 1].position - _legs[i].Bones[j].position;

                //Debug.Log("j: " + j + " j-1: " + (j - 1));

                Vector3 v = _legs[i].LegDistances[j - 1] * p.normalized;

                Vector3 newPos = _legs[i].Bones[j].position + v;

                temPos[i, j] = newPos;
            }


        }
        #endregion
    }
}

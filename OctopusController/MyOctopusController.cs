using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace OctopusController
{
    public enum TentacleMode { LEG, TAIL, TENTACLE };

    public class MyOctopusController 
    {
        
        MyTentacleController[] _tentacles =new  MyTentacleController[4];

        Transform _currentRegion;
        Transform _target;

        Transform[] _randomTargets;// = new Transform[4];


        float _twistMin, _twistMax;
        float _swingMin, _swingMax;

        bool isBallShoot = false;
        Transform[] allRegions;


        #region public methods
        //DO NOT CHANGE THE PUBLIC METHODS!!

        public float TwistMin { set => _twistMin = value; }
        public float TwistMax { set => _twistMax = value; }
        public float SwingMin {  set => _swingMin = value; }
        public float SwingMax { set => _swingMax = value; }
        

        public void TestLogging(string objectName)
        {

           
            Debug.Log("hello, I am initializing my Octopus Controller in object "+objectName);

            
        }

        public void Init(Transform[] tentacleRoots, Transform[] randomTargets)
        {
            _tentacles = new MyTentacleController[tentacleRoots.Length];

            // foreach (Transform t in tentacleRoots)
            for(int i = 0;  i  < tentacleRoots.Length; i++)
            {

                _tentacles[i] = new MyTentacleController();
                _tentacles[i].LoadTentacleJoints(tentacleRoots[i],TentacleMode.TENTACLE);
                //TODO: initialize any variables needed in ccd
            }

            _randomTargets = randomTargets;
            //TODO: use the regions however you need to make sure each tentacle stays in its region


            
        }

              
        public void NotifyTarget(Transform target, Transform region)
        {
            _currentRegion = region;
            _target = target;
        }

        public void NotifyShoot() {
            //TODO. what happens here?
            Debug.Log("Shoot");
            isBallShoot = true;
        }


        public void UpdateTentacles()
        {
            //TODO: implement logic for the correct tentacle arm to stop the ball and implement CCD method
            for (int i = 0; i < _tentacles.Length; i++)
            {
                update_ccd(i);
            }
            
        }




        #endregion


        #region private and internal methods
        //todo: add here anything that you need
        void update_ccd(int i) 
        {
            if (!isBallShoot)
            {
                RandomTarget(i);
            }
            else
            {
                LockToTarget(i);
            }
        }   


        void LockToTarget(int i)
        {
            if (CheckRegionIndex() != i)
            {
                //Debug.Log("ENTERING IF LOCK TO TARGET");
                RandomTarget(i);
                return;
            }
            //Debug.Log("ENTERING LOCK TO TARGET");
            for (int j = _tentacles[i].Bones.Length - 2; j >= 0; j--)
            {
                Vector3 vectorToEffector = _tentacles[i].EndEffector.position - _tentacles[i].Bones[j].position;
                Vector3 vectorToTarget = _target.position - _tentacles[i].Bones[j].position;

                Vector3 rotationAxis = Vector3.Cross(vectorToEffector, vectorToTarget).normalized;

                float angle = Vector3.Angle(vectorToEffector, vectorToTarget);

                _tentacles[i].Bones[j].Rotate(rotationAxis, angle, Space.World);
            }
        }

        void RandomTarget(int i)
        {
            for (int j = _tentacles[i].Bones.Length - 2; j >= 0; j--)
            {
                Vector3 vectorToEffector = _tentacles[i].EndEffector.position - _tentacles[i].Bones[j].position;
                Vector3 vectorToTarget = _randomTargets[i].position - _tentacles[i].Bones[j].position;

                Vector3 rotationAxis = Vector3.Cross(vectorToEffector, vectorToTarget).normalized;

                float angle = Vector3.Angle(vectorToEffector, vectorToTarget);

                _tentacles[i].Bones[j].Rotate(rotationAxis, angle, Space.World);
            }
        }

        int CheckRegionIndex()
        {
            return _currentRegion.GetSiblingIndex()-1;
        }

        #endregion
    }
}

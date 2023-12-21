using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;




namespace OctopusController
{

    
    internal class MyTentacleController

    //MAINTAIN THIS CLASS AS INTERNAL
    {
        TentacleMode tentacleMode;
        Transform[] _bones;
        Transform _endEffectorSphere;
        float _legTotalDistance = 0.0f;
        float[] _legDistances;

        

        public Transform[] Bones { get => _bones; }
        public float LegTotalDistance { get => _legTotalDistance; }
        public float[] LegDistances { get => _legDistances; }

        public Transform EndEffector { get => _endEffectorSphere; }

        List<Transform> tempList;

        //Exercise 1.
        public Transform[] LoadTentacleJoints(Transform root, TentacleMode mode)
        {
            _legDistances = new float[3];
            tempList = new List<Transform>();
            //TODO: add here whatever is needed to find the bones forming the tentacle for all modes
            //you may want to use a list, and then convert it to an array and save it into _bones
            tentacleMode = mode;

            switch (tentacleMode){
                case TentacleMode.LEG:
                    //TODO: in _endEffectorsphere you keep a reference to the base of the leg

                    Transform node = root.GetChild(0);

                    tempList.Add(node);

                    for(int i = 1; i < 3; i++)
                    {
                        node = node.GetChild(1);
                        tempList.Add(node);
                    }

                    // Distance between joints and total distance
                    for (int i = 0; i < tempList.Count-1; i++)
                    {
                        _legDistances[i] = (tempList[i].position - tempList[i+1].position).magnitude;
                        _legTotalDistance += LegDistances[i];
                    }

                    _bones = tempList.ToArray();
                    
                    _endEffectorSphere = node.GetChild(1);
                    break;
                case TentacleMode.TAIL:
                    //TODO: in _endEffectorsphere you keep a reference to the red sphere
                    
                    Transform node1 = root;

                    tempList.Add(node1); // Joint 0

                    for (int i = 1; i < 5; i++) // Joint 1 -- Joint 4
                    {
                        node1 = node1.GetChild(1);
                        tempList.Add(node1);
                    }

                    _bones = tempList.ToArray();

                    _endEffectorSphere = node1.GetChild(1);
                    break;
                case TentacleMode.TENTACLE:
                    //TODO: in _endEffectorphere you  keep a reference to the sphere with a collider attached to the endEffector
                    
                    // 49 Iteracions per arribar a Bone.001
                    // 50 Iteracions per arribar a Bone.001_end
                    // 51 Iteracions per arribar a BallRegion

                    Transform node2 = root; //Tentacle_name

                    node2 = node2.GetChild(0); // Armature
                    tempList.Add(node2);

                    node2 = node2.GetChild(0); // Bone
                    tempList.Add(node2);

                    for (int i = 0; i < 50; i++) // Bone.050 -- Bone.001_end
                    {
                        node2 = node2.GetChild(0);
                        tempList.Add(node2);
                    }

                    _bones = tempList.ToArray();

                    node2 = node2.GetChild(0);
                    _endEffectorSphere = node2; // BallRegion 

                    break;
            }
            return Bones;
        }
    }
}

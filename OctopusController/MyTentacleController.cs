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

        public Transform[] Bones { get => _bones; }

        //Exercise 1.
        public Transform[] LoadTentacleJoints(Transform root, TentacleMode mode)
        {
            //TODO: add here whatever is needed to find the bones forming the tentacle for all modes
            //you may want to use a list, and then convert it to an array and save it into _bones
            tentacleMode = mode;

            switch (tentacleMode){
                case TentacleMode.LEG:
                    //TODO: in _endEffectorsphere you keep a reference to the base of the leg

                    Transform node = root.GetChild(0);

                    Bones[0] = root.GetChild(0);

                    for(int i =1; i< 3; i++)
                    {
                        
                        Bones[i] = node.GetChild(1);
                        node = node.GetChild(1);
                    }

                    _endEffectorSphere = node.GetChild(1);


                    break;
                case TentacleMode.TAIL:
                    //TODO: in _endEffectorsphere you keep a reference to the red sphere
                    
                    Transform node1 = root;
                    Bones[0] = root; // Joint 0

                    for (int i = 1; i < 5; i++) // Joint 1 -- Joint 4
                    {
                        node1 = node1.GetChild(1);
                        Bones[i] = node1;
                    }

                    _endEffectorSphere = node1.GetChild(1);



                    break;
                case TentacleMode.TENTACLE:
                    //TODO: in _endEffectorphere you  keep a reference to the sphere with a collider attached to the endEffector
                    
                    // 49 Iteracions per arribar a Bone.001
                    // 50 Iteracions per arribar a Bone.001_end
                    // 51 Iteracions per arribar a BallRegion

                    Transform node2 = root; //Tentacle_name

                    node2 = node2.GetChild(0); // Armature
                    Bones[0] = node2;

                    node2 = node2.GetChild(0); // Bone
                    Bones[1] = node2; 
                    
                    for (int i = 2; i < 53; i++) // Bone.050 -- Bone.001_end
                    {
                        node2 = node2.GetChild(0);
                        Bones[i] = node2;
                    }

                    node2 = node2.GetChild(0);
                    _endEffectorSphere = node2; // BallRegion 

                    break;
            }
            return Bones;
        }
    }
}

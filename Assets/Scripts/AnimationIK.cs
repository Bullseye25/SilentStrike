using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class AnimationIK : MonoBehaviour
{


    protected Animator animator;
    public bool ikActive = false;
    public IKPointsClass IKPoints;

    Transform chest;
    public Transform target;
    public Vector3 offset;
   




    [System.Serializable]
    public class IKPointsClass
    {
        public Transform rightHand, leftHand;
      
    }




    void OnEnable()
    {


        animator = GetComponent<Animator>();
        chest = animator.GetBoneTransform(HumanBodyBones.Neck);



    }



    private void LateUpdate()
    {
        chest.LookAt(target);
        chest.rotation = chest.rotation * Quaternion.Euler(offset);
    }



    //a callback for calculating IK
    void OnAnimatorIK(int layerIndex)
    {


      //  Debug.Log("ON IK");

        if (animator.enabled != true) return;




        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {


                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);



                //animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
                //animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);

                //animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
                //animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);




                if (IKPoints.leftHand != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, IKPoints.leftHand.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, IKPoints.leftHand.rotation);
                }

                //set the position and the rotation of the right hand where the external object is
                if (IKPoints.rightHand != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightHand, IKPoints.rightHand.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, IKPoints.rightHand.rotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
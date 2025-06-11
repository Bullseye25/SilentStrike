using UnityEngine;

public class IkHandsWithGun : MonoBehaviour
{
    public Animator animator;
    public Transform rightHandPosition, leftHandPosition;
    
    // A callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            // Right Hand IK
            if (rightHandPosition)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPosition.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPosition.rotation);
            }

            // Left Hand IK
            if (leftHandPosition)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosition.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPosition.rotation);
            }
        }
    }

}

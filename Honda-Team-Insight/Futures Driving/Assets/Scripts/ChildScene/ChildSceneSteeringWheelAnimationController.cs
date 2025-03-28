using System.Collections;
using UnityEngine;

public class ChildSceneSteeringWheelAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    private int animationID = 0; // Default AnimationID

    public void SetAnimationID(int id)
    {
        animationID = id;
        animator.SetInteger("AnimationID", animationID);
    }

    public void SetAnimationIDToZero()
    {
        SetAnimationID(0);
    }

    public int getAnimationID()
    {
        return animationID;
    }
}

using System.Collections;
using UnityEngine;

public class SteeringWheelAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    [SerializeField] GameObject retractableTray;
    private int animationID = 0; // Default AnimationID
    private bool isAnimating = false; // Track if a coroutine is already running

    void Start()
    {
        retractableTray.transform.localPosition = new Vector3(-1.045f, 0.908502f, -0.4100003f);
    }

    void Update()
    {
        if (animationID == 1 && !isAnimating)
        {
            // Animate tray to extended position
            if (retractableTray != null)
            {
                StartCoroutine(MoveTrayToPosition(new Vector3(-0.585f, retractableTray.transform.localPosition.y, retractableTray.transform.localPosition.z), 1.0f));
            }
        }
        else if (animationID == 0 && !isAnimating)
        {
            // Animate tray back to retracted position
            if (retractableTray != null)
            {
                StartCoroutine(MoveTrayToPosition(new Vector3(-1.045f, retractableTray.transform.localPosition.y, retractableTray.transform.localPosition.z), 1.0f));
            }
        }
    }

    // Coroutine to animate the retractable tray
    public IEnumerator MoveTrayToPosition(Vector3 targetPosition, float duration)
    {
        isAnimating = true; // Mark animation as running

        // Wait for "StayingRetreated" animation state if moving to extended position
        if (targetPosition.x > -0.8f)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("StayingRetreated"))
            {
                yield return null;
            }
        }

        float time = 0;
        Vector3 startPosition = retractableTray.transform.localPosition;

        while (time < duration)
        {
            retractableTray.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        retractableTray.transform.localPosition = targetPosition;
        isAnimating = false; // Mark animation as finished
    }

    public void SetAnimationID(int id)
    {
        animationID = id;
        animator.SetInteger("AnimationID", animationID);
    }

    public void SetAnimationIDToOne()
    {
        SetAnimationID(1);
        Debug.Log("SetAnimationIDToOne");
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

using UnityEngine;

public class JournalAnimationHandler : MonoBehaviour
{

    public void HandleJournalAnimation()
    {
        Animator journalAnimator = GetComponent<Animator>();

        if (journalAnimator != null)
        {
            journalAnimator.SetInteger("AnimationID", 1);
            Debug.Log("Set AnimationID to 1");

            StartCoroutine(ResetAnimationWhenClosed(journalAnimator));
        }
        else
        {
            Debug.LogError("No Animator component found on journalAnimationTrigger!");
        }
    }

    private System.Collections.IEnumerator ResetAnimationWhenClosed(Animator animator)
    {
        yield return new WaitForSeconds(1f);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedBook"))
        {
            yield return null; // Wait for the next frame
        }

        animator.SetInteger("AnimationID", 0);
        Debug.Log("Set AnimationID back to 0");
    }
}

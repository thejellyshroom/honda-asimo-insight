using System.Collections;
using UnityEngine;

public class BirthdayCelebration : MonoBehaviour
{
    [SerializeField] GameObject celebrationParticles;
    [SerializeField] GameObject car;

    public void CelebrateBirthday()
    {
        // Play birthday celebration audio
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getCelebrateBdaySpeech(), 1, 0.5f);

        // Play birthday music and show celebration particles
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getBirthdayMusic(), 1, ChildSceneAudio.Instance.getCelebrateBdaySpeech().length + 0.5f);
        Invoke("ShowConfetti", ChildSceneAudio.Instance.getCelebrateBdaySpeech().length + 0.5f);
        Invoke("ShowConfetti", ChildSceneAudio.Instance.getCelebrateBdaySpeech().length + 4.5f);
        Invoke("ShowConfetti", ChildSceneAudio.Instance.getCelebrateBdaySpeech().length + 8.5f);
    }

    public float getCelebrateBirthdayLength()
    {
        float length = ChildSceneAudio.Instance.getCelebrateBdaySpeech().length + ChildSceneAudio.Instance.getBirthdayMusic().length + 0.5f;
        return length;
    }

    public void ShowConfetti()
    {
        // confetti particles are created inside the moving car
        GameObject confetti = Instantiate(celebrationParticles, car.transform);
        confetti.transform.localPosition = Vector3.zero;

        Destroy(confetti, 10.0f);
    }

    public Vector3 getCarPosition()
    {
        return car.transform.position;
    }
}

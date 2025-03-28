using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] private AudioSource soundObj;
    [SerializeField] AudioClip onboarding;
    [SerializeField] AudioClip personalizationPrompt;
    [SerializeField] AudioClip yesPlease;
    [SerializeField] AudioClip upliftingMusic;
    [SerializeField] AudioClip approachingSchool;
    private ChoiceAppear choiceAppearInstance;

    private int voiceSequence = 0;
    void Start()
    {
        choiceAppearInstance = FindFirstObjectByType<ChoiceAppear>();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlaySound(onboarding, 1, 3f);
        voiceSequence++;
        //show choices after the onboarding audio clip ends
        Invoke("ShowChoices", onboarding.length + 3.2f);
    }
    //play the onboarding audio clip when it starts after a 3 second delay
    public void PlaySound(AudioClip clip, float volume, float delay)
    {
        AudioSource audioSource = Instantiate(soundObj, Vector3.zero, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.PlayDelayed(delay);
        float clipLength = clip.length + delay;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void ShowChoices()
    {
        if (choiceAppearInstance != null)
        {
            choiceAppearInstance.ShowChoices("Confirm Driver", "Switch Driver");
        }
    }

    public AudioClip getYesPlease()
    {
        return yesPlease;
    }

    public AudioClip getPersonalizationPrompt()
    {
        return personalizationPrompt;
    }

    public AudioClip getUpliftingMusic()
    {
        return upliftingMusic;
    }

    public AudioClip getApproachingSchool()
    {
        return approachingSchool;
    }

    public int getVoiceSequence()
    {
        return voiceSequence;
    }

    public AudioClip getOnboard()
    {
        return onboarding;
    }

    public void incrementVoiceSequence()
    {
        voiceSequence++;
    }
}

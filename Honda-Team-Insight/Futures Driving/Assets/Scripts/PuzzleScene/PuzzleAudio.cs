
using UnityEngine;

public class PuzzleAudio : MonoBehaviour
{
    public static PuzzleAudio Instance;
    [SerializeField] private AudioSource soundObj;
    [SerializeField] AudioClip onboarding;
    [SerializeField] AudioClip goToCafe;
    [SerializeField] AudioClip finishedAssembly;
    [SerializeField] AudioClip broughtMemories;
    [SerializeField] AudioClip saveConfig;
    [SerializeField] AudioClip music;
    private ChoiceAppear choiceAppearInstance;

    private int voiceSequence = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlaySound(onboarding, 1, 3f);
        voiceSequence++;

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

    public float getOnboardingLength()
    {
        return onboarding.length;
    }

    public AudioClip getGoToCafe()
    {
        return goToCafe;
    }

    public AudioClip getFinishedAssembly()
    {
        return finishedAssembly;
    }

    public AudioClip getBroughtMemories()
    {
        return broughtMemories;
    }

    public AudioClip getSaveConfig()
    {
        return saveConfig;
    }

    public AudioClip getMusic()
    {
        return music;
    }
}
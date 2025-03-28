using UnityEngine;
using TMPro;

public class ChildSceneAudio : MonoBehaviour
{
    public static ChildSceneAudio Instance;
    [SerializeField] private AudioSource soundObj;
    [SerializeField] AudioClip trustModeActivated;
    [SerializeField] AudioClip onboarding;
    [SerializeField] AudioClip ofCourse;
    [SerializeField] AudioClip thankYou;
    [SerializeField] AudioClip greetChildren;
    [SerializeField] AudioClip mentionBirthday;
    [SerializeField] AudioClip celebrateBirthdaySpeech;
    [SerializeField] AudioClip birthdayMusic;

    [Header("Conversation Audio Clips")]
    [SerializeField] AudioClip tryStartConversation;
    [SerializeField] AudioClip shutDownConversation;
    [SerializeField] AudioClip aidConversation;
    [SerializeField] AudioClip continueConversation;
    [SerializeField] AudioClip parentBond;
    [SerializeField] AudioClip suggestToPlay;
    [SerializeField] AudioClip okay;

    [Header("Ai Scheduler Audio Clips")]
    [SerializeField] AudioClip aiScheduler;
    [SerializeField] AudioClip thankAsimo;

    [SerializeField] TextMeshProUGUI windshieldUIText;

    private ChoiceAppear choiceAppearInstance;

    private int voiceSequence = 0;
    private GameObject windshieldUI;
    void Start()
    {
        choiceAppearInstance = FindFirstObjectByType<ChoiceAppear>();
        windshieldUI = windshieldUIText.transform.parent.gameObject;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlaySound(onboarding, 1, 3f);
        PlaySound(ofCourse, 3, onboarding.length + 3.5f);
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
        windshieldUIText.text = "Change the materials to the ones saved in\"Child Mode\"?";
        windshieldUI.SetActive(true);
        if (choiceAppearInstance != null)
        {
            choiceAppearInstance.ShowChoices("Yes", "No");
        }
    }

    public AudioClip getThanks()
    {
        return thankYou;
    }

    public AudioClip getTrustModeActivated()
    {
        return trustModeActivated;
    }

    public AudioClip getAiScheduler()
    {
        return aiScheduler;
    }

    public AudioClip getThankAsimo()
    {
        return thankAsimo;
    }

    public AudioClip getChildrenGreeting()
    {
        return greetChildren;
    }

    public AudioClip getParentBond()
    {
        return parentBond;
    }

    public AudioClip getBirthdayMention()
    {
        return mentionBirthday;
    }

    public AudioClip getCelebrateBdaySpeech()
    {
        return celebrateBirthdaySpeech;
    }

    public AudioClip getBirthdayMusic()
    {
        return birthdayMusic;
    }

    public AudioClip getTryStartConversation()
    {
        return tryStartConversation;
    }

    public AudioClip getShutDownConversation()
    {
        return shutDownConversation;
    }

    public AudioClip getAidConversation()
    {
        return aidConversation;
    }

    public AudioClip getContinueConversation()
    {
        return continueConversation;
    }

    public int getVoiceSequence()
    {
        return voiceSequence;
    }

    public void incrementVoiceSequence()
    {
        voiceSequence++;
    }

    public AudioClip getSuggestToPlay()
    {
        return suggestToPlay;
    }

    public AudioClip getOkay()
    {
        return okay;
    }
}

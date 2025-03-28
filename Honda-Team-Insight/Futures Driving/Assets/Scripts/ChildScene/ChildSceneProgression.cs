using HMI.UI.Cluster.Car;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChildSceneProgression : MonoBehaviour
{
    [SerializeField] GameObject animationHandler;

    [SerializeField] GameObject chooseYes;
    [SerializeField] GameObject windshieldUI;
    [SerializeField] TextMeshProUGUI windshieldUIText;

    RaycastInteractable raycastInteractableScript;
    ChangeToPersonalized changeToPersonalizedScript;
    ChoiceAppear choiceAppearScript;
    ChildSceneSteeringWheelAnimationController steeringWheelAnimationController;
    ChildSceneDriveCar driveCarScript;
    ChildImageAppear childImageAppearScript;
    UIChanges UIscript;
    BirthdayCelebration birthdayCelebrationScript;

    [SerializeField] GameObject[] children;

    private bool trustMode = false;

    void Start()
    {
        HideWindshieldUI();
        changeToPersonalizedScript.ChangeToPersonalizedMaterials();

        UpdateWindshieldText("Additional passengers detected.");
        ApplyWindowTint();

        Invoke("ShowWindshieldUI", 1.0f);
        Invoke("HideWindshieldUI", 3.7f);
    }

    void Awake()
    {
        raycastInteractableScript = this.gameObject.GetComponent<RaycastInteractable>();
        changeToPersonalizedScript = this.gameObject.GetComponent<ChangeToPersonalized>();
        choiceAppearScript = this.gameObject.GetComponent<ChoiceAppear>();
        UIscript = this.gameObject.GetComponent<UIChanges>();

        steeringWheelAnimationController = animationHandler.GetComponent<ChildSceneSteeringWheelAnimationController>();
        birthdayCelebrationScript = animationHandler.GetComponent<BirthdayCelebration>();
        driveCarScript = animationHandler.GetComponent<ChildSceneDriveCar>();
        childImageAppearScript = animationHandler.GetComponent<ChildImageAppear>();
    }

    void Update()
    {
        raycastInteractableScript.HandleRaycast();

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject == chooseYes)
            {
                HandleChooseYes();
            }
        }
    }

    private void HandleChooseYes()
    {
        int voiceSequence = ChildSceneAudio.Instance.getVoiceSequence();

        if (voiceSequence == 1)
        {
            AfterChangeToChildFriendly();
            changeToPersonalizedScript.ChangeToChildFriendlyMaterials();
        }
        else if (voiceSequence == 2)
        {
            EnterChildMode();
        }
        else if (voiceSequence == 3)
        {
            StartDriveToHome();
        }

        Debug.Log($"Voice sequence: {voiceSequence}");
    }

    private void AfterChangeToChildFriendly()
    {
        windshieldUI.SetActive(false);
        choiceAppearScript.HideChoices();

        // child thanks mom for changing the materials
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getThanks(), 1, 1.0f);

        ChildSceneAudio.Instance.incrementVoiceSequence();

        // ui offers to change ai assistant to child friendly mode
        Invoke("AskChangePersonalityForChildren", ChildSceneAudio.Instance.getThanks().length);
    }

    private void AskChangePersonalityForChildren()
    {
        windshieldUIText.text = "Alex and Hayden recognized as passengers. Do you want me to change my voice and personality to be more child-friendly?";
        windshieldUI.SetActive(true);

        choiceAppearScript.ShowChoices("Yes", "No");
    }

    private void EnterChildMode()
    {
        // User chooses to enter child mode and AI greets children
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getChildrenGreeting(), 1, 0.5f);

        ChildSceneAudio.Instance.incrementVoiceSequence(); // Increment to 3

        //windshield asks if user wants to enter trust mode for the drive back to the house
        UpdateWindshieldText("Start drive back to house in trust mode?");
        choiceAppearScript.ShowChoices("Yes", "No");

        //after greeting, child responds to AI
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getBirthdayMention(), 1, ChildSceneAudio.Instance.getChildrenGreeting().length + 0.5f);

        //after child responds, AI celebrates birthday
        birthdayCelebrationScript.Invoke("CelebrateBirthday", ChildSceneAudio.Instance.getBirthdayMention().length +
            ChildSceneAudio.Instance.getChildrenGreeting().length + 1.0f);

        //after birthday celebration, parent and child have a conversation
        Invoke("ParentChildConversation", birthdayCelebrationScript.getCelebrateBirthdayLength() + ChildSceneAudio.Instance.getBirthdayMention().length +
            ChildSceneAudio.Instance.getChildrenGreeting().length + 2.0f);
    }

    private void StartDriveToHome()
    {
        UpdateWindshieldText("Currently in school zone. Please drive manually. Trust mode will be activated once you leave the school zone.");
        choiceAppearScript.HideChoices();
        UIscript.ChangeMaterials(5, 0, 0);

        Invoke("HideWindshieldText", 8.0f);
        driveCarScript.Invoke("DriveToHouse", 6.0f);

        //then invoke trust mode after another 13 seconds
        Invoke("EnterTrustMode", 13.0f);
    }

    private void ParentChildConversation()
    {
        float waitTime = 0.5f;
        // Parent and child have a conversation
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getTryStartConversation(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getTryStartConversation().length;
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getShutDownConversation(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getShutDownConversation().length;

        //ai helps facilitate
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getAidConversation(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getAidConversation().length;
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getContinueConversation(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getContinueConversation().length;
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getParentBond(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getParentBond().length;

        //suggest to play
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getSuggestToPlay(), 1, waitTime + 2.0f);
        waitTime += ChildSceneAudio.Instance.getSuggestToPlay().length + 2.0f;

        //okay
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getOkay(), 1, waitTime);
        waitTime += ChildSceneAudio.Instance.getOkay().length;

        //ai scheduler
        Invoke("AiScheduler", waitTime);
    }
    private void AiScheduler()
    {
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        UpdateWindshieldText("Children not detected. Changing personality and voice back to Sam's default mode.");
        Invoke("HideWindshieldText", 5.0f);

        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getAiScheduler(), 1, 5.0f);
        childImageAppearScript.Invoke("ShowCalendar", 8.0f);

        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getThankAsimo(), 1, ChildSceneAudio.Instance.getAiScheduler().length + 5.5f);
    }

    private void EnterTrustMode()
    {
        trustMode = true;
        UpdateWindshieldText("Left school zone. Trust mode activated.");
        ChildSceneAudio.Instance.PlaySound(ChildSceneAudio.Instance.getTrustModeActivated(), 0.5f, 0.5f);
        steeringWheelAnimationController.SetAnimationID(1);

        //hide text after 5 seconds
        Invoke("HideWindshieldText", 5.0f);
    }


    void UpdateWindshieldText(string newText)
    {
        if (windshieldUIText != null)
        {
            windshieldUIText.text = newText;
        }
    }

    void HideWindshieldText()
    {
        if (windshieldUIText != null)
        {
            windshieldUIText.text = "";
        }
    }

    public void ShowWindshieldUI()
    {
        windshieldUI.SetActive(true);
    }

    public void HideWindshieldUI()
    {
        windshieldUI.SetActive(false);
    }

    public void ApplyWindowTint()
    {
        changeToPersonalizedScript.ApplyWindowTint();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}

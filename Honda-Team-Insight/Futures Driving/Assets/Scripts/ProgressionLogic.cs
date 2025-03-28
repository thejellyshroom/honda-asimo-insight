using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressionLogic : MonoBehaviour
{
    [SerializeField] GameObject animationHandler;
    [SerializeField] GameObject carInteriorSceneTrigger;
    [SerializeField] GameObject journalAnimationTrigger;
    [SerializeField] GameObject chooseYes;
    [SerializeField] GameObject windshieldUI;
    [SerializeField] TextMeshProUGUI windshieldUIText;

    DecorateCar decorationScript;
    RaycastInteractable raycastInteractableScript;
    ChangeToPersonalized changeToPersonalizedScript;
    ChoiceAppear choiceAppearScript;
    SteeringWheelAnimationController steeringWheelAnimationController;
    DriveCar driveCarScript;
    JournalAnimationHandler journalAnimationHandler;
    ImageAppear imageAppearScript;
    UIChanges UIscript;

    private bool trustMode = false;

    void Start()
    {
        decorationScript = this.gameObject.GetComponent<DecorateCar>();
        raycastInteractableScript = this.gameObject.GetComponent<RaycastInteractable>();
        changeToPersonalizedScript = this.gameObject.GetComponent<ChangeToPersonalized>();
        choiceAppearScript = this.gameObject.GetComponent<ChoiceAppear>();
        UIscript = this.gameObject.GetComponent<UIChanges>();

        steeringWheelAnimationController = animationHandler.GetComponent<SteeringWheelAnimationController>();
        driveCarScript = animationHandler.GetComponent<DriveCar>();
        journalAnimationHandler = animationHandler.GetComponent<JournalAnimationHandler>();
        imageAppearScript = animationHandler.GetComponent<ImageAppear>();
        HideWindshieldUI();
    }

    void Awake()
    {
        // Assign initial text
        UpdateWindshieldText("High UV rays detected. Applying tint to windows.");

        // Show UI after audio
        Invoke("ShowWindshieldUI", 1.0f);
        Invoke("ApplyWindowTint", 3.0f);
        Invoke("HideWindshieldUI", 3.7f);
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
            else if (clickedObject == carInteriorSceneTrigger)
            {
                ChangeScene("Scene2_CarInterior");
            }
            else if (clickedObject == journalAnimationTrigger)
            {
                journalAnimationHandler.HandleJournalAnimation();
            }
            else
            {
                HandleDecorationClick(clickedObject);
            }
        }
    }

    private void HandleChooseYes()
    {
        int voiceSequence = AudioController.Instance.getVoiceSequence();

        if (voiceSequence == 1)
        {
            HandlePersonalizationPrompt();
            UIscript.ChangeMaterials(1, 0, 0);
        }
        else if (voiceSequence == 2)
        {
            // HandleTrustModeSequence();
        }
        else if (voiceSequence == 3)
        {
            HandlePhotoPrompt();

            //hide windshield text and choices
            HideWindshieldText();
            choiceAppearScript.HideChoices();
        }

        Debug.Log($"Voice sequence: {voiceSequence}");
    }

    private void HandlePersonalizationPrompt()
    {
        changeToPersonalizedScript.ChangeToPersonalizedMaterials();
        choiceAppearScript.HideChoices();

        // Ask if the user wants to enter trust mode
        AudioController.Instance.PlaySound(AudioController.Instance.getPersonalizationPrompt(), 1, 1.5f);

        AudioController.Instance.incrementVoiceSequence();

        // Play voice command after the personalization prompt audio clip ends
        AudioController.Instance.PlaySound(AudioController.Instance.getYesPlease(), 1, AudioController.Instance.getPersonalizationPrompt().length + 2.0f);
        Invoke("ScreenAskToTrustUI", AudioController.Instance.getPersonalizationPrompt().length);

        //Invoke the trust sequence
        Invoke("HandleTrustModeSequence", AudioController.Instance.getPersonalizationPrompt().length + AudioController.Instance.getYesPlease().length + 3.0f);
    }
    private void ScreenAskToTrustUI()
    {
        UIscript.ChangeMaterials(2, 0, 0);
    }

    private void HandleTrustModeSequence()
    {
        // User chooses to enter trust mode
        steeringWheelAnimationController.SetAnimationID(1);
        choiceAppearScript.HideChoices();
        UIscript.ChangeMaterials(3, 0, 0);

        // Play music and increment voice sequence
        AudioController.Instance.PlaySound(AudioController.Instance.getUpliftingMusic(), 1, 0.5f);
        driveCarScript.DriveToSchool();

        // Destroy audio clip after 15 seconds and switch to school zone notification
        Destroy(FindFirstObjectByType<AudioSource>().gameObject, 23);

        AudioController.Instance.PlaySound(AudioController.Instance.getApproachingSchool(), 1, 23.5f);

        // Reset steering wheel after trust mode sequence
        steeringWheelAnimationController.Invoke("SetAnimationIDToZero", 23.5f);
        Invoke("ArriveAtSchoolUI", 25.0f);

        AudioController.Instance.incrementVoiceSequence(); // Increment to 3

        //switch to children scene after 35 seconds
        Invoke("SwitchToChildrenScene", 35.0f);

        // Update UI text and show choices for photo prompt
        Invoke("ShowWindshieldUI", 1.0f);
        UpdateWindshieldText("Do you want me to pull up some photos you like to look at often?");

        Invoke("ChoiceToPullUpPhotos", 1.5f);

        // if animation id of steering wheel is set back to 0, hide choices and windshield ui
        //will need to check recurringly
        InvokeRepeating("CheckSteeringWheelAnimationID", 1.5f, 1.0f);

    }
    private void ChoiceToPullUpPhotos()
    {
        choiceAppearScript.ShowChoices("Yes", "No");
    }
    private void ArriveAtSchoolUI()
    {
        UIscript.ChangeMaterials(4, 0, 0);
    }
    private void SwitchToChildrenScene()
    {
        ChangeScene("Scene3_Children");
    }

    private void CheckSteeringWheelAnimationID()
    {
        if (steeringWheelAnimationController.getAnimationID() == 0)
        {
            CancelInvoke("CheckSteeringWheelAnimationID");
            imageAppearScript.StopImageSwitching();

            HideWindshieldUI();
            choiceAppearScript.HideChoices();
        }
    }

    private void HandlePhotoPrompt()
    {
        imageAppearScript.StartImageSwitching();

        // Hide windshield text and choices
        HideWindshieldText();
        choiceAppearScript.HideChoices();

        Debug.Log("Started image switching");
    }

    private void HandleDecorationClick(GameObject clickedObject)
    {
        if (clickedObject == decorationScript.getDecoration1())
        {
            decorationScript.Decorate(decorationScript.getDecoration1(), new Vector3(-0.791f, 1.233f, 0.116f));
        }
        else if (clickedObject == decorationScript.getDecoration2())
        {
            decorationScript.Decorate(decorationScript.getDecoration2(), new Vector3(-0.69f, 1.248f, -0.531f));
        }
        else if (clickedObject == decorationScript.getDecoration3())
        {
            decorationScript.Decorate(decorationScript.getDecoration3(), new Vector3(-0.814f, 1.241f, -0.447f));
        }
        else if (clickedObject == decorationScript.getDecoration4())
        {
            decorationScript.Decorate(decorationScript.getDecoration4(), new Vector3(-0.723f, 1.242f, 0.35f));
        }
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

    public GameObject getJournalAnimationTrigger()
    {
        return journalAnimationTrigger;
    }
}

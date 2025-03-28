using UnityEngine;
using HMI.UI.Cluster.Car;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PuzzleSceneProgression : MonoBehaviour
{

    [SerializeField] GameObject animationHandler;

    [SerializeField] GameObject windshieldUI;
    [SerializeField] TextMeshProUGUI windshieldUIText;

    RaycastInteractable raycastInteractableScript;
    SteeringWheelAnimationController steeringWheelAnimationController;
    DriveCar driveCarScript;
    [SerializeField] TextMeshProUGUI heartrateText;
    private float heartrateUpdateTimer = 0f;
    private float heartrateUpdateInterval = 1f;

    [SerializeField] GameObject retractableTray;

    [SerializeField] GameObject[] children;

    private bool assemblyStarted = false;

    void Start()
    {
        heartrateText.text = "86";

        HideWindshieldUI();
        retractableTray.transform.localPosition = new Vector3(-1.045f, 0.908502f, -0.4100003f);

        //tray pulls out and starts assembly
        steeringWheelAnimationController.Invoke("SetAnimationIDToOne", PuzzleAudio.Instance.getOnboardingLength() + 2.0f);
        Invoke("HeadToCafe", PuzzleAudio.Instance.getOnboardingLength() + 5.0f);
    }

    void Awake()
    {
        raycastInteractableScript = this.gameObject.GetComponent<RaycastInteractable>();

        steeringWheelAnimationController = animationHandler.GetComponent<SteeringWheelAnimationController>();
        driveCarScript = animationHandler.GetComponent<DriveCar>();
    }

    void HeadToCafe()
    {
        PuzzleAudio.Instance.PlaySound(PuzzleAudio.Instance.getGoToCafe(), 1, 0.0f);
        PuzzleAudio.Instance.PlaySound(PuzzleAudio.Instance.getMusic(), 0.4f, 5.0f);
        driveCarScript.Invoke("DriveToSchool", PuzzleAudio.Instance.getGoToCafe().length + 1.0f);

        Invoke("AffirmativeUI", PuzzleAudio.Instance.getGoToCafe().length + 1.0f);

        assemblyStarted = true;
    }

    void AffirmativeUI()
    {
        UpdateWindshieldText("Cafe found! Starting drive to 3Catea.");
        Invoke("HideWindshieldUI", 3.5f);
    }

    void Update()
    {
        raycastInteractableScript.HandleRaycast();

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        //update each second
        heartrateUpdateTimer += Time.deltaTime;
        if (heartrateUpdateTimer >= heartrateUpdateInterval)
        {
            heartrateUpdateTimer = 0f;

            if (assemblyStarted)
            {
                LowerHeartrate();
            }
            else
            {
                UpdateHeartrate();
            }
        }

    }

    private void UpdateHeartrate()
    {
        //random heartrate range around 80s
        int heartrate = Random.Range(80, 86);
        heartrateText.text = heartrate.ToString();
    }

    private void LowerHeartrate()
    {
        //each second, heartrate decreases by 0, 1, or 2
        int decrease = Random.Range(0, 3);
        int currentHeartrate = int.Parse(heartrateText.text);
        int newHeartrate = Mathf.Max(currentHeartrate - decrease, 60); // Prevent going below 60
        heartrateText.text = newHeartrate.ToString();
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

        }
    }


    void UpdateWindshieldText(string newText)
    {
        if (windshieldUI.activeSelf == false)
        {
            ShowWindshieldUI();
        }

        windshieldUIText.text = newText;
    }

    public void ShowWindshieldUI()
    {
        windshieldUI.SetActive(true);
    }

    public void HideWindshieldUI()
    {
        windshieldUI.SetActive(false);
    }

}

using System.Collections;
using UnityEngine;

public class AssembleShip : MonoBehaviour
{
    RaycastInteractable raycastInteractableScript;
    private Vector3 assembledPositions;
    private Quaternion assembledRotations;
    ChangeMaterials changeMaterialsScript;

    bool assemblyComplete;
    bool soundPlayed = false;
    [SerializeField] GameObject[] partsForAssembly;
    [SerializeField] GameObject ship;
    [SerializeField] GameObject savedConfigs;
    private Coroutine hovering;
    private bool allSoundsFinished = false;

    void Awake()
    {
        assemblyComplete = false;
        soundPlayed = false;
        assembledPositions = new Vector3(0.0f, 0.0f, 0.0f);
        assembledRotations = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        raycastInteractableScript = this.gameObject.GetComponent<RaycastInteractable>();
        changeMaterialsScript = FindFirstObjectByType<ChangeMaterials>();

        allSoundsFinished = false;
        savedConfigs.SetActive(false);
    }

    void Update()
    {
        raycastInteractableScript.HandleRaycast();

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        if (assemblyComplete)
        {
            if (!soundPlayed)
            {
                float waitTime = 0.0f;
                Debug.Log("Assembly complete - Attempting to change materials");
                changeMaterialsScript.Invoke("ChangeToWater", 13.0f);

                PuzzleAudio.Instance.PlaySound(PuzzleAudio.Instance.getFinishedAssembly(), 1, 1.0f);
                waitTime += PuzzleAudio.Instance.getFinishedAssembly().length + 1.0f;
                PuzzleAudio.Instance.PlaySound(PuzzleAudio.Instance.getBroughtMemories(), 1, waitTime);
                waitTime += PuzzleAudio.Instance.getBroughtMemories().length + 1.0f;
                PuzzleAudio.Instance.PlaySound(PuzzleAudio.Instance.getSaveConfig(), 1, waitTime);
                waitTime += PuzzleAudio.Instance.getSaveConfig().length + 1.0f;

                Invoke("FinishAllSounds", waitTime);
                soundPlayed = true;
            }

            if (hovering == null)
            {
                hovering = StartCoroutine(AnimateShipMovement());
            }
        }
    }

    private void FinishAllSounds()
    {
        StartCoroutine(AnimateShipToSaveConfig());

    }

    private IEnumerator AnimateShipMovement()
    {
        // First animate the ship to the target position
        float moveDuration = 3.0f;
        float elapsedTime = 0.0f;
        Vector3 startPosition = ship.transform.localPosition;
        Quaternion startRotation = ship.transform.localRotation;
        Vector3 targetPosition = new Vector3(0.00178f, -0.00096f, 0.00447f);
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        while (elapsedTime < moveDuration)
        {
            ship.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            ship.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ship.transform.localPosition = targetPosition;
        ship.transform.localRotation = targetRotation;

        // Now start the continuous rotation
        StartCoroutine(ContinuousRotation());
    }

    private IEnumerator ContinuousRotation()
    {
        float rotationSpeed = 20.0f; // Degrees per second

        while (true) // Rotate indefinitely
        {
            ship.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator AnimateShipToSaveConfig()
    {
        savedConfigs.SetActive(true);

        float moveDuration = 3.0f;
        float elapsedTime = 0.0f;
        Vector3 startPosition = ship.transform.localPosition;
        Quaternion startRotation = ship.transform.localRotation;
        Vector3 targetPosition = new Vector3(-0.000292f, -0.004566f, 0.001939f);
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);

        while (elapsedTime < moveDuration)
        {
            ship.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            ship.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ship.transform.localPosition = targetPosition;
        ship.transform.localRotation = targetRotation;
        ship.transform.localScale = targetScale;
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.CompareTag("Interactable"))
            {
                clickedObject.transform.localPosition = assembledPositions;
                clickedObject.transform.localRotation = assembledRotations;

                // Check if ALL parts are in the correct position
                bool allPartsAssembled = true;
                foreach (GameObject part in partsForAssembly)
                {
                    if (part.transform.localPosition != assembledPositions)
                    {
                        allPartsAssembled = false;
                        break;
                    }
                }

                assemblyComplete = allPartsAssembled;
            }
        }
    }
}

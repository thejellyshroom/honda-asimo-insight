using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageAppear : MonoBehaviour
{
    [Header("Image Prefab and Container")]
    [SerializeField] GameObject imagePrefab; // Prefab for UI Image
    [SerializeField] Transform imageContainer; // Parent container for images

    [Header("Sprites")]
    [SerializeField] Sprite[] photoSprites; // Array of sprites to display
    [SerializeField] float imageScale = 0.004f; // Scale factor for the image


    private GameObject currentImage; // Reference to the currently displayed image
    private int currentIndex = 0; // Index of the current sprite in the array
    private Coroutine imageSwitchingCoroutine; // Reference to the coroutine
    private Vector3 spawnPosition;

    void Start()
    {
        imageScale = 0.004f;
        spawnPosition = new Vector3(0.27f, 0.15f, 0);
    }

    public void StartImageSwitching()
    {
        if (imageSwitchingCoroutine == null)
        {
            imageSwitchingCoroutine = StartCoroutine(SwitchImages());
        }
    }

    public void StopImageSwitching()
    {
        if (imageSwitchingCoroutine != null)
        {
            StopCoroutine(imageSwitchingCoroutine);
            imageSwitchingCoroutine = null;

            // Clear the last displayed image
            if (currentImage != null)
            {
                Destroy(currentImage);
                currentImage = null;
            }
        }
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            // Clear the previous image
            if (currentImage != null)
            {
                Destroy(currentImage);
            }

            // Instantiate a new image prefab
            currentImage = Instantiate(imagePrefab, imageContainer);
            currentImage.transform.localPosition = spawnPosition;

            // Set the sprite for the current index
            Image imageComponent = currentImage.GetComponent<Image>();
            if (imageComponent != null && photoSprites.Length > 0)
            {
                imageComponent.sprite = photoSprites[currentIndex];
            }
            else
            {
                Debug.LogError("Image component missing on prefab or no sprites available!");
                yield break;
            }

            // Optionally adjust size or layout dynamically here
            RectTransform rectTransform = currentImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = new Vector3(imageScale, imageScale, 1f);
            }

            // Wait for 2-3 seconds before switching to the next image
            yield return new WaitForSeconds(2.5f);

            // Move to the next sprite in the array (loop back to start if at the end)
            currentIndex = (currentIndex + 1) % photoSprites.Length;
        }
    }
}

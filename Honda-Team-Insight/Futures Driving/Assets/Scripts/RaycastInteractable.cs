using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class RaycastInteractable : MonoBehaviour
{
    private GameObject lastHoveredObject; // To track the last hovered object
    private TMP_Text lastHoveredText; // To track the last hovered TextMeshPro component
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>(); // Store original materials
    [SerializeField] Material interactableMaterial; // The material to highlight interactable objects
    [SerializeField] Color textHighlightColor = Color.yellow; // Highlight color for TextMeshPro

    public void HandleRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject currentObject = hit.collider.gameObject;

            if (currentObject.CompareTag("Interactable"))
            {
                // Check if we are hovering over a different object
                if (lastHoveredObject != null && lastHoveredObject != currentObject)
                {
                    ResetLastHoveredMaterial(); // Reset material of the previously hovered object
                }

                TMP_Text tmpText = currentObject.GetComponent<TMP_Text>();

                if (tmpText != null) // Check for TextMeshPro component
                {
                    ResetLastHoveredMaterial(); // Reset material of previous object before highlighting text
                    HighlightText(tmpText);
                    lastHoveredText = tmpText;
                    lastHoveredObject = null; // Clear material-related hover tracking
                }
                else
                {
                    ResetLastHoveredText(); // Reset text color of previous object before highlighting material
                    HighlightMaterial(currentObject);
                    lastHoveredObject = currentObject; // Track the current object as the last hovered one
                    lastHoveredText = null; // Clear text-related hover tracking
                }
            }
            else
            {
                ResetLastHoveredMaterial();
                ResetLastHoveredText();
            }
        }
        else
        {
            ResetLastHoveredMaterial();
            ResetLastHoveredText();
        }
    }

    private void HighlightMaterial(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Save original materials only once for each renderer
            if (!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials[renderer] = renderer.materials.Clone() as Material[];
            }

            // Apply interactable material to all submeshes
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = interactableMaterial;
            }

            renderer.materials = newMaterials;
        }
    }

    private void HighlightText(TMP_Text tmpText)
    {
        tmpText.color = textHighlightColor; // Change to highlight color
    }

    private void ResetLastHoveredMaterial()
    {
        if (lastHoveredObject != null)
        {
            Renderer[] renderers = lastHoveredObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                if (originalMaterials.ContainsKey(renderer))
                {
                    renderer.materials = originalMaterials[renderer]; // Restore original materials
                    originalMaterials.Remove(renderer); // Remove this renderer's entry from the dictionary
                }
            }

            lastHoveredObject = null; // Clear reference to last hovered object
        }
    }

    private void ResetLastHoveredText()
    {
        if (lastHoveredText != null)
        {
            lastHoveredText.color = Color.white; // Restore default text color or use a stored original color
            lastHoveredText = null; // Clear reference to last hovered text object
        }
    }
}

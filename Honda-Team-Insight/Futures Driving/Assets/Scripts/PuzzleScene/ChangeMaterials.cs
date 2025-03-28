using UnityEngine;

public class ChangeMaterials : MonoBehaviour
{
    [SerializeField] GameObject cabin;

    [SerializeField] GameObject steeringWheel;

    [Header("Water Materials")]
    [SerializeField] Material mainWater;
    [SerializeField] Material accentWater;
    [SerializeField] Material seatsWater;

    private Material originalMain;
    private Material originalAccent;
    private Material originalSeats;

    [SerializeField] GameObject particleEffect;


    private Material glassMaterial;

    void Start()
    {
        //save current materials of all gameobjects
        SaveOriginalMaterials();
        particleEffect.SetActive(false);
    }


    public void ChangeToWater()
    {
        // Get the current materials array from the cabin's renderer
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        Material[] cabinMaterials = cabinRenderer.materials;

        // Modify specific elements in the materials array
        if (cabinMaterials.Length >= 3) // Ensure there are at least 3 material slots
        {
            cabinMaterials[0] = mainWater;    // Main material in slot 0
            cabinMaterials[1] = accentWater;  // Accent material in slot 1
            cabinMaterials[2] = seatsWater;   // Seats material in slot 2

            // Log to verify material assignment
            Debug.Log("Changing cabin materials to water materials");
        }
        else
        {
            Debug.LogError("Not enough material slots on cabin. Expected at least 3, found " + cabinMaterials.Length);
        }

        // Reassign the modified materials array back to the renderer
        cabinRenderer.materials = cabinMaterials;

        // Change materials for other objects directly
        steeringWheel.GetComponent<Renderer>().material = mainWater;
        //get all children of steering wheel and change their materials
        Renderer[] steeringWheelChildren = steeringWheel.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in steeringWheelChildren)
        {
            //if materials.length is less than 2, change material and continue. if not, change first material to dash, then second material to personalized buttons
            if (childRenderer.materials.Length < 2)
            {
                childRenderer.material = mainWater;
            }
            else
            {
                Material[] childMaterials = childRenderer.materials;
                childMaterials[0] = mainWater;
                childMaterials[1] = accentWater;
                childRenderer.materials = childMaterials;
            }
        }

        particleEffect.SetActive(true);
    }


    private void SaveOriginalMaterials()
    {
        // Save the original materials of the cabin
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        Material[] cabinMaterials = cabinRenderer.materials;

        if (cabinMaterials.Length >= 3)
        {
            originalMain = cabinMaterials[0];    // Main material in slot 0
            originalAccent = cabinMaterials[1];  // Accent material in slot 1
            originalSeats = cabinMaterials[2];   // Seats material in slot 2

            Debug.Log("Original materials saved");
        }
        else
        {
            Debug.LogError("Not enough material slots on cabin. Expected at least 3, found " + cabinMaterials.Length);
        }
    }
}

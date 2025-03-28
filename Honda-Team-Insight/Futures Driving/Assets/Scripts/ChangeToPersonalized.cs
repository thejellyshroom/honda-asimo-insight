using UnityEngine;

public class ChangeToPersonalized : MonoBehaviour
{
    [SerializeField] GameObject cabin;

    [SerializeField] GameObject steeringWheel;

    [Header("Glass Material")]
    [SerializeField] GameObject carBody;

    [Header("Personalized Materials")]
    [SerializeField] Material personalizedButtons;
    [SerializeField] Material personalizedDash;
    [SerializeField] Material personalizedDoors;
    [SerializeField] Material personalizedSeats;


    [Header("Children Materials")]
    [SerializeField] Material childrenButtons;
    [SerializeField] Material childrenDash;
    [SerializeField] Material childrenDoors;
    [SerializeField] Material childrenSeats;

    private Material originalButtons;
    private Material originalDash;
    private Material originalDoors;
    private Material originalSeats;


    private Material glassMaterial;

    void Start()
    {
        //save current materials of all gameobjects
        SaveOriginalMaterials();
    }

    void Awake()
    {
        //get glass material, which is element 7 of the materials array
        glassMaterial = carBody.GetComponent<Renderer>().materials[7];
    }

    public void ChangeToPersonalizedMaterials()
    {
        // Get the current materials array from the cabin's renderer
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        Material[] cabinMaterials = cabinRenderer.materials;

        // Modify specific elements in the materials array
        if (cabinMaterials.Length >= 4) // Ensure there are at least 4 material slots
        {
            cabinMaterials[0] = personalizedSeats;
            cabinMaterials[1] = personalizedDoors;
            cabinMaterials[2] = personalizedButtons;
            cabinMaterials[3] = personalizedDash;
        }

        // Reassign the modified materials array back to the renderer
        cabinRenderer.materials = cabinMaterials;

        // Change materials for other objects directly

        steeringWheel.GetComponent<Renderer>().material = personalizedDash;
        //get all children of steering wheel and change their materials
        Renderer[] steeringWheelChildren = steeringWheel.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in steeringWheelChildren)
        {
            //if materials.length is less than 2, change material and continue. if not, change first material to dash, then second material to personalized buttons
            if (childRenderer.materials.Length < 2)
            {
                childRenderer.material = personalizedDash;
            }
            else
            {
                Material[] childMaterials = childRenderer.materials;
                childMaterials[0] = personalizedButtons;
                childMaterials[1] = personalizedDash;
                childRenderer.materials = childMaterials;
            }
        }
    }

    public void ChangeToChildFriendlyMaterials()
    {
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        Material[] cabinMaterials = cabinRenderer.materials;

        // Modify specific elements in the materials array
        if (cabinMaterials.Length >= 4) // Ensure there are at least 4 material slots
        {
            cabinMaterials[0] = childrenSeats;
            cabinMaterials[1] = childrenDoors;
            cabinMaterials[2] = childrenButtons;
            cabinMaterials[3] = childrenDash;
        }

        // Reassign the modified materials array back to the renderer
        cabinRenderer.materials = cabinMaterials;

        steeringWheel.GetComponent<Renderer>().material = childrenDash;
        //get all children of steering wheel and change their materials
        Renderer[] steeringWheelChildren = steeringWheel.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in steeringWheelChildren)
        {
            Material[] childMaterials = childRenderer.materials;
            // Check if the material array has enough elements before accessing them
            if (childMaterials.Length > 0)
            {
                childMaterials[0] = childrenButtons;

                if (childMaterials.Length > 1)
                {
                    childMaterials[1] = childrenDash;
                }

                childRenderer.materials = childMaterials;
            }
        }
    }

    private void SaveOriginalMaterials()
    {
        // Save the original materials of the cabin
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        originalSeats = cabinRenderer.materials[0];
        originalDoors = cabinRenderer.materials[1];
        originalButtons = cabinRenderer.materials[2];
        originalDash = cabinRenderer.materials[3];
    }

    public void ResetMaterialsToDefault()
    {
        // Reset the materials of the cabin
        Renderer cabinRenderer = cabin.GetComponent<Renderer>();
        cabinRenderer.materials[0] = originalSeats;
        cabinRenderer.materials[1] = originalDoors;
        cabinRenderer.materials[2] = originalButtons;
        cabinRenderer.materials[3] = originalDash;

        // Reset the materials of the steering wheel
        steeringWheel.GetComponent<Renderer>().material = originalDash;
        //get all children of steering wheel and change their materials
        Renderer[] steeringWheelChildren = steeringWheel.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in steeringWheelChildren)
        {
            //if materials.length is less than 2, change material and continue. if not, change first material to dash, then second material to personalized buttons
            if (childRenderer.materials.Length < 2)
            {
                childRenderer.material = originalDash;
            }
            else
            {
                Material[] childRendererMaterials = childRenderer.materials;
                childRendererMaterials[0] = originalButtons;
                childRendererMaterials[1] = originalDash;
                childRenderer.materials = childRendererMaterials;
            }
        }
    }

    public void ApplyWindowTint()
    {
        if (glassMaterial != null)
        {
            // Set Render Face to "Both" (Cull Off)
            glassMaterial.SetFloat("_Cull", 0);
            Debug.Log("Window tint applied: Render Face set to Both");
        }
        else
        {
            Debug.LogError("Glass material not found!");
        }
    }

    public void ResetWindowTint()
    {
        if (glassMaterial != null)
        {
            // Reset Render Face to default (Cull Back)
            glassMaterial.SetFloat("_Cull", 1);
            Debug.Log("Window tint reset: Render Face set to Back");
        }
        else
        {
            Debug.LogError("Glass material not found!");
        }
    }
}

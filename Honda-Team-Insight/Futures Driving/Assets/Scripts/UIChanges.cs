using UnityEngine;

public class UIChanges : MonoBehaviour
{
    [SerializeField] GameObject iviScreen;
    [SerializeField] GameObject climateScreen;
    [SerializeField] GameObject clusterScreen;

    [SerializeField] Material[] personalizedClimateScreens;
    [SerializeField] Material[] personalizedClusterScreens;
    [SerializeField] Material[] personalizedIviScreens;

    private Material originalClimateScreen;
    private Material originalClusterScreen;
    private Material originalIviScreen;
    void Start()
    {
        SaveOriginalMaterials();
    }

    public void ChangeMaterials(int ivi_index, int cluster_index, int climate_index)
    {
        if (ivi_index < personalizedIviScreens.Length)
        {
            iviScreen.GetComponent<Renderer>().material = personalizedIviScreens[ivi_index];
            Debug.Log("Changed ivi screen to index: " + ivi_index);
        }

        if (cluster_index < personalizedClusterScreens.Length)
        {
            clusterScreen.GetComponent<Renderer>().material = personalizedClusterScreens[cluster_index];
            Debug.Log("Changed cluster screen to index: " + cluster_index);
        }

        if (climate_index < personalizedClimateScreens.Length)
        {
            climateScreen.GetComponent<Renderer>().material = personalizedClimateScreens[climate_index];
            Debug.Log("Changed climate screen to index: " + climate_index);
        }

    }

    private void SaveOriginalMaterials()
    {
        // Save the original materials of the climate screen
        originalClimateScreen = climateScreen.GetComponent<Renderer>().material;

        // Save the original materials of the cluster screen
        originalClusterScreen = clusterScreen.GetComponent<Renderer>().material;

        // Save the original materials of the ivi screen
        originalIviScreen = iviScreen.GetComponent<Renderer>().material;
    }

    public void ResetMaterialsToDefault()
    {

        // Reset the materials of the climate screen
        climateScreen.GetComponent<Renderer>().material = originalClimateScreen;

        // Reset the materials of the cluster screen
        clusterScreen.GetComponent<Renderer>().material = originalClusterScreen;

        // Reset the materials of the ivi screen
        iviScreen.GetComponent<Renderer>().material = originalIviScreen;
    }
}

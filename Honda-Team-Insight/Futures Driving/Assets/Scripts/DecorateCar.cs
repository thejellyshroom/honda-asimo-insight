using UnityEngine;

public class DecorateCar : MonoBehaviour
{
    [SerializeField] GameObject carInterior;
    [SerializeField] GameObject carTray;
    [SerializeField] GameObject carDecoration1;
    [SerializeField] GameObject carDecoration2;
    [SerializeField] GameObject carDecoration3;
    [SerializeField] GameObject carDecoration4;

    private Vector3 decoration1Position;
    private Vector3 decoration2Position;
    private Vector3 decoration3Position;
    private Vector3 decoration4Position;

    void Start()
    {
        // Define initial positions for Tray decorations
        decoration1Position = new Vector3(0.001619302f, -0.001620842f, -0.0001176105f);
        decoration2Position = new Vector3(0.003240635f, 0.001488443f, -0.0001308462f);
        decoration3Position = new Vector3(0.002020677f, 0.001579031f, -8.14934e-05f);
        decoration4Position = new Vector3(0.002849729f, -0.00185257f, -8.546263e-05f);

        // Initialize decorations in Tray
        carDecoration1.transform.localPosition = decoration1Position;
        carDecoration2.transform.localPosition = decoration2Position;
        carDecoration3.transform.localPosition = decoration3Position;
        carDecoration4.transform.localPosition = decoration4Position;
    }

    public void Decorate(GameObject clickedObject, Vector3 position)
    {
        Transform currentParent = clickedObject.transform.parent;

        if (currentParent == carTray.transform)
        {
            // Move from Tray to CarInterior
            clickedObject.transform.SetParent(carInterior.transform);
            clickedObject.transform.localPosition = position;
        }
        else if (currentParent == carInterior.transform)
        {
            // Move from CarInterior back to Tray
            clickedObject.transform.SetParent(carTray.transform);

            if (clickedObject == carDecoration1)
            {
                clickedObject.transform.localPosition = decoration1Position;
            }
            else if (clickedObject == carDecoration2)
            {
                clickedObject.transform.localPosition = decoration2Position;
            }
            else if (clickedObject == carDecoration3)
            {
                clickedObject.transform.localPosition = decoration3Position;
            }
            else if (clickedObject == carDecoration4)
            {
                clickedObject.transform.localPosition = decoration4Position;
            }
        }
    }

    public GameObject getDecoration1()
    {
        return carDecoration1;
    }

    public GameObject getDecoration2()
    {
        return carDecoration2;
    }

    public GameObject getDecoration3()
    {
        return carDecoration3;
    }

    public GameObject getDecoration4()
    {
        return carDecoration4;
    }
}

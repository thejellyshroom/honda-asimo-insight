using System.Collections;
using UnityEngine;

public class DriveCar : MonoBehaviour
{
    [SerializeField] private GameObject car;

    void Start()
    {
        car.transform.position = new Vector3(-3.479f, 0.1494979f, 29.06f);
    }

    public void DriveToSchool()
    {
        //moves to specific vector3 position throughout 30 seconds
        StartCoroutine(MoveCarToPosition(new Vector3(-3.479f, 0.1494979f, -23f), 30.0f));
    }

    public IEnumerator MoveCarToPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = car.transform.position;

        while (time < duration)
        {
            car.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        car.transform.position = targetPosition;
    }
}

using UnityEngine;

public class MainMenuCameraPan : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float rotationSpeed = 5f;
    public float verticalOffset = 2f;

    private float angle = 0f;

    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians) * distance;
        float z = Mathf.Cos(radians) * distance;
        Vector3 newPosition = new Vector3(x, verticalOffset, z) + target.position;
        transform.position = newPosition;
        transform.LookAt(target.position);
    }
}
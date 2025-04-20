using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;
    public float height = 50f;
    public float rotationAngle = 90f;

    void LateUpdate()
    {
        Vector3 pos = player.position;
        pos.y = height;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rotationAngle, player.eulerAngles.y, 0);
    }
}

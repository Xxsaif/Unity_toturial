using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;
    public float height = 50f;
    public float rotationAngle = 90f;

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + height, player.position.z);
        transform.rotation = Quaternion.Euler(rotationAngle, player.eulerAngles.y, 0);
    }
}

using UnityEngine;
// Created by Herman Bergström
public class Map : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject playerPointer;
    const float mapActivePointerScale = 10f;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            map.SetActive(!map.activeSelf);
            playerPointer.transform.localScale = map.activeSelf ? Vector3.one * mapActivePointerScale : Vector3.one;
        }
    }
}

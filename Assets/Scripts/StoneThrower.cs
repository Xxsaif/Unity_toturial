using UnityEngine;

public class StoneThrower : MonoBehaviour
{
    public GameObject stonePrefab; // Prefab för sten (kunna kasta fler åt gången)
    public Transform throwPoint;   
    public float throwForce = 10f; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            ThrowStone();
        }
    }

    void ThrowStone()
    {
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = stone.GetComponent<Rigidbody>();
        rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
    }
}

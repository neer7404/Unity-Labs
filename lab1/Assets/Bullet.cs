using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f; // Destruction over time

    void Start()
    {
        // Automatically destroy bullet after 3 seconds
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destruction of enemy using Tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy enemy
            Destroy(gameObject); // Destroy bullet
        }
        else 
        {
            Destroy(gameObject); // Destroy bullet if it hits floor/walls
        }
    }
}
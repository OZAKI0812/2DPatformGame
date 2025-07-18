using UnityEngine;

public class SawController : MonoBehaviour
{
    public float length = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Static;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
    // Update is called once per frame
    void Update()
    {
        if (PandaController.gameState != "playing") return;
        GameObject panda = GameObject.FindGameObjectWithTag("Player");

        float dist = Vector2.Distance(transform.position, panda.transform.position);

        if (length >= dist)
        {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
            if (rigidbody.bodyType == RigidbodyType2D.Static)
            {
                rigidbody.bodyType = RigidbodyType2D.Dynamic;

                rigidbody.freezeRotation = false;
                rigidbody.angularVelocity = 180f;
            }
        }
    }
}

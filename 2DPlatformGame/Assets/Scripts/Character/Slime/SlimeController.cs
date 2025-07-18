using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float speed = 1.0f;
    public float length = 10.0f;
    private bool isEnable = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PandaController.gameState != "playing") return;

        GameObject panda = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(transform.position, panda.transform.position);
        if (length >= distance)
        {
            isEnable = true;
        }
        if (isEnable)
        {
            Vector3 pos = this.transform.position;
            pos.x -= speed * Time.deltaTime;
            this.transform.position = pos;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}

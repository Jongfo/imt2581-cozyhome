using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float despawnTimer = 4f;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        despawnTimer -= Time.fixedDeltaTime;
        if(despawnTimer <= 0)
        {
            Destroy(this.gameObject);
        }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Solid"))
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            if(other.GetComponent<RabbitBehaviour>() != null)
            {
                other.GetComponent<RabbitBehaviour>().Hunted();
            }
        }
    }
}

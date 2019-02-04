using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBehaviour : MonoBehaviour
{
    public GameObject rabbitMeat;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Arrows call this function to kill the rabbit and drop food.
    public void Hunted()
    {
        Debug.Log("I got hunted");
        //spawn food
        Instantiate(rabbitMeat, this.transform.position, Quaternion.identity); // not working? 
        //die
        Destroy(this.gameObject);
    }
}

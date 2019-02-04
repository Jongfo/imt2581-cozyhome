using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAndArrow : MonoBehaviour
{
    public Rigidbody2D arrowFab;    //Projectile to spawn
    public float speed = 1000f; //speed of projectile

    private Camera cam;
    private const float reloadDelay = 2f;
    private float reloadWait = 2f;


    public bool equipped;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (equipped && GetComponent<Player>().canMove)
        {
            //get aim axis (get mouse camera position in relation to player)

            Vector3 aimDir = cam.ScreenToWorldPoint(Input.mousePosition);
            aimDir -= this.transform.position;
            aimDir.z = this.transform.position.z;


            //fire the arrow
            reloadWait -= Time.deltaTime;
            if (Input.GetButtonDown("Fire1"))
            {
                //reload delay
                if (reloadWait <= 0f)
                {
                    reloadWait = reloadDelay;
                    FireArrow(aimDir.normalized);
                }
            }
        }
    }
    
    void FireArrow(Vector3 aimDirection)
    {
        //spawn projectile
        Quaternion rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) - 45);
        Rigidbody2D arrowClone = Instantiate(arrowFab, new Vector3(transform.position.x, transform.position.y, 0), rotation);
        //bug encounter: z position used to be taken from the camera.


        arrowClone.GetComponent<Rigidbody2D>().AddForce(aimDirection * speed);
        

    }
}

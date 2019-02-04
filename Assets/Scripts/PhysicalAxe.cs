using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAxe : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        Tree tree = collision.gameObject.GetComponent<Tree>();
        if (tree != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Axe>().OnHitTree(tree);
        }
    }

    public void OnEndAnimation()
    {
        Destroy(gameObject);
    }
}

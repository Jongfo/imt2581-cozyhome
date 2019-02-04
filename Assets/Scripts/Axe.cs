using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    int durability = 100;
    public bool equipped;

    [SerializeField] GameObject axePrefab;

    private void Update()
    {
        if (equipped)
        {
            if (GameObject.Find("Axe") == null && Input.GetButtonDown("Fire1"))
            {
                // Start swinging the axe
                GameObject axe = Instantiate(axePrefab, transform);
                axe.name = "Axe";
            }
        }
    }

    public void OnHitTree(Tree tree)
    {
        // If we hit a tree, decrease the trees hitpoints
        tree.TakeDamage(1);
        durability--;
    }
}

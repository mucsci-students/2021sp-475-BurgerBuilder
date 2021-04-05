using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Food foodName;
    public PhysicMaterial foodPhysics;
    public Material foodMaterial;

    public bool isGarbage = false;

    private Rigidbody rigidbody;

    void Start()
    {
        if(foodMaterial != null && foodPhysics != null)
        {
            GetComponent<MeshRenderer>().material = foodMaterial;
            GetComponent<BoxCollider>().material = foodPhysics;
        }
        rigidbody = GetComponent<Rigidbody>();
    }

    public enum Food 
    {
        TopBun,
        BottomBun,
        Patty,
        Lettuce,
        Cheese
    }
}

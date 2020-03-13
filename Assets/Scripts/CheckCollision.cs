using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    // this class will return the gameObject that has started to collide with this object
    // and the bool value of collision

    public GameObject CollidingObject { private set; get; } = null;

    public bool IsColliding { private set; get; } = false;

    // when object collides
    private void OnTriggerStay(Collider other)
    {
        IsColliding = true;
        CollidingObject = other.gameObject;
    }

    // when object exits
    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
    }
}

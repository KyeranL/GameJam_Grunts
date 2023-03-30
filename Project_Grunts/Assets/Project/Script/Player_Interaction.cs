using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float raycastDistance = 10f;
    public LayerMask layerMask;
    public GameObject hitObject;

    void Update()
    {
        // Cast a ray from the transform's forward direction
        Ray ray = new Ray(transform.position, transform.forward);

        // Check if the ray hits any colliders
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
        {
            // If the ray hits a collider, store the game object and print its name to the console
            hitObject = hit.collider.gameObject;
            Debug.Log("Hit " + hitObject.name);

            // Draw a line between the starting point of the raycast and the point where it hits a collider
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        else
        {
            // If the ray doesn't hit anything, draw a line showing the full extent of the raycast
            Debug.DrawLine(transform.position, transform.position + transform.forward * raycastDistance, Color.green);

            // Set hitObject to null if there is no object hit by the raycast
            hitObject = null;
        }
    }
}

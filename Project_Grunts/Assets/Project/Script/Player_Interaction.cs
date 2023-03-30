using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Player_Interaction : MonoBehaviour
{
    public float raycastDistance = 10f;
    public LayerMask layerMask;
    public GameObject hitObject;
    public GameObject childInteractive;
    private bool feedbackPlayed = false;

    private void Update()
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

            // Find the child object with the name "Feel"
            GameObject feelObject = hitObject.transform.Find("Feel").gameObject;
            if (feelObject != null)
            {
                // Find the child object with the name "Feel_interaction"
                Transform feelInteraction = feelObject.transform.Find("Feel_interaction");
                if (feelInteraction != null && !feedbackPlayed)
                {
                    // Get the MMFeedback component from the child object and play its feedbacks
                    MMFeedbacks feelFeedbacks = feelInteraction.GetComponent<MMFeedbacks>();
                    if (feelFeedbacks != null)
                    {
                        feelFeedbacks.PlayFeedbacks();
                        feedbackPlayed = true;
                    }
                }
            }
        }
        else
        {
            // If the ray doesn't hit anything, draw a line showing the full extent of the raycast
            Debug.DrawLine(transform.position, transform.position + transform.forward * raycastDistance, Color.green);

            // Set hitObject to null if there is no object hit by the raycast
           

            // Find the child object with the name "Feel" if feedbackPlayed is true
            if (feedbackPlayed)
            {
                GameObject feelObject = hitObject.transform.Find("Feel").gameObject;
                if (feelObject != null)
                {
                    // Find the child object with the name "exit_interaction"
                    Transform exitInteraction = feelObject.transform.Find("Exit_interaction");
                    if (exitInteraction != null)
                    {
                        // Get the MMFeedback component from the child object and play its feedbacks
                        MMFeedbacks exitFeedbacks = exitInteraction.GetComponent<MMFeedbacks>();
                        if (exitFeedbacks != null)
                        {
                            exitFeedbacks.PlayFeedbacks();
                            feedbackPlayed = false;
                        }
                    }
                }
            }

            hitObject = null;
        }
    }
}


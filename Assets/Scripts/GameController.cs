using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ScientistController ControlledUnit;
    private CameraController playerCamera;
    void Start()
    {
        playerCamera = Camera.main.GetComponent<CameraController>();
    }

    GameObject MouseOverScientest()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the ray hit something and if that object has the "Scientist" tag
        if (hit.collider != null && hit.collider.CompareTag("Scientist"))
        {
            return hit.collider.gameObject; // The mouse is over a scientist
        }

        return null; // The mouse is not over a scientist
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && ControlledUnit != null)
        {
            ControlledUnit.GetComponent<ScientistController>().isSelected = false;
            ControlledUnit = null;
            playerCamera.target = null;
        }

        // Check if the user is clicking the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Perform the raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {

                Debug.Log(hit.collider.gameObject.name);
                // Check if the object hit has the tag "Scientist"    
                if (hit.collider.gameObject.CompareTag("Scientist"))
                {
                    // Do something with the Scientist object
                    if (hit.collider.gameObject.GetComponent<ScientistController>().isAlive)
                    {
                        if (ControlledUnit != null && ControlledUnit != hit.collider.gameObject)
                        {
                            try
                            {
                                ControlledUnit.isSelected = false;
                                ControlledUnit.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            }
                            catch
                            {
                                Debug.LogWarning("ERROR");
                            }
                        }
                        ControlledUnit = hit.collider.gameObject.GetComponent<ScientistController>();

                        ControlledUnit.isSelected = true;
                        playerCamera.ChangeTarget(ControlledUnit.gameObject);
                    }
                }
            }
        }
    }
}

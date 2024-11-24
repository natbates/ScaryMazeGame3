using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f; // Speed of the camera when moving freely
    public float smoothTimeMove = 0.3f; // Time to reach the target
    public float smoothTimeZoom = 0.3f; // Time to reach the target

    private Vector3 velocity = Vector3.zero;

    private bool isShaking = false;
    private float shakeStrength;
    private float shakeDuration;
    private Coroutine shakeRoutine;

    public float zoomSpeed; // Speed at which the camera zooms in/out
    public float targetZoom; // The zoom level when the camera has a target
    public float defaultZoom; // The default zoom level when no target is present
    public float maxZoom; // Maximum zoom level
    public float minZoom; // Minimum zoom level

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Get the Camera component
        if (cam == null)
        {
            Debug.LogError("No Camera component found on this GameObject.");
            enabled = false; // Disable this script if no Camera component is found
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Zoom in on the target
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            // Smoothly follow the target
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0);

            if (isShaking)
            {
                smoothedPosition += Random.insideUnitSphere * shakeStrength;
            }

            transform.position = smoothedPosition;
        }
        else
        {
            // Handle zooming with mouse wheel
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0.0f)
            {
                // Calculate the target zoom level based on input
                float targetZoomLevel = cam.orthographicSize - scrollInput * zoomSpeed;

                // Clamp the target zoom level between minZoom and maxZoom
                targetZoomLevel = Mathf.Clamp(targetZoomLevel, minZoom, maxZoom);

                // Smoothly interpolate to the target zoom level
                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomLevel, ref velocity.z, smoothTimeZoom);
            }

            // Get input for horizontal and vertical movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Calculate desired movement direction and scale it by speed
            Vector3 inputDirection = new Vector3(moveHorizontal, moveVertical, 0);
            Vector3 desiredPosition = transform.position + inputDirection * speed * Time.deltaTime;

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTimeMove);

            // Update the camera position
            transform.position = smoothedPosition;
        }
    }



    public void ChangeTarget(GameObject scientist)
    {
        target = scientist.transform;
    }

    public void CameraShake(float strength, float duration)
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }
        shakeStrength = strength;
        shakeDuration = duration;
        shakeRoutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
    }
}

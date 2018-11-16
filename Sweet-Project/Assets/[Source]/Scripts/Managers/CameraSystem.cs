using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Range(0.01f, 5f)]
    public float smoothTime = 1f;

    public Vector3 lookOffset = Vector3.up;
    public Vector3 positionOffset = new Vector3(0f,3f,-10f);
    public float minZoom = 60f;
    public float maxZoom = 10f;
    private float zoomLimit = 10f;

    private Bounds bounds;
    private Camera cam;
    private Vector3 lastLookTarget;
    private Vector3 lookVelocity = Vector3.zero;
    private Vector3 moveVelocity = Vector3.zero;
    
    public void Init()
    {
        //cam = GetComponent<Camera>();
        cam = Camera.main;

        if(positionOffset.z != 0)
        {
            zoomLimit = -positionOffset.z;
        }
    }

    public void CalcLookPos(List<Vector3> playerPositions)
    {
        // Look Section
        if(playerPositions.Count == 0 || playerPositions == null)
        {
            return;
        }

        GetBounds(playerPositions);
        Zoom();

        if (lastLookTarget == Vector3.zero)
        {
            lastLookTarget = bounds.center + lookOffset;
        }

        Vector3 targetPos = Vector3.SmoothDamp(lastLookTarget, bounds.center + lookOffset, ref lookVelocity, smoothTime / 2);
        transform.LookAt(targetPos);
        lastLookTarget = targetPos;

        // Position Section
        Vector3 desiredPos = targetPos + positionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref moveVelocity, smoothTime);
    }

    private void GetBounds(List<Vector3> playerPositions)
    {
        bounds = new Bounds(playerPositions[0], Vector3.zero);

        for (int i = 0; i < playerPositions.Count; i++)
        {
            bounds.Encapsulate(playerPositions[i]);
        }
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, bounds.size.x / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
}

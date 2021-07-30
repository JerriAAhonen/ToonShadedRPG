using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CameraController : SingletonBehaviour<CameraController>
{
    public Transform targetTransform;
    public Transform camTransform;
    public Transform camPivotTransform;
    
    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    
    public float minPivot = -35f;
    public float maxPivot = 35f;
    
    private Vector3 camTransformPos;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    private float targetZPos;
    private float defaultZPos;
    private float lookAngle;
    private float pivotAngle;

    private float camSphereRadius = 0.2f;
    private float camCollisionOffset = 0.2f;
    private float minCollisionOffset = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        defaultZPos = camTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    private void Update()
    {
        var delta = Time.deltaTime;
        FollowTarget(delta);
        HandleCameraRotation(delta, Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        HandleCameraCollisions(delta);
    }

    private void FollowTarget(float delta)
    {
        var targetPos = Vector3.SmoothDamp(
            transform.position, 
            targetTransform.position, 
            ref cameraFollowVelocity, 
            delta / followSpeed);
        transform.position = targetPos;
    }

    private void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        lookAngle += (mouseX * lookSpeed) / delta;
        pivotAngle -= (mouseY * lookSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        var rotation = Vector3.zero;
        rotation.y = lookAngle;
        var targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        camPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta)
    {
        targetZPos = defaultZPos;
        RaycastHit hit;
        var direction = camTransform.position - camPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(
            camPivotTransform.position,
            camSphereRadius,
            direction,
            out hit,
            Mathf.Abs(targetZPos),
            ignoreLayers))
        {
            var distance = Vector3.Distance(camPivotTransform.position, hit.point);
            targetZPos = -(distance - camCollisionOffset);
        }

        if (Mathf.Abs(targetZPos) < minCollisionOffset)
            targetZPos = -minCollisionOffset;

        camTransformPos.z = Mathf.Lerp(camTransform.localPosition.z, targetZPos, delta / 0.2f);
        camTransform.localPosition = camTransformPos;
    }
}

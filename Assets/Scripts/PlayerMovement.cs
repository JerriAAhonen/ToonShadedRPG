using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 10f;
	public float sprintSpeed = 11f;
	public float jumpForce = 0.1f;
	public float gravity = -3f;
	public float turnSmoothTime = 0.1f;

	private CharacterController cc;
	private Transform cam;
	private float smoothVel;
	private float verticalVel;
	private PlayerMovementState movState;
	
	public event Action BecameIdle;
	public event Action StartedWalking;
	public event Action StartedRunning;
	public event Action Jumped;
	
	private void Awake()
	{
		cc = GetComponent<CharacterController>();
		if (Camera.main != null)
			cam = Camera.main.transform;
		else
			Debug.LogError("Didn't find a main camera", this);
	}

	private void Update()
	{
		var hor = Input.GetAxisRaw("Horizontal");
		var ver = Input.GetAxisRaw("Vertical");
		var jump = Input.GetKey(KeyCode.Space);
		var sprint = Input.GetKey(KeyCode.LeftShift);
		var dir = new Vector3(hor, 0, ver).normalized;
		
		// Gravity
		if (cc.isGrounded)
		{
			verticalVel = -0.5f;
		}
		else
		{
			verticalVel += gravity * Time.deltaTime;
		}
		
		// Jump
		if (jump && cc.isGrounded)
		{
			verticalVel = jumpForce * Time.deltaTime;
			Jumped?.Invoke();
		}
		
		var moveDir = new Vector3(0f, verticalVel, 0f);
		
		// Point the player in the direction of the camera
		if (InputHandler.Instance.IsAiming)
		{
			// Get the angle the camera is facing 
			var targetAngle = cam.eulerAngles.y;
			
			// Smooth the transition between rotations
			var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}
		
		if (dir.sqrMagnitude >= Mathf.Epsilon)
		{
			// Get the angle the player is facing 
			var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			
			// Only rotate the player if he is not aiming
			if (!InputHandler.Instance.IsAiming)
			{
				// Smooth the transition between rotations
				var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
			}

			// Get the move dir from the direction the camera is looking at
			moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			moveDir.y += verticalVel;

			var oldState = movState;
			movState = sprint ? PlayerMovementState.Running : PlayerMovementState.Walking;
			if (oldState != movState)
				InvokeStateChange(movState);
		}
		else
		{
			var oldState = movState;
			movState = PlayerMovementState.Idle;
			if (oldState != movState)
				InvokeStateChange(movState);
		}

		var moveSpeed = sprint ? sprintSpeed : speed;
		cc.Move(moveDir * (moveSpeed * Time.deltaTime));
	}

	private void InvokeStateChange(PlayerMovementState newState)
	{
		switch (newState)
		{
			case PlayerMovementState.Idle:
				BecameIdle?.Invoke();
				break;
			case PlayerMovementState.Walking:
				StartedWalking?.Invoke();
				break;
			case PlayerMovementState.Running:
				StartedRunning?.Invoke();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
		}
	}

	#region Types

	private enum PlayerMovementState { Idle, Walking, Running}

	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] [Range(0, 100)] private float speed = 5.0f;
	[SerializeField] private float jumpForce = 250.0f;
	private float horizontalMove = 0.0f;
	private float verticalMove = 0.0f;
	private float jump = 0.0f;
	//private bool IsJumpable = true;
	private int jumpCount = 1;

	private Rigidbody rd;
	private CameraController cameraCon;

	// Start is called before the first frame update
	void Start()
	{
		rd = gameObject.GetComponent<Rigidbody>();
		if(rd == null) { rd = gameObject.AddComponent<Rigidbody>(); }
		cameraCon = Camera.main.gameObject.GetComponent<CameraController>();
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		KeyInput();
	}

	private void FixedUpdate()
	{
		float DeltaTime = Time.fixedDeltaTime;

		Movement(DeltaTime);
	}

	void KeyInput()
	{
		horizontalMove = Input.GetAxis("Horizontal");
		verticalMove = Input.GetAxis("Vertical");

		if (Input.GetAxisRaw("Horizontal") == 0.0f) { horizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { verticalMove = 0.0f; }

		jump = Input.GetAxisRaw("Jump");

		//if (Input.GetKeyDown(KeyCode.Space) == true) { JumpKey = true; }
		//else if (Input.GetKeyUp(KeyCode.Space) == true) { JumpKey = false; }
	}

	void Movement(float DeltaTime)
	{
		if(cameraCon != null)
		{
			if (cameraCon.cameraTarget == this.gameObject)
			{
				if (horizontalMove != 0 || verticalMove != 0)
				{
					//rd.AddForce(new Vector3(HorizontalMove, 0.0f, VerticalMove) * DeltaTime * Speed);
					transform.Translate(new Vector3(horizontalMove, 0.0f, verticalMove) * DeltaTime * speed);
				}

				if (jump != 0)
				{
					if (jumpCount > 0)
					{
						rd.AddForce(Vector3.up * jumpForce);
						jumpCount = jumpCount - 1;
					} 
				}
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject != gameObject)
		{
			//IsJumpable = true;
			jumpCount = 1;
		}
	}

	//private void OnCollisionExit(Collision collision)
	//{ 
	//	if(collision.gameObject != gameObject)
	//	{
	//		IsJumpable = false;
	//	} 
	//}
}

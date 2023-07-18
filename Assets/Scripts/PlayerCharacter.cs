using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] [Range(0, 100)] private float Speed = 5.0f;
	[SerializeField] private float JumpForce = 250.0f;
	private float HorizontalMove = 0.0f;
	private float VerticalMove = 0.0f;
	private float Jump = 0.0f;
	//private bool IsJumpable = true;
	private int JumpCount = 1;

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
		HorizontalMove = Input.GetAxis("Horizontal");
		VerticalMove = Input.GetAxis("Vertical");

		if (Input.GetAxisRaw("Horizontal") == 0.0f) { HorizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { VerticalMove = 0.0f; }

		Jump = Input.GetAxisRaw("Jump");

		//if (Input.GetKeyDown(KeyCode.Space) == true) { JumpKey = true; }
		//else if (Input.GetKeyUp(KeyCode.Space) == true) { JumpKey = false; }
	}

	void Movement(float DeltaTime)
	{
		if(cameraCon != null)
		{
			if (cameraCon.CameraTarget == this.gameObject)
			{
				if (HorizontalMove != 0 || VerticalMove != 0)
				{
					//rd.AddForce(new Vector3(HorizontalMove, 0.0f, VerticalMove) * DeltaTime * Speed);
					transform.Translate(new Vector3(HorizontalMove, 0.0f, VerticalMove) * DeltaTime * Speed);
				}

				if (Jump != 0)
				{
					if (JumpCount > 0)
					{
						rd.AddForce(Vector3.up * JumpForce);
						JumpCount = JumpCount - 1;
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
			JumpCount = 1;
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

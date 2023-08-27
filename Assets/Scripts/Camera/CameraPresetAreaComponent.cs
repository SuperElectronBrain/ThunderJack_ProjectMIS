using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public enum CameraType
{
	None,
	PlayerCharacter,
	Spline,
}

public enum CameraTarget
{
	None,
	PlayerCharacter,
	Spline,
}

public class CameraPresetAreaComponent : MonoBehaviour
{
	public CameraType m_CameraType;
	public CameraTarget m_CameraTarget;
	[SerializeField] private float angle;
	[SerializeField] private float distance;
	[SerializeField] private SplineContainer m_CameraSpline;
	[SerializeField] private SplineContainer m_CameraTargetSpline;

	[HideInInspector] public PlayerCharacter m_PlayerCharacter;

	// Start is called before the first frame update
	void Start()
	{
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	// Update is called once per frame
	//void Update()
	//{
	//
	//}

	//public Quaternion GetCameraTarget(Vector3 p_Position)
	//{
	//	if (m_PlayerCharacter != null)
	//	{
	//		if(m_CameraTarget == CameraTarget.PlayerCharacter)
	//		{
	//			return Quaternion.LookRotation(m_PlayerCharacter.transform.position - p_Position);
	//		}
	//		else if(m_CameraTarget == CameraTarget.Manual)
	//		{
	//
	//		}
	//	}
	//
	//	return Quaternion.identity;
	//}

	public Quaternion CalculateCameraLotation(Vector3 p_Position)
	{
		if (m_PlayerCharacter != null)
		{
			if (m_CameraTarget == CameraTarget.PlayerCharacter)
			{
				return Quaternion.LookRotation(m_PlayerCharacter.transform.position - p_Position);
			}
			else if (m_CameraTarget == CameraTarget.Spline)
			{
				if (m_CameraTargetSpline != null)
				{
					NativeSpline t_SplinePath = new NativeSpline(new SplinePath<Spline>(m_CameraTargetSpline.Splines), m_CameraTargetSpline.transform.localToWorldMatrix);
					SplineUtility.GetNearestPoint(t_SplinePath, p_Position, out float3 t_Point, out float t_t);
					return Quaternion.LookRotation(new Vector3(t_Point.x, t_Point.y, t_Point.z) - p_Position);
				}
			}
		}

		return Quaternion.identity;
	}


	public Vector3 CalculateCameraPosition(Vector3 p_Position)
	{
		Vector3 t_Position = p_Position;

		if (m_PlayerCharacter != null)
		{
			if (m_CameraType == CameraType.PlayerCharacter)
			{
				t_Position = m_PlayerCharacter.transform.position + (transform.up * distance * Mathf.Sin(angle * Mathf.Deg2Rad)) + (transform.forward * -distance * Mathf.Cos(angle * Mathf.Deg2Rad));
			}
			else if(m_CameraType == CameraType.Spline)
			{
				if(m_CameraSpline != null)
				{
					NativeSpline t_SplinePath = new NativeSpline(new SplinePath<Spline>(m_CameraSpline.Splines), m_CameraSpline.transform.localToWorldMatrix);
					SplineUtility.GetNearestPoint(t_SplinePath, m_PlayerCharacter.transform.position, out float3 t_Point, out float t_t);
					t_Position = t_Point;
				}
			}

			//Vector3 t_ProjectedPoint = Vector3.Dot(transform.forward, m_PlayerCharacter.transform.position - transform.position) * (m_PlayerCharacter.transform.position - transform.position);
			//Vector3 t_Vector = ((transform.right * Mathf.Cos(angle)) + (transform.up * Mathf.Sin(angle))) * distance;
			//return t_ProjectedPoint + t_Vector;
		}

		return t_Position;
	}
}

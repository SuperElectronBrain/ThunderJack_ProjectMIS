using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInGame : MonoBehaviour
{
    [SerializeField] private bool bHideInGame = true;
	private bool bOriginVisibility;

	// Start is called before the first frame update
	void Start()
    {
        MeshRenderer t_MeshRenderer = GetComponent<MeshRenderer>();
		if (t_MeshRenderer != null) { bOriginVisibility = t_MeshRenderer.enabled; t_MeshRenderer.enabled = !bHideInGame; }

		SpriteRenderer t_SpriteRenderer = GetComponent<SpriteRenderer>();
		if (t_SpriteRenderer != null) { bOriginVisibility = t_MeshRenderer.enabled; t_SpriteRenderer.enabled = !bHideInGame; }
	}

	private void OnEnable()
	{
		MeshRenderer t_MeshRenderer = GetComponent<MeshRenderer>();
		if (t_MeshRenderer != null) { t_MeshRenderer.enabled = !bHideInGame; }

		SpriteRenderer t_SpriteRenderer = GetComponent<SpriteRenderer>();
		if (t_SpriteRenderer != null) { t_SpriteRenderer.enabled = !bHideInGame; }
	}

	private void OnDisable()
	{
		MeshRenderer t_MeshRenderer = GetComponent<MeshRenderer>();
		if (t_MeshRenderer != null) { t_MeshRenderer.enabled = bOriginVisibility; }

		SpriteRenderer t_SpriteRenderer = GetComponent<SpriteRenderer>();
		if (t_SpriteRenderer != null) { t_SpriteRenderer.enabled = bOriginVisibility; }
	}
}

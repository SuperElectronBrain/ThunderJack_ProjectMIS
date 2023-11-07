using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustratedGuideUIScript : UIScript
{
	[SerializeField] private Image itemSprite;
	[SerializeField] private Image itemScript;
	[SerializeField] private float[] elementalRatio = { 0.0f, 0.0f, 0.0f};
	[SerializeField] private RectTransform buttonsParent;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	protected override void RefresfAction()
	{
		base.RefresfAction();
	}
}

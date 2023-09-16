using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SalesStand : MonoBehaviour
{
	//[SerializeField] private GameObject SalesPanelPrefab;
	//private GameObject SalesPanel;
	//private GameObject SalesItemPanel;
	//[SerializeField] private GameObject NeedsPanelPrefab;
	//private GameObject NeedsPanel;
	//private GameObject NeedsItemPanel;
	//private Inventory inventory;
	//private Customer customer;

	// Start is called before the first frame update
	//void Start()
	//{
		//Canvas canvas = FindObjectOfType<Canvas>();
		//if (canvas != null)
		//{
			/*
			//if (SalesPanelPrefab != null) 
			//{
			//	SalesPanel = Instantiate(SalesPanelPrefab, canvas.transform);
			//	SalesPanel.SetActive(false);
			//
			//	FlexibleGridLayout ItemPanelLayout = UniFunc.GetChildComponent<FlexibleGridLayout>(SalesPanel);
			//	if (ItemPanelLayout != null)
			//	{
			//		SalesItemPanel = ItemPanelLayout.gameObject;
			//	}
			//
			//	Button button = UniFunc.GetChildComponent<Button>(SalesPanel);
			//	{
			//		button.onClick.AddListener(() => 
			//		{
			//			if(customer != null)
			//			{
			//				List<SelectedItem> selectedItems = inventory.GetSelectedItems();
			//				List<Item> items = inventory.GetItems();
			//				string compareString0 = ".";
			//				for (int i = 0; i < selectedItems.Count; i = i + 1)
			//				{
			//					if (selectedItems[i].itemAmount > 0)
			//					{
			//						compareString0 = compareString0 + items[i].itemCode + ".";
			//					}
			//				}
			//
			//				List<Need> needs = customer.GetNeeds();
			//				string compareString1 = ".";
			//				for(int i = 0; i < needs.Count; i = i + 1)
			//				{
			//					compareString1 = compareString1 + needs[i].need.itemCode + ".";
			//				}
			//
			//				if(compareString0 == compareString1)
			//				{
			//					for (int i = 0; i < selectedItems.Count; i = i + 1)
			//					{
			//						if (selectedItems[i].itemAmount > 0)
			//						{
			//							inventory.PopItem(items[i].itemCode, selectedItems[i].itemAmount);
			//						}
			//					}
			//					inventory.SelectionReset();
			//
			//					HideChilds(SalesItemPanel.transform);
			//					DisplaySalesPanel(false);
			//					HideChilds(NeedsItemPanel.transform);
			//					DisplayNeedsPanel(false);
			//
			//					for (int i = 0; i < needs.Count; i = i + 1)
			//					{
			//						for(int j = 0; j < needs[i].rewards.Count; j = j + 1)
			//						{
			//							inventory.AddItem(needs[i].rewards[j]);
			//						}
			//						customer.CompleteNeed(needs[i].need.itemCode);
			//					}
			//
			//					inventory.RefreshInventory();
			//				}
			//			}
			//		});
			//	}
			//}
			//
			//if (NeedsPanelPrefab != null)
			//{
			//	NeedsPanel = Instantiate(NeedsPanelPrefab, canvas.transform);
			//	NeedsPanel.SetActive(false);
			//
			//	FlexibleGridLayout ItemPanelLayout = UniFunc.GetChildComponent<FlexibleGridLayout>(NeedsPanel);
			//	if (ItemPanelLayout != null)
			//	{
			//		NeedsItemPanel = ItemPanelLayout.gameObject;
			//	}
			//}
			*/
		//}
	//}

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

	//private void OnDisable()
	//{
		//if (inventory != null)
		//{
		//	inventory.DisplayItems(false);
		//	inventory.itemSelectEvent.RemoveListener(ItemSelectEvent);
		//	inventory = null;
		//
		//	DisplaySalesPanel(false);
		//	DisplayNeedsPanel(false);
		//	HideChilds(SalesItemPanel.transform);
		//	HideChilds(NeedsItemPanel.transform);
		//}
	//}

	//private void ItemSelectEvent()
	//{
	//	if (inventory != null)
	//	{
	//		HideChilds(SalesItemPanel.transform);
	//
	//		int count = 0;
	//		List<SelectedItem> selectedItems = inventory.GetSelectedItems();
	//		List<Item> items = inventory.GetItems();
	//		for (int i = 0; i < selectedItems.Count; i = i + 1)
	//		{
	//			if (selectedItems[i].itemAmount > 0)
	//			{
	//				if(count < SalesItemPanel.transform.childCount)
	//				{
	//					if (SalesItemPanel.transform.GetChild(count).gameObject.activeSelf == false)
	//					{
	//						SalesItemPanel.transform.GetChild(count).gameObject.SetActive(true);
	//					}
	//
	//					Image image = SalesItemPanel.transform.GetChild(count).GetComponent<Image>();
	//					TextMeshProUGUI text = null;
	//					if (image != null)
	//					{
	//						text = UniFunc.GetChildComponent<TextMeshProUGUI>(SalesItemPanel.transform.GetChild(count));
	//
	//						//
	//					}
	//					if (text != null)
	//					{
	//						text.text = items[i].itemCode + " " + selectedItems[i].itemAmount;
	//					}
	//				}
	//				count = count + 1;
	//			}
	//		}
	//
	//		//for (int i = count; i < ItemPanel.transform.childCount; i = i + 1)
	//		//{
	//		//	if (ItemPanel.transform.GetChild(i).gameObject.activeSelf == true)
	//		//	{
	//		//		ItemPanel.transform.GetChild(i).gameObject.SetActive(false);
	//		//	}
	//		//}
	//	}
	//}

	//private void DisplayNeedItems()
	//{
	//	if(customer != null)
	//	{
	//		if(NeedsItemPanel != null)
	//		{
	//			HideChilds(NeedsItemPanel.transform);
	//
	//			List<Need> needs = customer.GetNeeds();
	//			for (int i = 0; i < (needs.Count < NeedsItemPanel.transform.childCount ? needs.Count : NeedsItemPanel.transform.childCount); i = i + 1)
	//			{
	//				if (NeedsItemPanel.transform.GetChild(i).gameObject.activeSelf == false)
	//				{
	//					NeedsItemPanel.transform.GetChild(i).gameObject.SetActive(true);
	//				}
	//
	//				Image image = NeedsItemPanel.transform.GetChild(i).GetComponent<Image>();
	//				TextMeshProUGUI text = null;
	//				if (image != null)
	//				{
	//					text = UniFunc.GetChildComponent<TextMeshProUGUI>(NeedsItemPanel.transform.GetChild(i));
	//
	//					//
	//				}
	//				if (text != null)
	//				{
	//					text.text = needs[i].need.itemCode + " " + needs[i].need.itemAmount;
	//				}
	//			}
	//		}
	//	}
	//}

	//public void HideChilds(Transform target)
	//{
	//	if(target != null)
	//	{
	//		for (int i = 0; i < target.childCount; i = i + 1)
	//		{
	//			if (target.GetChild(i).gameObject.activeSelf == true)
	//			{
	//				target.GetChild(i).gameObject.SetActive(false);
	//			}
	//		}
	//	}
	//}

	//public void DisplaySalesPanel(bool param)
	//{
	//	if (SalesPanel != null)
	//	{
	//		if (SalesPanel.activeSelf == !param)
	//		{
	//			SalesPanel.SetActive(param);
	//		}
	//	}
	//}

	//public void DisplayNeedsPanel(bool param)
	//{
	//	if (NeedsPanel != null)
	//	{
	//		if (NeedsPanel.activeSelf == !param)
	//		{
	//			NeedsPanel.SetActive(param);
	//		}
	//	}
	//}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject != this.gameObject)
	//	{
	//		if (other.gameObject.GetComponent<PlayerCharacter>() != null)
	//		{
	//			inventory = other.gameObject.GetComponent<Inventory>();
	//			if (inventory != null)
	//			{
	//				inventory.DisplayItems(true);
	//				inventory.itemSelectEvent.AddListener(ItemSelectEvent);
	//
	//				if (customer != null)
	//				{
	//					if (customer.GetNeeds().Count > 0)
	//					{
	//						DisplaySalesPanel(true);
	//						DisplayNeedsPanel(true);
	//						DisplayNeedItems();
	//					}
	//				}
	//			}
	//		}
	//
	//		if (other.gameObject.GetComponent<Customer>() != null) 
	//		{
	//			customer = other.gameObject.GetComponent<Customer>();
	//			if (customer.GetNeeds().Count > 0)
	//			{
	//				if(inventory != null)
	//				{
	//					DisplaySalesPanel(true);
	//					DisplayNeedsPanel(true);
	//					DisplayNeedItems();
	//				}
	//			}
	//		}
	//	}
	//}

	//private void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject != this.gameObject)
	//	{
	//		if (other.gameObject.GetComponent<PlayerCharacter>() != null)
	//		{
	//			if(other.gameObject == inventory.gameObject)
	//			{
	//				inventory.DisplayItems(false);
	//				inventory.itemSelectEvent.RemoveListener(ItemSelectEvent);
	//				inventory = null;
	//
	//				DisplaySalesPanel(false);
	//				DisplayNeedsPanel(false);
	//				HideChilds(SalesItemPanel.transform);
	//				HideChilds(NeedsItemPanel.transform);
	//			}
	//		}
	//
	//		if (other.gameObject.GetComponent<Customer>() != null)
	//		{
	//			if(other.gameObject == customer.gameObject)
	//			{
	//				customer = null;
	//
	//				DisplaySalesPanel(false);
	//				DisplayNeedsPanel(false);
	//				HideChilds(SalesItemPanel.transform);
	//				HideChilds(NeedsItemPanel.transform);
	//			}
	//		}
	//	}
	//}
}

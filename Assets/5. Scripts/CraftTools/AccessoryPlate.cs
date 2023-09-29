using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AccessoryPlate : MonoBehaviour
{
	public AdvencedItem m_Input = new AdvencedItem();
	[SerializeField] private SpriteRenderer m_SpriteRenderer;
	[SerializeField] private TMPro.TextMeshPro m_Text;
	
    // Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CraftItem(AdvencedItem p_AItem)
    {
		//List<GemRecipe> t_GemRecipes = UniFunc.FindRecipesOfElement(UniFunc.FindRecipesOfElement(GameManager.Instance.ItemManager.GetGemRecipe(), 1, p_AItem.itemCode), 2, m_Input.itemCode);

		//if (t_GemRecipes != null)
		int t_ItemCode = GameManager.Instance.ItemManager.GetCombinationItem(p_AItem.itemCode, m_Input.itemCode);
		if (t_ItemCode != -1)
        {
			// m_Input = new AdvencedItem(t_GemRecipes[0].itemID, p_AItem.itemProgress, 1);
			m_Input = new AdvencedItem(t_ItemCode, p_AItem.itemProgress, 1);
			RefreshPlate();
			return true;
		}

		return false;
    }

    public void RefreshPlate()
    {
		//if (m_Input.IsAddable(new AdvencedItem()) == false)
        //{
            //if (m_Input.itemAmount > 0)
            //{
		if (m_SpriteRenderer != null)
		{
			m_SpriteRenderer.sprite = UniFunc.FindSprite(m_Input.itemCode);
		}
		if (m_Text != null)
		{
			m_Text.text = m_Input.itemCode + "";
		}
			//}
        //}
	}
}
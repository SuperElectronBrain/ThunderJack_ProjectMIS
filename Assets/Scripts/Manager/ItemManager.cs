using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Materials = 1, Gem, Accessory, Jewelry
}

public class ItemManager : MonoBehaviour
{
    Dictionary<int, JewelryItemData> jewelryItemData;
    Dictionary<int, BasicItemData> materialItemData;
    [SerializeField]
    List<BasicItemData> basicItemData;
    [SerializeField]
    List<ShopItemData> shopItemList;

    [SerializeField]
    List<GemRecipe> gemRecipes;

    [SerializeField]
    int itemA, ItemB;

    // Start is called before the first frame update
    void Start()
    {
        LoadItemData();
        LoadMaterialElement();
        LoadGemRecipe();
        LoadShopItemData();
    }

    //Load Material Element
    #region
    void LoadMaterialElement()
    {
        var elementData = GameManager.Instance.DataBase.Parser("element_Master");

        foreach (var element in elementData)
        {
            int itemIdx = Tools.IntParse(element["Item_ID"]) - 1;

            MaterialItemData newItemData = basicItemData[itemIdx] as MaterialItemData;

            if(newItemData != null)
                newItemData = (MaterialItemData)basicItemData[itemIdx];

            newItemData += new MaterialItemData
            {
                elementType1 = Tools.IntParse(element["element_Type_1"]),
                elementType2 = Tools.IntParse(element["element_Type_1"]),
                elementType3 = Tools.IntParse(element["element_Type_1"]),
                elementPercent1 = Tools.IntParse(element["Element_Percent_1"]),
                elementPercent2 = Tools.IntParse(element["Element_Percent_2"]),
                elementPercent3 = Tools.IntParse(element["Element_Percent_3"])
            };

            basicItemData[itemIdx] = newItemData;
        }
    }
    #endregion

    //Load Item
    #region
    void LoadItemData()
    {
        jewelryItemData = new();
        basicItemData = new();
        materialItemData = new();

        var itemRecipe = GameManager.Instance.DataBase.Parser("Item_Master");

        foreach (var ir in itemRecipe)
        {
            int itemType = Tools.IntParse(ir["Item_Type"]);
            Sprite resourceImage = GameManager.Instance.AddressableManager.LoadObject<Sprite>(ir["Item_Icon_Name"].ToString());

            switch (itemType)
            {
                case (int)ItemType.Jewelry:
                    basicItemData.Add(
                    new JewelryItemData
                    {
                        itemID = Tools.IntParse(ir["Item_ID"]),
                        itemNameEg = ir["Material_Name_EG"].ToString(),
                        itemNameKo = ir["Item_Name_KO"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage,
                        itemText = ir["Item_Text"].ToString()
                    }
                    );
                    break;
                case (int)ItemType.Materials:
                    basicItemData.Add(
                    new MaterialItemData
                    {
                        itemID = Tools.IntParse(ir["Item_ID"]),
                        itemNameEg = ir["Material_Name_EG"].ToString(),
                        itemNameKo = ir["Item_Name_KO"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage,
                        itemText = ir["Item_Text"].ToString(),
                    }
                    );
                    break;
                default:
                    basicItemData.Add(
                    new BasicItemData
                    {
                        itemID = Tools.IntParse(ir["Item_ID"]),
                        itemNameEg = ir["Material_Name_EG"].ToString(),
                        itemNameKo = ir["Item_Name_KO"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage,
                        itemText = ir["Item_Text"].ToString()
                    }
                    );
                    break;
            }
        }
    }
    #endregion

    //Load Gem_Recipe
    #region 
    void LoadGemRecipe()
    {
        gemRecipes = new();

        var gemRecipe = GameManager.Instance.DataBase.Parser("Make_Master");

        foreach(var recipe in gemRecipe)
        {
            gemRecipes.Add(
                new GemRecipe
                {
                    //itemNameEg = recipe[]
                    itemNameKo = recipe["Item_Name_Ko"].ToString(),
                    material1 = Tools.IntParse(recipe["Make_Material_1"]),
                    material2 = Tools.IntParse(recipe["Make_Material_2"]),
                    material3 = Tools.IntParse(recipe["Make_Material_3"]),
                    materialPercent1 = Tools.FloatParse(recipe["Make_Percent_1"]),
                    materialPercent2 = Tools.FloatParse(recipe["Make_Percent_2"]),
                    materialPercent3 = Tools.FloatParse(recipe["Make_Percent_3"])
                }
                );
        }
    }
    #endregion

    public void LoadShopItemData()
    {
        shopItemList = new();

        var shopItemData = GameManager.Instance.DataBase.Parser("Shop_Master");

        foreach(var item in shopItemData)
        {
            shopItemList.Add(
                new ShopItemData
                {
                    itemId = Tools.IntParse(item["Item_ID"]),
                    buyValue = Tools.IntParse(item["Buy_Value"]),
                    sellValue = Tools.IntParse(item["Sell_Value"]),
                    shopType = Tools.IntParse(item["Shop_Type"]),
                    shopRate = Tools.FloatParse(item["Shop_Rate"])
                }
                );
        }
    }


    public int GetCombinationItem(int item1, int item2)
    {
        int gem, accessory;

        if (basicItemData[item1].itemType == ItemType.Gem)
        {
            gem = item1;
            accessory = item2;
        }            
        else
        {
            gem = item2;
            accessory = item1;
        }

        foreach (var jewelryItem in jewelryItemData)
        {
            if (jewelryItem.Value.itemComb1 == gem && jewelryItem.Value.itemComb2 == accessory)
                return jewelryItem.Key;
        }

        return -1;
    }

    public string GetItemName(int itemID)
    {
        return basicItemData[itemID - 1].itemNameKo;
    }

    public Sprite GetItemSprite(int itemID)
    {
        return basicItemData[itemID - 1].itemResourceImage;
    }

    public string GetItemText(int itemID)
    {
        return basicItemData[itemID - 1].itemText;
    }

    public List<GemRecipe> GetGemRecipe()
    {
        return gemRecipes;
    }
}

//ItemData
#region

[System.Serializable]
public class BasicItemData
{
    public int itemID;
    public string itemNameKo;
    public string itemNameEg;        
    public Sprite itemResourceImage;
    public ItemType itemType;
    public string itemText;
}
[System.Serializable]
public class JewelryItemData : BasicItemData
{
    public int itemComb1;
    public int itemComb2;   
}
[System.Serializable]
public class MaterialItemData : BasicItemData
{
    public int elementType1;
    public int elementType2;
    public int elementType3;
    public float elementPercent1;
    public float elementPercent2;
    public float elementPercent3;
    public static MaterialItemData operator +(MaterialItemData mid1, MaterialItemData mid2)
    {
        return new MaterialItemData
        {
            itemID = mid1.itemID,
            itemNameKo = mid1.itemNameKo,
            itemNameEg = mid1.itemNameEg,
            itemResourceImage = mid1.itemResourceImage,
            itemType = mid1.itemType,
            itemText = mid1.itemText,
            elementType1 = mid2.elementType1,
            elementType2 = mid2.elementType2,
            elementType3 = mid2.elementType3,
            elementPercent1 = mid2.elementPercent1,
            elementPercent2 = mid2.elementPercent2,
            elementPercent3 = mid2.elementPercent3
        };
    }
}
#endregion

[System.Serializable]
public class GemRecipe
{
    public string itemNameKo;
    public string itemNameEg;
    public int material1;
    public int material2;
    public int material3;
    public float materialPercent1;
    public float materialPercent2;
    public float materialPercent3;
}

[System.Serializable]
public class ShopItemData
{
    public int itemId;
    public int buyValue;
    public int sellValue;
    public int shopType;
    public float shopRate;
}
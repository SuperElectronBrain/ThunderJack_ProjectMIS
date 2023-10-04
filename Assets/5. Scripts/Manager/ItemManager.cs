using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum ItemType
{
    Materials = 1, Gem, Accessory, Jewelry
}

public enum SalesItemType
{
    Materials = 1, Accessory
}

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    List<BasicItemData> basicItemData;
    [SerializeField]
    List<ShopItemData> shopItemList;

    [SerializeField]
    List<GemRecipe> gemRecipes;
    [SerializeField]
    List<JewelryItemData> jewelryItemData;

    [SerializeField]
    List<MaterialItemData> materialItemData;

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
        materialItemData = new List<MaterialItemData>();
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
                elementType2 = Tools.IntParse(element["element_Type_2"]),
                //elementType3 = Tools.IntParse(element["element_Type_3"]),
                elementPercent1 = Tools.IntParse(element["Element_Percent_1"]),
                elementPercent2 = Tools.IntParse(element["Element_Percent_2"]),
                //elementPercent3 = Tools.IntParse(element["Element_Percent_3"])
            };

            
            basicItemData[itemIdx] = newItemData;
            materialItemData.Add(newItemData);
        }
    }
    #endregion

    //Load Item
    #region
    void LoadItemData()
    {
        basicItemData = new();
        jewelryItemData = new();

        var itemRecipe = GameManager.Instance.DataBase.Parser("Item_Master");

        foreach (var ir in itemRecipe)
        {
            int itemType = Tools.IntParse(ir["Item_Type"]);
            Sprite resourceImage = AddressableManager.LoadObject<Sprite>(ir["Item_Icon_Name"].ToString());

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
            if (Tools.IntParse(recipe["Item_Type"]) == (int)ItemType.Gem)
            {
                gemRecipes.Add(
                new GemRecipe
                {
                    //itemNameEg = recipe[]
                    itemID = Tools.IntParse(recipe["Item_ID"]),
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
            else
            {
                int itemComb1 = Tools.IntParse(recipe["Make_Material_1"]);
                int itemComb2 = Tools.IntParse(recipe["Make_Material_2"]);
                ((JewelryItemData)basicItemData[Tools.IntParse(recipe["Item_ID"]) - 1]).itemComb1 = itemComb1;
                ((JewelryItemData)basicItemData[Tools.IntParse(recipe["Item_ID"]) - 1]).itemComb2 = itemComb2;
            }
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
                    shopRate = Tools.FloatParse(item["Shop_Rate"]),
                    sellFame = Tools.IntParse(item["Sell_Fame"])
                }
                );
        }
    }

    /// <summary>
    /// 아이템 조합식 확인 아이템 ID 반환 없으면 -1반환
    /// </summary>
    /// <param name="gem"></param>
    /// <param name="accessory"></param>
    /// <returns></returns>
    public int GetCombinationItem(int gem, int accessory)
    {

        foreach (var jewelryItem in basicItemData)
        {
            if (jewelryItem.itemType != ItemType.Jewelry)
                continue;

            if (((JewelryItemData)jewelryItem).itemComb1 == gem && ((JewelryItemData)jewelryItem).itemComb2 == accessory)
                return jewelryItem.itemID;
        }

        return -1;
    }

    public int GetItemIdByName(string itemName)
    {
        foreach(var item in basicItemData)
        {
            if (item.itemNameKo == itemName)
                return item.itemID;
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

    public RequestStuff GetRequestStuffByItemID(int itemID)
    {
        itemID--;
        if(basicItemData[itemID] is JewelryItemData)
        {
            return new RequestStuff
            {
                requestStuff1 = ((JewelryItemData)basicItemData[itemID]).itemComb1,
                requestStuff2 = ((JewelryItemData)basicItemData[itemID]).itemComb2
            };
        }
        return new RequestStuff
        {
            requestStuff1 = 0,
            requestStuff2 = 0
        };
    }

    public MaterialItemData GetMaterialItem(int itemID)
    {
        return (MaterialItemData)basicItemData[itemID - 1];
    }

    public BasicItemData GetBasicItemData(int itemID)
    {
        if (itemID < 0 || itemID >= basicItemData.Count)
            return null;

        return basicItemData[itemID - 1];
    }

    public List<ShopItemData> GetShopItemData()
    {
        return shopItemList;
    }

    public SalesData GetSalesData(int itemID)
    {
        return new SalesData
        {
            money = shopItemList[itemID - 1].sellValue,
            fame = shopItemList[itemID - 1].sellFame
        };
    }

    public List<ShopItemData> GetShopItemDataBySalesType(SalesItemType salesItemType)
    {
        List<ShopItemData> returnShopItemList = new List<ShopItemData>();

        for(int i = 0; i < shopItemList.Count; i++)
        {
            if(shopItemList[i].shopType == (int)salesItemType)
            {
                returnShopItemList.Add(shopItemList[i]);
            }            
        }

        return returnShopItemList;
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
    public int itemID;
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
    public int sellFame;
    public int shopType;
    public float shopRate;
}
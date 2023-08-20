using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gem = 1, Accessory, Jewelry
}

public class ItemManager : Singleton<ItemManager>
{
    Dictionary<int, JewelryItemData> jewelryItemData;
    Dictionary<int, BasicItemData> basicItemData;
    Dictionary<int, BasicItemData> allItemData;
    Dictionary<string, Sprite> itemResources;
    [SerializeField]
    List<Sprite> spriteList = new();

    [SerializeField]
    List<BasicItemData> list = new(); 

    [SerializeField]
    int itemA, ItemB;

    // Start is called before the first frame update
    void Start()
    {
        jewelryItemData = new();
        basicItemData= new();
        itemResources = new();

        var itemRecipe = DataBase.Instance.Parser("Item_Master");

        foreach (var ir in itemRecipe)
        {
            int itemType = Tools.IntParse(ir["Item_Type"]);
            Sprite resourceImage = AddressableManager.Instance.LoadObject<Sprite>(ir["Item_Resource_Name"].ToString());

            if (itemType == (int)ItemType.Jewelry)
            {
                jewelryItemData.Add(Tools.IntParse(ir["Item_ID"]),
                    new JewelryItemData
                    {
                        itemNameEg = ir["Item_Name_Eg"].ToString(),
                        itemNameKo = ir["Item_Name_Ko"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage,
                        itemComb1 = Tools.IntParse(ir["Item_Combination_1"]),
                        itemComb2 = Tools.IntParse(ir["Item_Combination_2"]),
                        itemComb3 = Tools.IntParse(ir["Item_Combination_3"])
                    }
                    );
                list.Add(
                    new JewelryItemData
                    {
                        itemNameEg = ir["Item_Name_Eg"].ToString(),
                        itemNameKo = ir["Item_Name_Ko"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage,
                        itemComb1 = Tools.IntParse(ir["Item_Combination_1"]),
                        itemComb2 = Tools.IntParse(ir["Item_Combination_2"]),
                        itemComb3 = Tools.IntParse(ir["Item_Combination_3"])
                    }
                    );
            }
            else
            {
                basicItemData.Add(Tools.IntParse(ir["Item_ID"]),
                    new BasicItemData
                    {
                        itemNameEg = ir["Item_Name_Eg"].ToString(),
                        itemNameKo = ir["Item_Name_Ko"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage
                    }
                    );
                list.Add(
                    new BasicItemData
                    {
                        itemNameEg = ir["Item_Name_Eg"].ToString(),
                        itemNameKo = ir["Item_Name_Ko"].ToString(),
                        itemType = (ItemType)Tools.IntParse(ir["Item_Type"]),
                        itemResourceImage = resourceImage
                    }
                    );
            }
            //AddressableManager.Instance.CreateImage(resourceImage);
        }

        //Debug.Log(GetItemName(GetCombinationItem(0, 9)));
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetItemName(GetCombinationItem(itemA, ItemB)));
        }
    }

    public int GetCombinationItem(int item1, int item2, int item3 = -1)
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
        return jewelryItemData[itemID].itemNameKo;
    }
}

[System.Serializable]
public class BasicItemData
{
    public string itemNameEg;
    public string itemNameKo;
    public ItemType itemType;
    public Sprite itemResourceImage;
}
[System.Serializable]
public class JewelryItemData : BasicItemData
{
    public int itemComb1;
    public int itemComb2;
    public int itemComb3;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.IO;
using UnityEngine.UI;

public class SpineSkinChanger : MonoBehaviour
{
    SkeletonAnimation skAni;
    SkeletonData skData;

    static List<ClothData> clothesTypeA;
    static List<ClothData> clothesTypeB;     

    static List<RaceData> raceTypeA;
    static List<RaceData> raceTypeB;

    static string race;
    static string cloth;

    [SerializeField]
    Button button;
    [SerializeField]
    Transform panel;
    List<GameObject> buttonList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();

        LoadSkinData();

        RandomSkin();        
    }

    public void LoadSkinData()
    {
        var clothesData = GameManager.Instance.DataBase.Parser("Clothes_Master");
        var raceData = GameManager.Instance.DataBase.Parser("Race_Master");

        clothesTypeA = new List<ClothData>();
        clothesTypeB = new List<ClothData>();
        raceTypeA = new List<RaceData>();
        raceTypeB = new List<RaceData>();

        foreach(var cloth in clothesData)
        {
            ClothData clothData = new()
            {
                clothType = Tools.IntParse(cloth["Clothes_Type"]),
                clothNameEg = cloth["Clothes_Name_Eg"].ToString(),
                clothesCompatible = Tools.IntParse(cloth["Clothes_compatible"])
            };

            switch(clothData.clothType)
            {
                case 1:
                    clothesTypeA.Add(clothData);
                    break;
                case 2:
                    clothesTypeB.Add(clothData);
                    break;
                default:
                    throw new System.Exception("Cloth Type Error");
            }
        }

        foreach(var race in raceData)
        {
            RaceData newRaceData = new RaceData()
            {
                raceMainType = Tools.IntParse(race["Race_Main_Type"]),
                raceSubType = Tools.IntParse(race["Race_Sub_Type"]),
                raceMainName = race["Race_Main_Name"].ToString(),
                raceSubName = race["Race_Sub_Name"].ToString()
            };

            switch(newRaceData.raceMainType)
            {
                case 1:
                    raceTypeA.Add(newRaceData);
                    break;
                case 2:
                    raceTypeB.Add(newRaceData);
                    break;
            }
        }
    }

    public static void SkinReset(SkeletonAnimation skAni)
    {        
        skAni.skeletonDataAsset.Clear();
        if (isTypeA)
            skAni.initialSkinName = "1. full/Dress_Sheep";
        else
            skAni.initialSkinName = "1. Full/Human_Travler";
        skAni.Initialize(true);
    }

    public static void ChangeType(SkeletonAnimation skAni, bool isType)
    {        
        string customerType = isType ? "A" : "B";
        skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>("NormalCustomer" + customerType);
        skAni.skeletonDataAsset.Clear();

        var skData = skAni.skeleton.Data;
        var mix = new Skin("default");       

        skAni.skeleton.SetSkin(mix);

        skAni.skeleton.SetSlotsToSetupPose();
    }

    static bool isTypeA = true;

    public void ChangeType()
    {
        isTypeA = !isTypeA;
        ChangeType(skAni, isTypeA);
    }

    public void RandomSkin()
    {
        RandomSkinChange(skAni,isTypeA);
        Init();
    }

    public void Init()
    {
        List<RaceData> searchRaceList = isTypeA ? raceTypeA : raceTypeB;
        List<ClothData> searchClothList = isTypeA ? clothesTypeA : clothesTypeB;

        int clothIdx = 0;

        for(int i = 0; i < searchClothList.Count; i++)
        {
            if (cloth.Replace("Cloth/", "") == searchClothList[i].clothNameEg)
            {
                clothIdx = i;
                break;
            }                
        }

        for(int i = 0; i < buttonList.Count; i++)
        {
            Destroy(buttonList[i]);
        }

        for(int i = 0; i < searchRaceList.Count; i++)
        {
            if ((searchClothList[clothIdx].clothesCompatible & searchRaceList[i].raceSubType) == 0)
                continue;

            Button newButton = Instantiate(button, panel);
            buttonList.Add(newButton.gameObject);

            newButton.GetComponentInChildren<Text>().text = searchRaceList[i].raceSubName;
            newButton.gameObject.name = searchRaceList[i].raceMainName + "/" + searchRaceList[i].raceSubName;
            newButton.onClick.AddListener(() =>
            {
                Debug.Log(newButton.gameObject.name);
                race = newButton.gameObject.name;
                SelectSkinChange(skAni);
                Init();
            });
        }

        for (int i = 0; i < searchClothList.Count; i++)
        {
            Button newButton = Instantiate(button, panel);
            buttonList.Add(newButton.gameObject);

            newButton.GetComponentInChildren<Text>().text = searchClothList[i].clothNameEg;
            newButton.gameObject.name = "Cloth/" + searchClothList[i].clothNameEg;
            newButton.onClick.AddListener(() =>
            {
                cloth = newButton.gameObject.name;
                SelectSkinChange(skAni);
                Init();
            });
        }
    }

    static void SelectSkinChange(SkeletonAnimation skAni)
    {
        SkinReset(skAni);
        SkeletonData skData = skAni.skeleton.Data;
        
        var mix = new Skin("default");
        mix.AddSkin(skData.FindSkin(race));
        mix.AddSkin(skData.FindSkin(cloth));

        skAni.skeleton.SetSkin(mix);

        skAni.skeleton.SetSlotsToSetupPose();
    }

    public static void RandomSkinChange(SkeletonAnimation skAni, bool isTypeA = true)
    {
        //GetRandomCloth(1);
        SkinReset(skAni);
        SkeletonData skData = skAni.skeleton.Data;

        if (isTypeA)
        {
            int randomRaceIdx = Random.Range(0, raceTypeA.Count);

            cloth = "Cloth/" + GetRandomCloth(raceTypeA[randomRaceIdx]);

            race = raceTypeA[randomRaceIdx].raceMainName + "/" + raceTypeA[randomRaceIdx].raceSubName;

            var mix = new Skin("default");
            mix.AddSkin(skData.FindSkin(race));
            mix.AddSkin(skData.FindSkin(cloth));

            skAni.skeleton.SetSkin(mix);
        }
       /* else
        {
            string newCloth = "Cloth/" + ((ClothTypeB)Random.Range(0, ((int)ClothTypeB.End))).ToString();
            RaceTypeB race = (RaceTypeB)Random.Range(0, ((int)RaceTypeB.End));
            string newRace = null;
            string type = null;

            switch (race)
            {
                case RaceTypeB.Human:
                    type = ((HumanRaceTypeB)Random.Range(0, ((int)HumanRaceTypeB.End))).ToString();
                    break;
                case RaceTypeB.Owl:
                    type = ((OwlRaceTypeB)Random.Range(0, ((int)OwlRaceTypeB.End))).ToString();
                    break;
                case RaceTypeB.Tiger:
                    type = ((TigerRaceTypeB)Random.Range(0, ((int)TigerRaceTypeB.End))).ToString();
                    break;
            }

            type = type.Replace("_", " ");

            newRace = race.ToString() + "/" + type;

            var mix = new Skin("default");
            Debug.Log(newRace);
            Debug.Log(newCloth);
            mix.AddSkin(skData.FindSkin(newRace));
            mix.AddSkin(skData.FindSkin(newCloth));

            skAni.skeleton.SetSkin(mix);
        }*/

        skAni.skeleton.SetSlotsToSetupPose();
    }

    static string GetRandomCloth(RaceData raceData)
    {
        List<ClothData> randomClothList = new List<ClothData>();
        List<ClothData> searchClothList = raceData.raceMainType == 1 ? clothesTypeA : clothesTypeB;

        string randomCloth = "";

        for (int i = 0; i < searchClothList.Count; i++)
        {
            if ((searchClothList[i].clothesCompatible & raceData.raceSubType) > 0)
                randomClothList.Add(searchClothList[i]);
        }

        if(randomClothList.Count == 0)
            return randomCloth;

        int randomClothIdx = Random.Range(0, randomClothList.Count);

        randomCloth = randomClothList[randomClothIdx].clothNameEg;

        return randomCloth;
    }

    const string filePath = "Editor/SkinList.csv";
    public string description;

    public void WriteSkinInfo()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream);

        //writer.WriteLine(string.Format("{0}, {1}", skin, cloth));

        writer.Close();
    }
}

public class ClothData
{
    public int clothType;
    public string clothNameEg;
    public int clothesCompatible;
}

public class RaceData
{
    public int raceMainType;
    public int raceSubType;
    public string raceMainName;
    public string raceSubName;
}

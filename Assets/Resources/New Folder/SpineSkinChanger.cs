using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.IO;
using System.Text;

public enum HumanRaceType
{
    Blonde_Human, Blue_Human, Brown_Human, Red_human, End
}

public enum ParrotRaceType
{
    Blue_Parrot, Yellow_Parrot, End
}

public enum SheepRaceType
{
    Pink_Sheep, white_Sheep, End
}

public enum RaceType
{
    Human, Parrot, Sheep, End
}

public enum ClothType
{
    Dress_Blue, Dress_Yellow, Shirts_Brown, Shirts_Green, Shirts_White, Vest, End
}

public class SpineSkinChanger : MonoBehaviour
{
    SkeletonAnimation skAni;
    SkeletonData skData;
    [SpineSkin, SerializeField]
    string skin;
    [SpineSkin("Cloth"), SerializeField]
    string cloth;
    [SpineSlot, SerializeField]
    string slot;
    [SpineAttachment, SerializeField]
    string key;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
        skData = skAni.skeleton.Data;

        var mix = new Skin("default");
        /*        mix.AddSkin(skData.FindSkin("skin-base"));
                mix.AddSkin(skData.FindSkin("nose/short"));
                mix.AddSkin(skData.FindSkin("eyelids/girly"));
                mix.AddSkin(skData.FindSkin("eyes/violet"));
                mix.AddSkin(skData.FindSkin("hair/brown"));
                mix.AddSkin(skData.FindSkin("clothes/hoodie-orange"));
                mix.AddSkin(skData.FindSkin("legs/pants-jeans"));
                mix.AddSkin(skData.FindSkin("accessories/bag"));
                mix.AddSkin(skData.FindSkin("accessories/hat-red-yellow"));*/

        mix.AddSkin(skData.FindSkin(skin));
        mix.AddSkin(skData.FindSkin(cloth));

        skAni.skeleton.SetSkin(mix);
        
        skAni.skeleton.SetSlotsToSetupPose();
        Debug.Log(skData.Skins.Count);
    }

    public static void SkinReset(SkeletonAnimation skAni)
    {
        skAni.skeletonDataAsset.Clear();
        skAni.Initialize(true);
    }

    public static void RandomSkinChange(SkeletonAnimation skAni)
    {
        SkinReset(skAni);
        SkeletonData skData = skAni.skeleton.Data;

        string newCloth = "Cloth/" + ((ClothType)Random.Range(0, ((int)ClothType.End))).ToString();
        RaceType race = (RaceType)Random.Range(0, ((int)RaceType.End));
        string newRace = null;
        string type = null;

        switch (race)
        {
            case RaceType.Human:
                type = ((HumanRaceType)Random.Range(0, ((int)HumanRaceType.End))).ToString();
                break;
            case RaceType.Parrot:
                type = ((ParrotRaceType)Random.Range(0, ((int)ParrotRaceType.End))).ToString();
                break;
            case RaceType.Sheep:
                type = ((SheepRaceType)Random.Range(0, ((int)SheepRaceType.End))).ToString();
                break;
        }

        type = type.Replace("_", " ");

        newRace = race.ToString() + "/" + type;

        var mix = new Skin("default");
        mix.AddSkin(skData.FindSkin(newRace));
        mix.AddSkin(skData.FindSkin(newCloth));

        skAni.skeleton.SetSkin(mix);

        skAni.skeleton.SetSlotsToSetupPose();
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

        writer.WriteLine(string.Format("{0}, {1}", skin, cloth));

        writer.Close();
    }
}

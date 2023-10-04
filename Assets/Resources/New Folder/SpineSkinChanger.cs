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
public enum HumanRaceTypeB
{
    Human1, Human2, End
}

public enum OwlRaceTypeB
{
    OwlBrown, OwlGray, End
}

public enum TigerRaceTypeB
{
    TigerOrange, TigerWhite, End
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

public enum RaceTypeB
{
    Human, Owl, Tiger, End
}

public enum ClothType
{
    Dress_Blue, Dress_Yellow, Shirts_Brown, Shirts_Green, Shirts_White, Vest, End
}

public enum ClothTypeB
{
    RobeGreen, RobeViolet, SuitBlue, SuitGreen, TravlerPink, TravlerRed, End
}

public enum GuestType
{
    None, GuestA, GuestB
}

public class SpineSkinChanger : MonoBehaviour
{
    SkeletonAnimation skAni;
    SkeletonData skData;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();

        RandomSkin();
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
    }

    public static void RandomSkinChange(SkeletonAnimation skAni, bool isTypeA = true)
    {
        SkinReset(skAni);
        SkeletonData skData = skAni.skeleton.Data;

        if(isTypeA)
        {
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
        }
        else
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
        }
              
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

        //writer.WriteLine(string.Format("{0}, {1}", skin, cloth));

        writer.Close();
    }
}

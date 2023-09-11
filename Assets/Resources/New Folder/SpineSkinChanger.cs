using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Spine.Unity.AttachmentTools;

public class SpineSkinChanger : MonoBehaviour
{
    SkeletonAnimation skAni;
    SkeletonData skData;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
        skData = skAni.skeleton.Data;

        var mix = new Skin("default");
        mix.AddSkin(skData.FindSkin("skin-base"));
        mix.AddSkin(skData.FindSkin("nose/short"));
        mix.AddSkin(skData.FindSkin("eyelids/girly"));
        mix.AddSkin(skData.FindSkin("eyes/violet"));
        mix.AddSkin(skData.FindSkin("hair/brown"));
        mix.AddSkin(skData.FindSkin("clothes/hoodie-orange"));
        mix.AddSkin(skData.FindSkin("legs/pants-jeans"));
        mix.AddSkin(skData.FindSkin("accessories/bag"));
        mix.AddSkin(skData.FindSkin("accessories/hat-red-yellow"));

        skAni.skeleton.SetSkin(mix);
        
        skAni.skeleton.SetSlotsToSetupPose();
        Debug.Log(skData.Skins.Count);
    }

    public void SkinReset()
    {
        skAni.skeletonDataAsset.Clear();
        skAni.Initialize(true);
    }

    public void SkinChange1()
    {
        var mix = new Skin("default");
        mix.AddSkin(skData.FindSkin("skin-base"));
        mix.AddSkin(skData.FindSkin("nose/long"));
        mix.AddSkin(skData.FindSkin("eyelids/girly"));
        mix.AddSkin(skData.FindSkin("eyes/yellow"));
        mix.AddSkin(skData.FindSkin("hair/brown"));
        mix.AddSkin(skData.FindSkin("clothes/dress-green"));
        mix.AddSkin(skData.FindSkin("legs/pants-green"));
        mix.AddSkin(skData.FindSkin("accessories/scarf"));
        mix.AddSkin(skData.FindSkin("accessories/hat-red-yellow"));

        skAni.skeleton.SetSkin(mix);

        skAni.skeleton.SetSlotsToSetupPose();
    }

    public void SkinChange2()
    {
        var mix = new Skin("default");
        mix.AddSkin(skData.FindSkin("skin-base"));
        mix.AddSkin(skData.FindSkin("nose/short"));
        mix.AddSkin(skData.FindSkin("eyelids/girly"));
        mix.AddSkin(skData.FindSkin("eyes/violet"));
        mix.AddSkin(skData.FindSkin("hair/brown"));
        mix.AddSkin(skData.FindSkin("clothes/hoodie-orange"));
        mix.AddSkin(skData.FindSkin("legs/pants-jeans"));
        mix.AddSkin(skData.FindSkin("accessories/bag"));
        mix.AddSkin(skData.FindSkin("accessories/hat-red-yellow"));

        skAni.skeleton.SetSkin(mix);

        skAni.skeleton.SetSlotsToSetupPose();
    }
}

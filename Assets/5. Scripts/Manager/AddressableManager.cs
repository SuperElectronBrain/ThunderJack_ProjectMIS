using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Threading.Tasks;

public class AddressableManager : MonoBehaviour
{
    [SerializeField]
    AssetLabelReference labelReference;
    [SerializeField]
    AssetReference assetReference;
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField]
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        //Addressables.Instantiate();
        /*Addressables.LoadAssetsAsync<Sprite>("Accessory", (result) => { go.Add(result); }).Completed += (handle) =>
        {
            foreach (var sprite in go)
            {
                assetReference.InstantiateAsync(canvas.transform).Completed += (result) =>
                {
                    result.Result.GetComponent<Image>().sprite = sprite;
                };
                //assetReference.InstantiateAsync(canvas.transform);
            }
        };*/
        Initialize();
    }

    public void Initialize()
    {
        Addressables.InitializeAsync().WaitForCompletion();
    }

    // Update is called once per frame
    /*void Update()
    {
*//*        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var index = gameObjects.Count - 1;
Addressables.LoadAssetAsync<T>(loadObjectName)
            Addressables.ReleaseInstance(gameObjects[index]);
            gameObjects.RemoveAt(index);
        }*//*
    }*/

    static bool AddressableNullCheck<T>(string key)
    {
        foreach (var l in Addressables.ResourceLocators)
        {
            IList<IResourceLocation> locs;
            if (l.Locate(key, typeof(T), out locs))
                return true;
        }
        return false;
    }

    public static T LoadObject<T>(string loadObjectName)
    {
        T returnObject = default;

        if (!AddressableNullCheck<T>(loadObjectName))
        {
            Debug.Log("실패" + "object Name = " + loadObjectName);

            if (typeof(T) == typeof(Spine.SkeletonData))
                loadObjectName = "Icon";
            else
                loadObjectName = "Stone";
        }
        else
        {
            Debug.Log("성공");
        }


        var op = Addressables.LoadAssetAsync<T>(loadObjectName);

        returnObject = op.WaitForCompletion();

        return returnObject;
    }

    public void Image(Sprite sprite)
    {
        assetReference.InstantiateAsync(canvas.transform).Completed += (result) =>
        {
            result.Result.GetComponent<Image>().sprite = sprite;
        };
    }
}

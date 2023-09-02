using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
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
    }

    // Update is called once per frame
    void Update()
    {
/*        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var index = gameObjects.Count - 1;

            Addressables.ReleaseInstance(gameObjects[index]);
            gameObjects.RemoveAt(index);
        }*/
    }

    public T LoadObject<T>(string loadObjectName)
    {
        T returnObject = default;

        //임시임 확실히 버그인지 알 수 있는 이미지로 바꿀 것
        if (Addressables.LoadResourceLocationsAsync(loadObjectName, typeof(object)).IsValid())
            loadObjectName = "Stone";

        var op = Addressables.LoadAssetAsync<T>(loadObjectName);

        returnObject = op.WaitForCompletion();

        return returnObject;
    }    

    public void CreateImage(Sprite sprite)
    {
        assetReference.InstantiateAsync(canvas.transform).Completed += (result) =>
        {
            result.Result.GetComponent<Image>().sprite = sprite;
        };
    }
}

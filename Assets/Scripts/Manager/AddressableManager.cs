using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Threading.Tasks;

public class AddressableManager : Singleton<AddressableManager>
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var index = gameObjects.Count - 1;
            // InstantiateAsync <-> ReleaseInstance
            // Destroy함수로써 ref count가 0이면 메모리 상의 에셋을 언로드한다.
            Addressables.ReleaseInstance(gameObjects[index]);
            gameObjects.RemoveAt(index);
        }
    }

    public T LoadObject<T>(string loadObjectName)
    {
        T returnObject = default;
        /*Addressables.LoadAssetAsync<T>(loadObjectName).Completed += (handle) =>
        {
            returnObject = handle.Result;
        };*/
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

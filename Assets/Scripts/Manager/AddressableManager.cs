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
    }

    // Update is called once per frame
    void Update()
    {
/*        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var index = gameObjects.Count - 1;
Addressables.LoadAssetAsync<T>(loadObjectName)
            Addressables.ReleaseInstance(gameObjects[index]);
            gameObjects.RemoveAt(index);
        }*/
    }

    public static T LoadObject<T>(string loadObjectName)
    {
        T returnObject = default;

        if (typeof(T) == typeof(Sprite))
        {

            var a = Addressables.LoadAssetAsync<T>(loadObjectName);

            a.WaitForCompletion();

            if (a.Status == AsyncOperationStatus.Succeeded)
            {

            }
            else
            {
                loadObjectName = "Icon";
            }


            /*            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(loadObjectName);

                        handle.WaitForCompletion();
                            if (handle.Status == AsyncOperationStatus.Succeeded)
                            {
                                Debug.Log($"�ּ� '{loadObjectName}'�� ã�ҽ��ϴ�.");
                                // �ּҰ� �����ϴ� ��� ������ �۾��� �߰��� �� �ֽ��ϴ�.
                            }
                            else
                            {
                                Debug.LogError($"�ּ� '{loadObjectName}'�� ã�� �� �����ϴ�.");
                            loadObjectName = "Stone";
                                // �ּҰ� �������� �ʴ� ��� ������ �۾��� �߰��� �� �ֽ��ϴ�.
                            }*/
            /*            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(loadObjectName);
                        //�ӽ��� Ȯ���� �������� �� �� �ִ� �̹����� �ٲ� ��
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Debug.Log("����");
                        }
                        else
                            loadObjectName = "Icon";*//*
            if (Addressables.LoadAssetAsync<T>(loadObjectName).IsValid())
            //if (Addressables.LoadResourceLocationsAsync(loadObjectName, typeof(object)).IsValid())
            {
                Debug.Log("����");
                //loadObjectName = "Icon";
            }                
            else
            {
                Debug.Log("����");
            }*/
        }

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

using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "Area Data", menuName = "Scriptable Object/Area Data", order = int.MaxValue)]
public class AreaSO : ScriptableObject
{
    [SerializeField]
    public Image image;
    [SerializeField]
    public TextMeshProUGUI text;

    [SerializeField]
    public string areaName;

    public bool hasActive = false;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourMaster : MonoBehaviour
{
    Dictionary<int, BehaviourData> behaviourData;
    [SerializeField]
    List<BehaviourData> behaviourDataList = new();

    // Start is called before the first frame update
    void Start()
    {
        behaviourData = new Dictionary<int, BehaviourData>();        

        var behaviours = GameManager.Instance.DataBase.Parser("BehaviourMaster");

        foreach(var behaviour in behaviours)
        {
            int actionType = Tools.IntParse(behaviour["Action_Type"]);
            int actionId = Tools.IntParse(behaviour["Action_ID"]);

            BehaviourData bData = null;

            switch(actionType)
            {
                case 1:
                    bData = new BehaviourType1
                    {
                        actionName = behaviour["Action_Name"].ToString(),
                        actionType = actionType,
                        actionGoal = Tools.IntParse(behaviour["Action_Goal"])
                    };
                    break;
                case 2:
                    bData = new BehaviourType2
                    {
                        actionName = behaviour["Action_Name"].ToString(),
                        actionType = actionType,
                        //actionGoal = GameManager.Instance.LocationManager.
                    };
                    break;
                case 3:
                    bData = new BehaviourType2
                    {
                        actionName = behaviour["Action_Name"].ToString(),
                        actionType = actionType,
                        actionGoal = GameManager.Instance.LocationManager.GetLocationPosition(1)
                    };
                    break;
            }

            behaviourDataList.Add(bData);
            behaviourData.Add(actionId, bData);
        }
    }

    public BehaviourData GetBehaviour(int behaviourID)
    {
        return behaviourData[behaviourID];
    }
}

[System.Serializable]
public class BehaviourData
{
    public string actionName;
    public int actionType;
}

[System.Serializable]
public class BehaviourType1 : BehaviourData
{
    //string animationName;
    public int actionGoal;
}

[System.Serializable]
public class BehaviourType2 : BehaviourData
{
    public Vector3 actionGoal;
}

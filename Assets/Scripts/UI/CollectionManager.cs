using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

[System.Serializable]
public class Collections
{
    public string name;
    public string explain;
}

[System.Serializable]
public class CollectionsList
{
    public List<Collections> Collections = new List<Collections>();
}

public class CollectionManager : MonoBehaviour,IPointerEnterHandler
{
    private CollectionsList CollectionsList = new CollectionsList();

    public Text c_name;
    public Text c_explain;

    public int c_number;

    void Start()
    {
        TextAsset asset = Resources.Load("Collections") as TextAsset;
        if (asset != null)
        {
            CollectionsList = JsonUtility.FromJson<CollectionsList>(asset.text);
        }
        else
        {
            print("Asset is null");
        }
    }

    public void collection_show(int number)
    {
        c_name.text = CollectionsList.Collections[number].name;
        c_explain.text = CollectionsList.Collections[number].explain;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.tag == "Collection")
        {
            collection_show(c_number);
        }
    }
}

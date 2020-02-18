using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

[System.Serializable]
public class Collection
{
    public string name;
    public string explain;
}

[System.Serializable]
public class CollectionList
{
    public List<Collection> Collections = new List<Collection>();
}

public class CollectionManager : MonoBehaviour,IPointerEnterHandler
{
    private CollectionList CollectionList = new CollectionList();

    public Text c_name;
    public Text c_explain;

    public int c_number;

    void Start()
    {
        TextAsset asset = Resources.Load("Collections") as TextAsset;
        if (asset != null)
        {
            CollectionList = JsonUtility.FromJson<CollectionList>(asset.text);
        }
        else
        {
            print("Asset is null");
        }
    }

    public void collection_show(int number)
    {
        c_name.text = CollectionList.Collections[number].name;
        c_explain.text = CollectionList.Collections[number].explain;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.tag == "Collection")
        {
            collection_show(c_number);
        }
    }
}

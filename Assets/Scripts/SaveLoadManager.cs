using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public string sceneName;
    public GameObject player;

    static private bool objectLoad = false;

    void Update()
    {
        if (player == null && objectLoad)
        {
            player = GameObject.FindWithTag("Player");
            print(objectLoad);
        }
        else if (objectLoad)
        {
            ObjectLoad();
            objectLoad = false;
        }
        else
        {
            ;
        }
    }

    public void Save()
    {
        ES2.Save(sceneName, "sceneName.txt");
        ES2.Save(player.transform.position, "playerPosition.txt");
    }

    public void MapLoad()
    {
        sceneName = ES2.Load<string>("sceneName.txt");
        SceneManager.LoadScene(sceneName);
        SoundManager.instance.PlayBgm(sceneName);
        objectLoad = true;
    }

    public void ObjectLoad()
    {
        player.transform.position = ES2.Load<Vector3>("playerPosition.txt");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public int NowHp;
    public int NowEnergy;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(this != instance) Destroy(gameObject);
        
    }
    void Start()
    {
        NowEnergy = 5;
        NowHp = 5;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void StartButtom()
    {
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
}


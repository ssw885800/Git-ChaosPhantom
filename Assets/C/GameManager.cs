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

    [Header("死亡面板")]
    public GameObject DeathCavas;

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
        DeathCavas.SetActive(false);//遊戲開始時關閉死亡面板
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(NowHp<=0)//當HP歸零時觸發死亡介面
        {
            Death();
        }
    }
    public void StartButtom()
    {
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
    void Death () //死亡面板
    {
        Time.timeScale = 0;
        DeathCavas.SetActive(true);
    }
    public void Reset()
    {
        Debug.Log("重來");
        
    }
}


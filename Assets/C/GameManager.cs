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

    [Header("���`���O")]
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
        DeathCavas.SetActive(false);//�C���}�l���������`���O
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(NowHp<=0)//��HP�k�s��Ĳ�o���`����
        {
            Death();
        }
    }
    public void StartButtom()
    {
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }
    void Death () //���`���O
    {
        Time.timeScale = 0;
        DeathCavas.SetActive(true);
    }
    public void Reset()
    {
        Debug.Log("����");
        
    }
}


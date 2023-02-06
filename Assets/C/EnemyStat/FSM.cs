using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum StateType //狀態類別註冊(枚舉類型)
{ 
    Idle,Teleport,AttackPrepair,Attack
}
[Serializable]
public class Parameter //基本參數定義，並由這個類統一管理
{
    public int Health;                  //生命值
    public float IdleTime;              //待機時間
    public float TeleportCD;            //傳送冷卻時間
    public bool A = false;              //傳送執行開關
    public Animator Animator;           //獲取動畫管理器
    public Transform transform;         //獲取自身座標向量
    public Transform[] TeleportPoint;   //從傳送點列表中獲取座標向量
    public GameObject TeleportTarget;   //傳送目標
}
public class FSM : MonoBehaviour
{
    public Parameter parameter;
    private IState currentState;    //所有實現這個IState類(class)，都可
    private Dictionary<StateType,IState> States = new Dictionary<StateType, IState>();  //聲明一個字典來方便查找狀態(字典的Key值為上面創建的StateType)
    void Start()
    {
        parameter.Animator = GetComponent<Animator>();  //獲取動畫管理器
        parameter.transform = GetComponent<Transform>();
        States.Add(StateType.Idle, new IdleState(this));            //新增字典內鍵值對-待機(將IdleState.cs裡的IdleState註冊進來其餘狀態同理)
        States.Add(StateType.Teleport, new TeleportState(this));    //新增字典內鍵值對-傳送
        //States.Add(StateType.Attack, new AttackState(this));

        TransitionState(StateType.Idle);                            //在開始時將狀態切換為待機

        
    }

    void Update()
    {
        currentState.onUpdate();     // 每次執行當下狀態所需執行的代碼(目前當下狀態引用自IdleState.cs裡)
    }
    public void TransitionState(StateType type)     //狀態切換的函數(方法)
    {
        if (currentState!= null)
            currentState.onExit();  //如果有前一個狀態，先執行前一段狀態的結束函數
        currentState=States[type];  //切換狀態為給定狀態
        currentState.onEnter();     //執行新狀態
    }
    public void FlipTo(Transform target) //看向玩家得函數(方法)
    {
        if (target != null)
        {
            if(transform.position.x >target.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
        }
    }

    public void TT() //傳送動畫觸發傳送開關
    {
        parameter.A = true;
        Debug.Log("觸發");
    }
}

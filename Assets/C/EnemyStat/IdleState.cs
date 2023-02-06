using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;

    public class IdleState : IState //待機狀態(請注意這裡的狀態記得到FSM裡進行註冊
{
    private FSM manager;         //引用當前物件的狀態機
    private Parameter parameter; //取得當前物件的參數(例子:E-10)
    private float timer;         //待機計時器

    public IdleState(FSM manager)   
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void onEnter()   //狀態觸發時執行
    {
        parameter.Animator.Play("E-10_Idle");
        
    }
    public void onUpdate()  //狀態運行中執行
    {
        timer += Time.deltaTime;
       if (timer >= parameter.IdleTime)
        {
            manager.TransitionState(StateType.Teleport);
            
        }   
        
        

    }
    public void onExit()    //狀態退出後執行
    {
        timer = 0;
    }
}
public class TeleportState : IState //傳送狀態
{
    FSM manager;
    private Parameter parameter;

    private int TeleportPosition = 0;
    public TeleportState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void onEnter()
    {
        parameter.Animator.Play("E-10_Teleport");
    }
    public void onUpdate()
    {
        
        if (parameter.A = true )
        {
            parameter.transform.position = new Vector3(parameter.TeleportPoint[TeleportPosition].position.x, parameter.transform.position.y, parameter.transform.position.z);
            manager.TransitionState(StateType.Idle);    // Debug.Log("啟用中");
        }
    }
    public void onExit()
    {
        parameter.A = false;    //傳送開關關閉
        TeleportPosition += Random.Range(0,2) ;     //結束時隨機選擇傳送點，並使下一次傳送點不會重複(當前傳送點ID加上隨機值=下一個傳送點ID，如果值大於清單總ID數將ID歸0)
        if (TeleportPosition >= parameter.TeleportPoint.Length)
        {
            TeleportPosition = 0;        
        }
    }
}

public class AttackPrepairState : IState //攻擊準備
{
    FSM manager;
    Parameter parameter;
    public AttackPrepairState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void onEnter()
    {
        parameter.Animator.Play("E-10_AttackPrepair");
    }
    public void onUpdate()
    {

    }
    public void onExit()
    {

    }
}
public class AttackState : IState   //攻擊觸發
{
    FSM manager;
    Parameter parameter;
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void onEnter()
    {
        parameter.Animator.Play("E-10_Attack");
    }
    public void onUpdate()
    {
    }
        
    public void onExit()
    {


    }
    
}
public class nulls : IState
{
    FSM manager;
    Parameter parameter;
    public nulls(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void onEnter()
    {
        parameter.Animator.Play("E-10_X");
    }
    public void onUpdate()
    {

    }
    public void onExit()
    {

    }
}

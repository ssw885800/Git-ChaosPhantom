using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pc_Shadow : MonoBehaviour
{  
    [SerializeField]
    GameManager gameManager;

    public LayerMask ground;
    public CapsuleCollider2D GroundHit;
    public CircleCollider2D ShadowHit;
    public BoxCollider2D ShadowAttack;
    public Rigidbody2D shadowRig;
    public Text HpText;

    public float MoveSpeed;
    public float SquMoveSpeed;
    public float jumpHigh;
    public float MaxJumpTime;
    public float DashSpeed;
    public float DashTime;
    public float DashCD;
    public float AttackCD;
    public float ReHpTime;
    public float EnergyRemainTime;
    public int MaxHp;
    public int MaxEnergy;

    public GameObject dashObj;//衝刺殘影物件
    public Animator animator;

    float MoveDir;
    float jumpTime;
    float Fall = 0;
    float DashTimeRemain;
    float DashRemain;
    float AttackRemain;
    float EnergyRemain;
    float ReHp;

    float StartAttack = 0.11f;
    float AttackTime = 0.19f;

    bool SquCheck = false;
    bool groundCheck = false;
    bool JumpKeep= false;
    bool JumpCheck = false;
    bool JumpToFallCheck = false;
    bool FallCheck = false;
    bool isDash = false;
    bool canHurt = true;
    bool isHurt;
    bool isLight;
    bool EnergyIsMax;
    bool Attacking;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        ReHp = ReHpTime;
        EnergyRemain = EnergyRemainTime;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(gameManager == null ) gameManager = FindObjectOfType<GameManager>();
        GroundCheck();
        EnergyRecovery();
        HpRecovery();
        if (!isHurt)
        {
            SquSwich();
            Jump();
            Dash();
            Attack();
        }
        HpText.text = "Hp:" + gameManager.NowHp + "/" + MaxHp + "\n" + "Energy:" + gameManager.NowEnergy + "/" + MaxEnergy;
    }
    void FixedUpdate()
    {
        if (!isHurt && !isDash && !Attacking) Move();
        Flip();
        JumpToFall();
        FallingFunction();
        HurtSwich();
        AnimationSwich();
    }
    void Move()     //移動
    {
        MoveDir = Input.GetAxis("Horizontal");  //取得左右移動按鍵
        if (!SquCheck) shadowRig.velocity = new Vector2(MoveDir * MoveSpeed, shadowRig.velocity.y);      //一般移速
        if (SquCheck) shadowRig.velocity = new Vector2(MoveDir * SquMoveSpeed, shadowRig.velocity.y);   //蹲下移速
    }
    void SquSwich()  //蹲下
    {

        if (Input.GetKey(KeyCode.S) && groundCheck)   //當按下S鍵
        {
            SquCheck = true;
            ShadowHit.enabled = false;    //關閉剛體
        }
        if (Input.GetKeyUp(KeyCode.S))                        //當放開S鍵
        {
            SquCheck = false;
            ShadowHit.enabled = true;     //開啟剛體
        }
    }
    void Jump()     //跳躍
    {
        Vector2 jumpVel = new Vector2(0.0f, jumpHigh);   //跳躍前置轉換
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck && !SquCheck)
        {
            JumpCheck = true;
            JumpKeep = true;
            jumpTime = MaxJumpTime;
            shadowRig.velocity = Vector2.up * jumpVel;       //跳躍高度
        }
        if (Input.GetKey(KeyCode.Space) && JumpKeep)
        {
            if(jumpTime > 0)
            {
                shadowRig.velocity = Vector2.up * jumpVel;
                jumpTime -= Time.deltaTime;
            }
            else JumpKeep = false; 
        }
        if (Input.GetKeyUp(KeyCode.Space)) JumpKeep = false;
    }
    void JumpToFall()   //跳躍至墜落轉換
    {
        Fall = shadowRig.velocity.y;           //當前垂直受力
        if (Fall <= 1 && !groundCheck && !isDash)
        {
            JumpToFallCheck = true;
            JumpCheck = false;
        }
    }
    void FallingFunction()  //墜落
    {
        if (Fall < 0 && !groundCheck && JumpToFallCheck)
        {
            JumpToFallCheck = false;
            FallCheck = true;
        }
    }
    void GroundCheck()   //地板檢測
    {
        if (GroundHit.IsTouchingLayers(ground)) { groundCheck = true; FallCheck = false; }   //目標是否碰觸地面圖層
        else groundCheck = false;
    }
    void Dash() //衝刺
    {
        if (!isDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Time.time >= DashRemain + DashCD)
                {
                    isDash = true;
                    canHurt = false;
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("Enemy"),true);
                    DashTimeRemain = DashTime;     //設置衝刺持續間
                    DashRemain = Time.time;        //記錄當下使用時間
                }
            }
        }
        else
        {
            DashTimeRemain -= Time.deltaTime;                                       //衝刺持續時間倒數
            if (DashTimeRemain <= 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("Enemy"), false);
                isDash = false;
                canHurt = true;
                dashObj.SetActive(false);
            }
            else
            {
                shadowRig.velocity = transform.right * DashSpeed;//衝刺速度
            }
        }
    }
    void OnCollisionStay2D(Collision2D collision)                                       // 碰撞
    {
        if (canHurt && collision.gameObject.tag == "Enemy") gameManager.NowHp -= 1;             //扣血
        if (collision.gameObject.tag == "Enemy" && canHurt)                                     
        {
            if (shadowRig.transform.position.x < collision.gameObject.transform.position.x)     //判斷受傷方向
            {
                shadowRig.velocity = new Vector2(-10, shadowRig.velocity.y + 5);                //擊退
                canHurt = false;
                isHurt = true;
            }
            else if (shadowRig.transform.position.x > collision.gameObject.transform.position.x)
            {
                shadowRig.velocity = new Vector2(10, shadowRig.velocity.y + 5);
                canHurt = false;
                isHurt = true;
            }
        }                                  //受到傷害(敵人)
    }
    void HurtSwich() //受傷狀態切換
    {
        if (Mathf.Abs(shadowRig.velocity.x) <= 0.1f && isHurt)
        {
            canHurt = true;
            isHurt = false;
        }
    }
    void Flip()     //翻轉
    {
        bool shadowdir = Mathf.Abs(shadowRig.velocity.x) > Mathf.Epsilon;     //防止抖動翻轉及停止時彈回另一側
        if (shadowdir)
        {
            if (shadowRig.velocity.x > 0.1f) transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (shadowRig.velocity.x < -0.1f) transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void AnimationSwich() //動畫切換管理
    {
        if (MoveDir != 0 && !JumpCheck && groundCheck && !isDash && !Attacking) animator.Play("Pc_Move");                  //移動動畫
        if (MoveDir == 0 && !JumpCheck && groundCheck && !isDash && !Attacking) animator.Play("Pc_Idle");                  //待機動畫
        if (JumpCheck && !isDash && !Attacking) animator.Play("Pc_JumpUpMove");                                            //往上跳躍動畫
        if (JumpToFallCheck && !isDash && !groundCheck && !JumpCheck && !Attacking) animator.Play("Pc_JumpDownMove");      //跳躍掉落動畫
        if (isDash && DashTimeRemain >0.15f) animator.Play("Pc_Dash");                                                      //衝刺動畫
        if (Attacking) animator.Play("Pc_Attack");
    }
    void EnergyRecovery() //能量回復
    {
        if (gameManager.NowEnergy < MaxEnergy)
        {
            EnergyIsMax = false;
        }
        else if (gameManager.NowEnergy >= MaxEnergy)
        {
            EnergyIsMax = true;
        }
        if (!EnergyIsMax)
        {
            EnergyRemain -= Time.deltaTime;
            if (EnergyRemain <= 0)
            {
                gameManager.NowEnergy++;
                EnergyRemain = EnergyRemainTime;
            }
        }
    }
    void HpRecovery() //耗藍回血
    {
        if(Input.GetKey(KeyCode.R) && gameManager.NowEnergy >= 2 && gameManager.NowHp < MaxHp)
        {
            ReHp -= Time.deltaTime;
            if (ReHp <= 0)
            {
                gameManager.NowHp++;
                gameManager.NowEnergy -= 2;
                ReHp = ReHpTime;
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            ReHp = ReHpTime;
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isDash)
        {
            if(Time.time >= AttackRemain + AttackCD)
            {
                Attacking = true;
                AttackRemain = Time.time;
                StartCoroutine(StartAttacking());
                StartCoroutine(DisabalHitbox());
            }          
        }
    }
    IEnumerator StartAttacking()
    {
        yield return new WaitForSeconds(StartAttack);
        ShadowAttack.enabled = true;
    }
    IEnumerator DisabalHitbox()
    {
        yield return new WaitForSeconds(AttackTime);
        ShadowAttack.enabled = false;
        Attacking = false;
    }
}
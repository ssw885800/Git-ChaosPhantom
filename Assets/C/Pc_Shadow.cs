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

    public GameObject dashObj;//�Ĩ�ݼv����
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
    void Move()     //����
    {
        MoveDir = Input.GetAxis("Horizontal");  //���o���k���ʫ���
        if (!SquCheck) shadowRig.velocity = new Vector2(MoveDir * MoveSpeed, shadowRig.velocity.y);      //�@�벾�t
        if (SquCheck) shadowRig.velocity = new Vector2(MoveDir * SquMoveSpeed, shadowRig.velocity.y);   //�ۤU���t
    }
    void SquSwich()  //�ۤU
    {

        if (Input.GetKey(KeyCode.S) && groundCheck)   //����US��
        {
            SquCheck = true;
            ShadowHit.enabled = false;    //��������
        }
        if (Input.GetKeyUp(KeyCode.S))                        //���}S��
        {
            SquCheck = false;
            ShadowHit.enabled = true;     //�}�ҭ���
        }
    }
    void Jump()     //���D
    {
        Vector2 jumpVel = new Vector2(0.0f, jumpHigh);   //���D�e�m�ഫ
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck && !SquCheck)
        {
            JumpCheck = true;
            JumpKeep = true;
            jumpTime = MaxJumpTime;
            shadowRig.velocity = Vector2.up * jumpVel;       //���D����
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
    void JumpToFall()   //���D�ܼY���ഫ
    {
        Fall = shadowRig.velocity.y;           //��e�������O
        if (Fall <= 1 && !groundCheck && !isDash)
        {
            JumpToFallCheck = true;
            JumpCheck = false;
        }
    }
    void FallingFunction()  //�Y��
    {
        if (Fall < 0 && !groundCheck && JumpToFallCheck)
        {
            JumpToFallCheck = false;
            FallCheck = true;
        }
    }
    void GroundCheck()   //�a�O�˴�
    {
        if (GroundHit.IsTouchingLayers(ground)) { groundCheck = true; FallCheck = false; }   //�ؼЬO�_�IĲ�a���ϼh
        else groundCheck = false;
    }
    void Dash() //�Ĩ�
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
                    DashTimeRemain = DashTime;     //�]�m�Ĩ����
                    DashRemain = Time.time;        //�O����U�ϥήɶ�
                }
            }
        }
        else
        {
            DashTimeRemain -= Time.deltaTime;                                       //�Ĩ����ɶ��˼�
            if (DashTimeRemain <= 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("Enemy"), false);
                isDash = false;
                canHurt = true;
                dashObj.SetActive(false);
            }
            else
            {
                shadowRig.velocity = transform.right * DashSpeed;//�Ĩ�t��
            }
        }
    }
    void OnCollisionStay2D(Collision2D collision)                                       // �I��
    {
        if (canHurt && collision.gameObject.tag == "Enemy") gameManager.NowHp -= 1;             //����
        if (collision.gameObject.tag == "Enemy" && canHurt)                                     
        {
            if (shadowRig.transform.position.x < collision.gameObject.transform.position.x)     //�P�_���ˤ�V
            {
                shadowRig.velocity = new Vector2(-10, shadowRig.velocity.y + 5);                //���h
                canHurt = false;
                isHurt = true;
            }
            else if (shadowRig.transform.position.x > collision.gameObject.transform.position.x)
            {
                shadowRig.velocity = new Vector2(10, shadowRig.velocity.y + 5);
                canHurt = false;
                isHurt = true;
            }
        }                                  //����ˮ`(�ĤH)
    }
    void HurtSwich() //���˪��A����
    {
        if (Mathf.Abs(shadowRig.velocity.x) <= 0.1f && isHurt)
        {
            canHurt = true;
            isHurt = false;
        }
    }
    void Flip()     //½��
    {
        bool shadowdir = Mathf.Abs(shadowRig.velocity.x) > Mathf.Epsilon;     //����ݰ�½��ΰ���ɼu�^�t�@��
        if (shadowdir)
        {
            if (shadowRig.velocity.x > 0.1f) transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (shadowRig.velocity.x < -0.1f) transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void AnimationSwich() //�ʵe�����޲z
    {
        if (MoveDir != 0 && !JumpCheck && groundCheck && !isDash && !Attacking) animator.Play("Pc_Move");                  //���ʰʵe
        if (MoveDir == 0 && !JumpCheck && groundCheck && !isDash && !Attacking) animator.Play("Pc_Idle");                  //�ݾ��ʵe
        if (JumpCheck && !isDash && !Attacking) animator.Play("Pc_JumpUpMove");                                            //���W���D�ʵe
        if (JumpToFallCheck && !isDash && !groundCheck && !JumpCheck && !Attacking) animator.Play("Pc_JumpDownMove");      //���D�����ʵe
        if (isDash && DashTimeRemain >0.15f) animator.Play("Pc_Dash");                                                      //�Ĩ�ʵe
        if (Attacking) animator.Play("Pc_Attack");
    }
    void EnergyRecovery() //��q�^�_
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
    void HpRecovery() //���Ŧ^��
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
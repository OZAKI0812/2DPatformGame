using UnityEngine;
using UnityEngine.InputSystem;

public class PandaController : MonoBehaviour
{
    private Rigidbody2D rbody;
    public float speed = 3.0f;
    public float jump = 9.0f;
    private float move;
    public LayerMask groundLayer;
    private bool goJump = false;
    private InputAction moveAct;
    private InputAction jumpAct;
    private Animator animator;
    private int nowAnime = 0;
    private int prevAnime = 0;
    public static string gameState;
    public int coinNum = 0;
    public int score = 0;

    public AudioSource jumpAudioSource;
    public AudioClip    jumpAudioClip;

    public AudioSource  damageAudioSource;
    public AudioClip    damageAudioClip;

    public AudioSource  coinAudioSource;
    public AudioClip    coinAudioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�v���C���[��Rigidbody2D��Animator�̎��̂��擾
        rbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        //InputAction����Move�����Jump�̃A�N�V�������擾
        moveAct = InputSystem.actions.FindAction("Move");
        jumpAct = InputSystem.actions.FindAction("Jump");
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }
        var rlValue = moveAct.ReadValue<Vector2>();
        move = rlValue.x;
        if (move > 0.0f)
        {
            transform.localScale = new Vector2(-0.25f, 0.25f);
        }
        else if (move < 0.0f)
        {
            transform.localScale = new Vector2(0.25f, 0.25f);
        }
        var jumpValue = jumpAct.ReadValue<float>();
        if (jumpValue > 0.0f)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        bool onGround = Physics2D.CircleCast(transform.position,
            0.15f,
            Vector2.down,
            0.0f,
            groundLayer);
        if (onGround || move != 0.0f)
        {
            rbody.linearVelocity = new Vector2(move * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)
        {
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
        if (onGround)
        {
            if (move == 0.0f)
            {
                nowAnime = 0;
            }
            else
            {
                nowAnime = 1;
            }
        }
        else
        {
            nowAnime = 2;
        }
        if (nowAnime != prevAnime)
        {
            prevAnime = nowAnime;
            animator.SetInteger("State", nowAnime);
        }
    }
    void Jump()
    {
        goJump = true;
    }

        //  �����蔻��
        void OnTriggerEnter2D(Collider2D collision)
    {
        //  ���������̂�Goal�̃^�O���t�����I�u�W�F�N�g�̂Ƃ�
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        //  ���������̂�Dead�̃^�O���t�����I�u�W�F�N�g�̂Ƃ�
        else if (collision.gameObject.tag == "Dead")
        {
            damageAudioSource.PlayOneShot(damageAudioClip);
            GameOver();
        }
        //  Item�^�O�̕��̂ƏՓ˂����Ƃ�
        else if (collision.gameObject.tag == "Item")
        {
            if (collision.gameObject.name == "Coin")
            {
                //  �A�C�e���̃X�N���v�g���擾
                CoinData coin = collision.gameObject.GetComponent<CoinData>();
                coinNum += 1;
                score += coin.score;

                coinAudioSource.PlayOneShot(coinAudioClip);

                //  �A�C�e�����폜
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.name == "Spring")
            {
                jumpAudioSource.PlayOneShot(jumpAudioClip);
                SpringData spring = collision.gameObject.GetComponent<SpringData>();

                // ������ɏ������i�������x�𖳎����邽�߁j
                rbody.linearVelocity= new Vector2(rbody.linearVelocity.x, 0);

                // �����Ɉ�C�ɃW�����v�͂�������
                rbody.AddForce(new Vector2(0, spring.jumpForce), ForceMode2D.Impulse);
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.name == "Slime")
            {
                SlimData enemy = collision.gameObject.GetComponent<SlimData>();
                score += enemy.score;
            }
            else if (collision.gameObject.name == "Bee")
            {
                BeeData enemy = collision.gameObject.GetComponent<BeeData>();
                score += enemy.score;
            }
            damageAudioSource.PlayOneShot(damageAudioClip);
            //  �폜
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(collision.gameObject);
        }
    }
    //�Q�[���I�[�o�[����
    public void GameOver()
    {
        //�Q�[���̏�Ԃ�ύX
        gameState = "gameover";
        //�Q�[����~
        GameStop();
        //�Q�[���I�[�o�[���o
        //�@�v���C���[�̓����蔻�������
        GetComponent<CapsuleCollider2D>().enabled = false;
        //�A�v���C���[��������ɒ��ˏグ�鉉�o
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    //�S�[������
    public void Goal()
    {
        //�Q�[���̏�Ԃ�ύX
        gameState = "stageclear";
        //�Q�[����~
        GameStop();                 
    }
    //�Q�[����~
    void GameStop()
    {
        //�v���C���[��Rigidbody2D���擾
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        //���x��0�ɂ��ċ�����~
        rbody.linearVelocity = new Vector2(0, 0);
    }
}

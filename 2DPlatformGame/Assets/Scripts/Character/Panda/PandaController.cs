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
    public int score = 0;
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
            GameOver();
        }
        //  Item�^�O�̕��̂ƏՓ˂����Ƃ�
        else if (collision.gameObject.tag == "Item")
        {
            //  �A�C�e���̃X�N���v�g���擾
            CoinData coin = collision.gameObject.GetComponent<CoinData>();
            //  �A�C�e���̃X�R�A�����Z
           score += coin.score;
            //  �A�C�e�����폜
            Destroy(collision.gameObject);
        }
    }
    //�Q�[���I�[�o�[����
    public void GameOver()
    {
        gameState = "gameover"; //�Q�[���̏�Ԃ�ύX
        GameStop();             //�Q�[����~
        //�Q�[���I�[�o�[���o
        //�@�v���C���[�̓����蔻�������
        GetComponent<CapsuleCollider2D>().enabled = false;
        //�A�v���C���[��������ɒ��ˏグ�鉉�o
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    //�S�[������
    public void Goal()
    {
        gameState = "stageclear";    //�Q�[���̏�Ԃ�ύX
        GameStop();                 //�Q�[����~
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

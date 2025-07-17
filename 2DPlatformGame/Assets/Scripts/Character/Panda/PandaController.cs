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
        //プレイヤーのRigidbody2DやAnimatorの実体を取得
        rbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        //InputActionからMoveおよびJumpのアクションを取得
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

    //  当たり判定
    void OnTriggerEnter2D(Collider2D collision)
    {
        //  当たったのがGoalのタグが付いたオブジェクトのとき
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        //  当たったのがDeadのタグが付いたオブジェクトのとき
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        //  Itemタグの物体と衝突したとき
        else if (collision.gameObject.tag == "Item")
        {
            //  アイテムのスクリプトを取得
            CoinData coin = collision.gameObject.GetComponent<CoinData>();
            //  アイテムのスコアを加算
           score += coin.score;
            //  アイテムを削除
            Destroy(collision.gameObject);
        }
    }
    //ゲームオーバー処理
    public void GameOver()
    {
        gameState = "gameover"; //ゲームの状態を変更
        GameStop();             //ゲーム停止
        //ゲームオーバー演出
        //①プレイヤーの当たり判定を消す
        GetComponent<CapsuleCollider2D>().enabled = false;
        //②プレイヤーを少し上に跳ね上げる演出
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    //ゴール処理
    public void Goal()
    {
        gameState = "stageclear";    //ゲームの状態を変更
        GameStop();                 //ゲーム停止
    }
    //ゲーム停止
    void GameStop()
    {
        //プレイヤーのRigidbody2Dを取得
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        //速度を0にして強制停止
        rbody.linearVelocity = new Vector2(0, 0);
    }
}

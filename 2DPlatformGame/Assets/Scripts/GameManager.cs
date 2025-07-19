using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //  画像のGameObject
    public GameObject               mainImage;
    //  リスタート用ボタンオブジェクト
    public GameObject               nextButton;
    public GameObject               restartButton;
    //  GAMEOVER画像
    public Sprite                   gameOverSpr;
    //  STAGECLEAR画像
    public Sprite                   stageClearSpr;
    //  TMProのテキスト
    public TMPro.TextMeshProUGUI    coinText;
    public TMPro.TextMeshProUGUI    ScoreText;
    //  ステージのスコア
    public int                      coin = 0;   
    public int                      stageScore = 0;

    public AudioSource              audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainImage.SetActive(false);

        nextButton.SetActive(false);
        restartButton.SetActive(false);

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //  ステージクリア
        if (PandaController.gameState == "stageclear")
        {
            mainImage.SetActive(true);
            mainImage.GetComponent<Image>().sprite = stageClearSpr;

            nextButton.SetActive(true);

            PandaController.gameState = "gameend";
        }
        //  ゲームオーバー
        else if (PandaController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            restartButton.SetActive(true);

            audioSource.Stop();

            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PandaController.gameState = "gameend";
        }
        else if (PandaController.gameState == "playing")
        {
            //  PlayerControllerを取得する
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PandaController playerController = player.GetComponent<PandaController>();
            if (playerController.coinNum != 0)
            {
                coin += playerController.coinNum;
                playerController.coinNum = 0;
                UpdateScore();
            }
            if (playerController.score != 0)
            {
                //  PandaControllerのScore値が0でなければstageScoreに加算
                stageScore += playerController.score;
                playerController.score = 0;
                UpdateScore();
            }

            //  スコアの更新
            UpdateScore();
        }
    }
    void UpdateScore()
    {
        //   TextMeshProのテキストの内容を更新
        coinText.SetText(coin.ToString());
        ScoreText.SetText(stageScore.ToString());
    }
}

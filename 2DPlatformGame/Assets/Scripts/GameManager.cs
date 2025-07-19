using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //  �摜��GameObject
    public GameObject               mainImage;
    //  ���X�^�[�g�p�{�^���I�u�W�F�N�g
    public GameObject               nextButton;
    public GameObject               restartButton;
    //  GAMEOVER�摜
    public Sprite                   gameOverSpr;
    //  STAGECLEAR�摜
    public Sprite                   stageClearSpr;
    //  TMPro�̃e�L�X�g
    public TMPro.TextMeshProUGUI    coinText;
    public TMPro.TextMeshProUGUI    ScoreText;
    //  �X�e�[�W�̃X�R�A
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
        //  �X�e�[�W�N���A
        if (PandaController.gameState == "stageclear")
        {
            mainImage.SetActive(true);
            mainImage.GetComponent<Image>().sprite = stageClearSpr;

            nextButton.SetActive(true);

            PandaController.gameState = "gameend";
        }
        //  �Q�[���I�[�o�[
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
            //  PlayerController���擾����
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
                //  PandaController��Score�l��0�łȂ����stageScore�ɉ��Z
                stageScore += playerController.score;
                playerController.score = 0;
                UpdateScore();
            }

            //  �X�R�A�̍X�V
            UpdateScore();
        }
    }
    void UpdateScore()
    {
        //   TextMeshPro�̃e�L�X�g�̓��e���X�V
        coinText.SetText(coin.ToString());
        ScoreText.SetText(stageScore.ToString());
    }
}

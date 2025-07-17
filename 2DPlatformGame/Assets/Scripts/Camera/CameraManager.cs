using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //カメラが移動可能な範囲を設定する
    //この範囲外は見せないようにするための設定
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;
    //強制横スクロール用フラグとスクロール速度
    public bool isScrollX = false;
    public float scrollSpeedX = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //"Player"というタグのついているオブジェクトを探してplayerに格納
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            //プレイヤーのX,Y座標を取得
            float px = player.transform.position.x;
            float py = player.transform.position.y;
            //カメラのZ座標を取得
            float cz = this.transform.position.z;

            //強制横スクロール処理
            if (isScrollX)
            {
                px = this.transform.position.x + (scrollSpeedX * Time.deltaTime);
            }

            //左右両端に移動制限をつけて画面外を表示しない
            if (px < leftLimit)
            {
                px = leftLimit;
            }
            else if (px > rightLimit)
            {
                px = rightLimit;
            }
            //上下両端に移動制限をつけて画面外を表示しない
            if (py > topLimit)
            {
                py = topLimit;
            }
            else if (py < bottomLimit)
            {
                py = bottomLimit;
            }
            //プレイヤーの位置をもとに、カメラ位置を更新するためのVector3値を生成して適用
            Vector3 v3 = new Vector3(px, py, cz);
            this.transform.position = v3;
        }
    }

    void FixedUpdate()
    {

    }
}

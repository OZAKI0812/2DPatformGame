using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class FishController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool goingUp = true;

    void Start()
    {
        // transformやアクティブ状態のチェックも一応入れておこう
        if (transform != null && gameObject.activeInHierarchy)
        {
            MoveYLoop();
        }
    }

    void MoveYLoop()
    {
        float distanceY = 7.5f;
        float duration = Random.Range(1.0f, 3.0f); // ランダム時間（自然感UP）

        // 向きに応じてスケール変更
        Vector3 scale = transform.localScale;
        scale.y = goingUp ? -0.75f : 0.75f;
        transform.localScale = scale;

        // 移動方向決定
        float moveY = goingUp ? distanceY : -distanceY;

        // アニメーション開始（ループっぽく再帰）
        transform.DOLocalMoveY(transform.localPosition.y + moveY, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                goingUp = !goingUp;
                MoveYLoop(); // 次のループ呼び出し
            });
    }

    // Update is called once per frame
    void Update()
    {
    }
}

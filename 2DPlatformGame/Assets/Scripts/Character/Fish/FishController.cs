using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class FishController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool goingUp = true;

    void Start()
    {
        // transform��A�N�e�B�u��Ԃ̃`�F�b�N���ꉞ����Ă�����
        if (transform != null && gameObject.activeInHierarchy)
        {
            MoveYLoop();
        }
    }

    void MoveYLoop()
    {
        float distanceY = 7.5f;
        float duration = Random.Range(1.0f, 3.0f); // �����_�����ԁi���R��UP�j

        // �����ɉ����ăX�P�[���ύX
        Vector3 scale = transform.localScale;
        scale.y = goingUp ? -0.75f : 0.75f;
        transform.localScale = scale;

        // �ړ���������
        float moveY = goingUp ? distanceY : -distanceY;

        // �A�j���[�V�����J�n�i���[�v���ۂ��ċA�j
        transform.DOLocalMoveY(transform.localPosition.y + moveY, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                goingUp = !goingUp;
                MoveYLoop(); // ���̃��[�v�Ăяo��
            });
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //�J�������ړ��\�Ȕ͈͂�ݒ肷��
    //���͈̔͊O�͌����Ȃ��悤�ɂ��邽�߂̐ݒ�
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;
    //�������X�N���[���p�t���O�ƃX�N���[�����x
    public bool isScrollX = false;
    public float scrollSpeedX = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //"Player"�Ƃ����^�O�̂��Ă���I�u�W�F�N�g��T����player�Ɋi�[
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            //�v���C���[��X,Y���W���擾
            float px = player.transform.position.x;
            float py = player.transform.position.y;
            //�J������Z���W���擾
            float cz = this.transform.position.z;

            //�������X�N���[������
            if (isScrollX)
            {
                px = this.transform.position.x + (scrollSpeedX * Time.deltaTime);
            }

            //���E���[�Ɉړ����������ĉ�ʊO��\�����Ȃ�
            if (px < leftLimit)
            {
                px = leftLimit;
            }
            else if (px > rightLimit)
            {
                px = rightLimit;
            }
            //�㉺���[�Ɉړ����������ĉ�ʊO��\�����Ȃ�
            if (py > topLimit)
            {
                py = topLimit;
            }
            else if (py < bottomLimit)
            {
                py = bottomLimit;
            }
            //�v���C���[�̈ʒu�����ƂɁA�J�����ʒu���X�V���邽�߂�Vector3�l�𐶐����ēK�p
            Vector3 v3 = new Vector3(px, py, cz);
            this.transform.position = v3;
        }
    }

    void FixedUpdate()
    {

    }
}

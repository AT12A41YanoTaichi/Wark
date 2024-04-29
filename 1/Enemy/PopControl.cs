using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�G�̃|�b�v���R�����[������

public class PopControl : MonoBehaviour
{

    [Header("�|�b�v����Ώۂ�����")]
    [SerializeField] private GameObject[] popTarget;

    [Header("�v���C���[�̎擾")]
    [SerializeField] private GameObject�@player;

    [Header("�|�b�v����ʒu")]
    [SerializeField] private GameObject popPos;

    [Header("�|�b�v���鐔")]
    [SerializeField] private int PopCount = 0;
    [Header("���|�b�v����܂ł̎���")]
    [SerializeField] private float popTime;
    private int Count = 3;

    private float time = 0.0f;
    [Header("PopEffect�̎擾")]
    [SerializeField] private GameObject PopEffect;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�Ƃ̋��������߂�
        var dis = (popPos.transform.position - player.transform.position).sqrMagnitude;
        dis = Mathf.Pow(dis, 0.5f);

        if (dis < 30.0f)
        {
            //�o����������菭�Ȃ�������
            if (PopCount > Count)
            {
                time += Time.deltaTime;
                if (popTime < time)
                {
                    Pop();
                    time = 0.0f;
                }

            }
        }

    }

    private void Pop()
    {
        if (popTarget.Length == 0)
        {
            return;
        }

        PopCount++;

        var popObj = Instantiate(popTarget[Random.Range(0, popTarget.Length - 1)]);

        var random = 10.0f * Random.insideUnitCircle;
        var circlePos = new Vector3(random.x, 0, random.y) + popPos.transform.position;
        
        var dis = (circlePos - popPos.transform.position).normalized;
        var pos = circlePos + dis * 5.0f;
        GameObject effect = Instantiate(PopEffect);
        effect.transform.position = pos;
        popObj.transform.position = pos;
        var rot = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        popObj.transform.rotation = rot;

        popObj.SetActive(true);
    }

}

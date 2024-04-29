using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideUIControl : MonoBehaviour
{
    public int state = 0;
    public bool loop = false;

    [Header("Text")]
    public Vector3 outPos01;
    public Vector3 inPos;
    public Vector3 outPos02;
    private float time;

    public bool SetPosY = false;
    public bool isDestroy = false;
    public float posY = 180;

    [Header("�X���C�h�C������܂ł̑҂�����")]
    public float InTime;

    [Header("�X���C�h�A�E�g����܂ł̑҂�����")]
    public float OutTime;

    [Header("Level UPUI�̃X���C�h�A�E�g�҂�����")]
    [SerializeField] private float OffActiveTime;

   public bool SaveUI = false;

    [Header("Level UPUI�Ȃ�true")]
    [SerializeField] private bool isLevelUPUI;

    // Start is called before the first frame update
    void Start()
    {
        if(!SaveUI)
        {
            StartCoroutine(StateChange());
        }
        
        if (transform.localPosition != outPos01)
        {
            transform.localPosition = outPos01;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //�����ʒu��ʃv���O�����Őݒ肷�邩
        if (SetPosY)
        {
            outPos01.y = posY;
            inPos.y = posY;
            outPos02.y = posY;
        }

        //�X���C�h�C���ʒu�Ɉړ�
        if (state == 0)
        {
            if (transform.localPosition != outPos01)
            {
                transform.localPosition = outPos01;
            }
            if (SaveUI)
            {
                time += Time.deltaTime;
                if (time >= InTime)
                {
                    state = 1;
                    time = 0.0f;
                }
            }
        }
        //�X���C�h�C��
        else if (state == 1)
        {

            if(inPos.x >= 0 &&
               inPos.y >= 0)
            {
                if(transform.localPosition.x > inPos.x + 1.0f &&
                   transform.localPosition.y > inPos.y + 1.0f &&
                   transform.localPosition.x > inPos.z + 1.0f)
                {
                    transform.localPosition = inPos;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, inPos, 4.0f * Time.unscaledDeltaTime);
                }
            }
            else
            {
                if (transform.localPosition.x < inPos.x - 1.0f &&
                    transform.localPosition.y < inPos.y - 1.0f &&
                    transform.localPosition.x < inPos.z - 1.0f)
                {
                    transform.localPosition = inPos;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, inPos, 4.0f * Time.unscaledDeltaTime);
                }
            }
            
            if (SaveUI)
            {
                time += Time.deltaTime;
                if (time >= OutTime) 
                {
                    state = 2;
                    time = 0.0f;
                }
            }
        }
        //�X���C�h�A�E�g
        else if (state == 2) 
        {

            if (outPos02.x > 0 &&
                outPos02.y > 0)
            {
                if (transform.localPosition.x > outPos02.x + 1.0f &&
                transform.localPosition.y > outPos02.y + 1.0f &&
                transform.localPosition.z > outPos02.z + 1.0f)
                {
                    transform.localPosition = outPos02;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, outPos02, 5.0f * Time.unscaledDeltaTime);
                }
            }
            else
            {
                if (transform.localPosition.x < outPos02.x - 1.0f &&
                transform.localPosition.y < outPos02.y - 1.0f &&
                transform.localPosition.z < outPos02.z- 1.0f)
                {
                    transform.localPosition = outPos02;
                    
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, outPos02, 5.0f * Time.unscaledDeltaTime);
                }
            }

            if (isLevelUPUI)
            {
                time += Time.deltaTime;
                if (time >= OffActiveTime)
                {
                    time = 0.0f;
                    gameObject.SetActive(false);
                }

            }
        }

        
    }

    public IEnumerator StateChange()
    {
        yield return new WaitForSeconds(InTime);
        state = 1;
        yield return new WaitForSeconds(OutTime);
        state = 2;
        yield return new WaitForSeconds(1.0f);
        if(isDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetPosY(float posy)
    {
        posY = posy;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Tips : MonoBehaviour, IUIState
{
    [Header("��������1Ϊ���ϣ�-1Ϊ���ϣ�")]
    public int direction = 1;
    [Header("x��������")]
    public float disx = 80;
    [Header("y��������")]
    public float disy = 15;
    [Header("������������ʱ��")]
    public float startt = 1;
    [Header("������������ʱ��")]
    public float endt = 1;
    public void OnHide()
    {
        transform.GetComponent<Image>().CrossFadeAlpha(0, endt, true);
        transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, endt, true);
    }

    public void OnShow()
    {
        transform.gameObject.SetActive(true);
        //transform.position = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(direction, 1, 1);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30 * direction));
        if (direction == -1)
            transform.GetChild(0).localScale = new Vector3(direction, 1, 1);
        transform.DOJump(transform.position + new Vector3(direction * disx, disy, 0), disy, 1, startt);
        transform.DORotate(new Vector3(0, 0, 0), startt).SetEase(Ease.OutSine);
    }

    public void Show()
    {
        ;
    }
}

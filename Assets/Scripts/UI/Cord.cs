using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cord : MonoBehaviour, IUIState
{
    Sequence seq;
    [Header("y�����ƾ���")]
    public float disy = 15;
    [Header("�½�ʱ��")]
    public float dropt = 0.5f;
    [Header("����ʱ��")]
    public float upt = 0.5f;
    public void OnHide()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + disy, transform.position.z), upt);
        float time = 0;
        DOTween.To(() => time, a => time = a, 1, upt + 0.5f).OnComplete(() =>
        {
            seq.Kill();
            gameObject.SetActive(false);
        }) ;
    }

    public void OnShow()
    {
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(new Vector3(transform.position.x, transform.position.y - disy, transform.position.z), dropt));
        seq.Append(transform.DOPunchPosition(new Vector3(0, 2.5f, 0), 1, 2, 0.1f));
    }
    void Start()
    {
        OnHide();
    }
    public void Show()
    {
        ;
    }
}

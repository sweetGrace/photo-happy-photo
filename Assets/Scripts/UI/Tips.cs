using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using System.Threading;
using TreeEditor;

public class Tips : MonoBehaviour, IUIState
{
    [Header("弹出方向（1为右上，-1为左上）")]
    public int direction = 1;
    [Header("x弹出距离")]
    public float disx = 3;
    [Header("y弹出距离")]
    public float disy = 1;
    [Header("弹出动画持续时间")]
    public float startt = 1;
    [Header("渐隐动画持续时间")]
    public float endt = 1;
    [Header("待机动画持续时间")]
    public float processt = 3;
    [Header("待机动画单次循环持续时间")]
    public float processangle = 30;
    Sequence seq;
    public Transform parent;
    private float x, y;
    public void OnHide()
    {
        transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, endt, true);
        float timeCount = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, parent.position.z - 0.1f) - new Vector3(direction * disx, disy, 0);
        DOTween.To(() => timeCount, a => timeCount = a, 1, endt).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OnShow()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, parent.position.z - 0.1f);
        if (seq != null) seq.Kill();
        gameObject.SetActive(true);
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30 * direction));
        if (direction == -1)
        {
            Transform child = transform.GetChild(0).GetChild(0);
            child.localScale = new Vector3(direction * child.localScale.x, child.localScale.y, child.localScale.z);
        }
            
        transform.DOJump(transform.position + new Vector3(direction * disx, disy, 0), disy, 1, startt);
        transform.DORotate(new Vector3(0, 0, 0), startt).SetEase(Ease.OutSine);
        float timeCount = 0;
        DOTween.To(() => timeCount, a => timeCount = a, 1, startt).OnComplete(() =>
        {
            Show();
        });
    }

    public void Show()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 0, processangle * direction), processt / 2).SetEase(Ease.InSine));
        seq.Append(transform.DORotate(new Vector3(0, 0, 0), processt / 2).SetEase(Ease.OutSine));
        seq.SetLoops(-1);
    }
}

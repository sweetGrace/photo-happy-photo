using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIState
{
    public abstract void OnShow();
    public abstract void Show();
    public abstract void OnHide();
}

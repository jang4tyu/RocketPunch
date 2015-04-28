using UnityEngine;
using System.Collections;

public class PanelBase : MonoBehaviour 
{
    public ePanel PanelName;
    void Awake()
    {
        transform.SetParent(UIManager.instance.transform);
        transform.localScale = Vector3.one;
    }

    public void Next()
    {
        SceneManager.instance.ActionEvent(eAction.NEXT);
    }
}

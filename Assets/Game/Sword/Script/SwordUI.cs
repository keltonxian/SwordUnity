using UnityEngine;
using PureMVC.Patterns.Facade;
using PureMVC.Const;

public class SwordUI : MonoBehaviour
{
    public void Enter()
    {

    }

    public void OnClickHome()
    {
        Facade.Instance.SendMessageCommand(NotiConst.SWORD_END, "SceneHome");
    }
}

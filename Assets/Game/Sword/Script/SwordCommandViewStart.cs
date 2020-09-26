using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using PureMVC.Command;
using PureMVC.Const;

public class SwordCommandViewStart : Command
{
    public override void Execute(INotification Notification)
    {
        Debug.Log("Sword View Start");
        Facade.Instance.RemoveCommand(NotiConst.SWORD_VIEW_START);
        Facade.Instance.RegisterProxy(new SwordProxy());
        Facade.Instance.RegisterMediator(new SwordViewMediator(Notification.Body as SwordView));
    }
}

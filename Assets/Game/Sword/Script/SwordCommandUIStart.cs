using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using PureMVC.Command;
using PureMVC.Const;

public class SwordCommandUIStart : Command
{
    public override void Execute(INotification Notification)
    {
        Debug.Log("Sword UI Start");
        Facade.Instance.RemoveCommand(NotiConst.SWORD_UI_START);
        Facade.Instance.RegisterMediator(new SwordUIMediator(Notification.Body as SwordUI));
    }
}

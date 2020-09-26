using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using PureMVC.Command;
using PureMVC.Const;

public class SwordCommandPlay : Command
{
    public override void Execute(INotification Notification)
    {
        Debug.Log("Sword Play");
        Facade.Instance.RemoveCommand(NotiConst.SWORD_PLAY);
        SendNotification(SwordViewMediator.NOTI_ENTER);
        SendNotification(SwordUIMediator.NOTI_ENTER);
    }
}

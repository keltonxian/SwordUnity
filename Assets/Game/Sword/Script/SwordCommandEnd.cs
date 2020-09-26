using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using PureMVC.Command;
using PureMVC.Const;
using PureMVC.Manager;

public class SwordCommandEnd : Command
{
    public override void Execute(INotification Notification)
    {
        Debug.Log("Sword End");
        Facade.Instance.RemoveCommand(NotiConst.SWORD_END);
        Facade.Instance.RemoveProxy(SwordProxy.NAME);
        Facade.Instance.RemoveMediator(SwordViewMediator.NAME);
        Facade.Instance.RemoveMediator(SwordUIMediator.NAME);
        Facade.Instance.GetManager<GameManager>(ManagerName.Game).LoadNextScene(Notification.Body as string);
    }
}

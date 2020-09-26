using PureMVC.Patterns.Mediator;
using PureMVC.Interfaces;

public class SwordUIMediator : Mediator
{
    public new static string NAME = "SwordUIMediator";

    public const string NOTI_ENTER = "UI_Enter";

    private SwordProxy _swordProxy;
    private SwordUI _swordUI;

    public SwordUIMediator(object viewComponent = null) : base(NAME, viewComponent)
    {
        _swordUI = viewComponent as SwordUI;
    }

    public override string[] ListNotificationInterests()
    {
        return new string[1] { NOTI_ENTER };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NOTI_ENTER:
                ViewEnter();
                break;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        _swordProxy = Facade.RetrieveProxy(SwordProxy.NAME) as SwordProxy;
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }

    public void ViewEnter()
    {
        _swordUI.Enter();
    }
}

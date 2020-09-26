using PureMVC.Patterns.Mediator;
using PureMVC.Interfaces;

public class SwordViewMediator : Mediator
{
    public new static string NAME = "SwordViewMediator";

    public const string NOTI_ENTER = "View_Enter";

    private SwordProxy _swordProxy;
    private SwordView _swordView;

    public SwordViewMediator(object viewComponent = null) : base(NAME, viewComponent)
    {
        _swordView = viewComponent as SwordView;
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
        _swordView.Enter();
    }
}

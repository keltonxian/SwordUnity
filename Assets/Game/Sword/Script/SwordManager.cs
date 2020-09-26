using PureMVC.Patterns.Facade;
using PureMVC.Const;

public class SwordManager : BaseScene
{
    public SwordView _swordView;
    public SwordUI _swordUI;

    public override void Init()
    {
        base.Init();
        Facade.Instance.RegisterCommand(NotiConst.ROLE_MALE_VIEW_START, () => new SwordCommandViewStart());
        Facade.Instance.RegisterCommand(NotiConst.ROLE_MALE_UI_START, () => new SwordCommandUIStart());
        Facade.Instance.RegisterCommand(NotiConst.ROLE_MALE_END, () => new SwordCommandEnd());
        Facade.Instance.RegisterCommand(NotiConst.ROLE_MALE_PLAY, () => new SwordCommandPlay());
        Facade.Instance.SendMessageCommand(NotiConst.ROLE_MALE_VIEW_START, _swordView);
        Facade.Instance.SendMessageCommand(NotiConst.ROLE_MALE_UI_START, _swordUI);
        Facade.Instance.SendMessageCommand(NotiConst.ROLE_MALE_PLAY);
    }
}

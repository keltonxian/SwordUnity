using PureMVC.Patterns.Proxy;

public class SwordProxy : Proxy
{
    public new static string NAME = "SwordProxy";

    public SwordProxy(object data = null) : base(NAME, data)
    {

    }
}

namespace ModelImplementation;

public interface IModel
{
    void HasOut(out string? s);
    bool HasOutReturn(out string? s);
}

public class Model : IModel
{
    public void HasOut(out string? s)
    {
        s = null;
    }

    public bool HasOutReturn(out string? s)
    {
        s = null;
        return false;
    }
}

// we use this for generating model IL
public class Harness: IModel
{
    private Model model = new Model();


    public void HasOut(out string? s)
    {
        model.HasOut(out s);
    }

    public bool HasOutReturn(out string? s)
    {
        return model.HasOutReturn(out s);
    }
}

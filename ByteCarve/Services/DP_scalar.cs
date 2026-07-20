namespace ByteCarve.Services;

public class DP_scalar
{
    private string op;
    ulong index;
    public DP_scalar(ulong ts,string ops)
    {
        index = ts;
        op = ops;
    }
}
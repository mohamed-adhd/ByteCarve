namespace ByteCarve.Services;
using System;
public class DP_scalar
{
    private string op;
    ulong index;
    public DP_scalar(ulong ts,string ops)
    {
        index = ts;
        op = ops;
    }
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo - hi + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }

    public void process_it(byte[] data)
    {
        uint word = BitConverter.ToUInt32(data, 0);
        uint op0 = extractBits(word, 25, 28);
        uint op1 = extractBits(word, 24, 24);
        uint op2 = extractBits(word, 21, 21);
        uint op3 = extractBits(word, 10, 15);
        switch (op0)
        {
            case 0b1111:
                switch (op1)
                {
                    case 1:
                        fpdp3(word);
                        break;
                    case 0:
                        break;
                }
                break;
            case 0b0111:
                break;
        }
    }
}
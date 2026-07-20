namespace ByteCarve.Services;
using System;
using System.IO;
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
        uint extra = extractBits(word, 21, 28);
        if (op0.Equals(0b1111) && op1.Equals(0b0) && op2.Equals(0b1) && (op3 & 0b001111) == 0b001000)
        {
            fpcomp(word);
        }else if (op0.Equals(0b1111) && op1.Equals(0b0) && op2.Equals(0b1) && (op3 & 0b011111) == 0b010000)
        {
            fpdp1(word);
        }else if (op0.Equals(0b1111) && op1.Equals(0b0) && op2.Equals(0b1) && op3.Equals(000000))
        {
            fpintcon(word);
        }else if (op0.Equals(0b1111) && op1.Equals(0b0) && op2.Equals(0b1))
        {
            fpdp1(word);
        }
        else if (op0.Equals(0b1111) &&  op2.Equals(0b1))
        {
            fpdp1(word);
        }else if (op0.Equals(0b1111) &&  op2.Equals(0b1))
        {
            advs3(word);
        }else if (op0.Equals(0b1111) &&  extra.Equals(0b01110000))
        {
            advcp(word);
        }

    }

    public void advcp(uint word)
    {
        
    }

    public void advs3(uint word)
    {
        
    }

    public void fpdp1(uint word)
    {
        
    }

    public void fpintcon(uint word)
    {
        
    }
    public void fpcomp(uint word)
    {
        uint sf = extractBits(word, 22, 23);
        string typo = "";
        string sz = sf == 0 ? "s" : sf == 1 ? "d" : sf == 3 ? "h" : "?";
        uint opc = extractBits(word, 0, 4);
        uint rm = extractBits(word, 16, 20);
        uint rn = extractBits(word, 5, 9);
        bool signaling  = (opc & 0b10000) != 0;
        bool cmpZero    = (opc & 0b01000) != 0;
        string mn = signaling ? "fcmpe" : "fcmp";
        string o2 = cmpZero ? "#0.0" : $"{sz}{rm}";
        File.AppendAllText(op + "bytecarve.s", mn + " " + sz+rn+" , "+o2 );
    }
}
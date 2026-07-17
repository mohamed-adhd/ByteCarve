namespace ByteCarve.Services;
using System.IO;
public class Branches
{
    private string op;
    ulong index;
    public Branches(ulong ts,string ops)
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

    public void process_it(uint word)
    {
        uint top8 = extractBits(word, 24,31);
        uint top7 = extractBits(word, 25,31);
        uint top6 = extractBits(word, 25,30);
        uint top5 = extractBits(word, 26,30);
        if (top8 == 0b11010100) exceptions(word);
        if (top8 == 0b11010101) systems(word);
        if (top7 == 0b1101011)  Uncond(word);
        if (top7 == 0b0101010)  Cond(word);
        if (top6 == 0b011010)   Compare(word);
        if (top6 == 0b011011)   test(word);
        if (top5 == 0b00101)    UncondBranchImmediate(word);
    }

    public void exceptions(uint word)
    {
        string typo = "";
        uint opc = extractBits(word, 21, 23);
        int im16 = (int)extractBits(word, 5, 20);
        uint ll=extractBits(word, 0, 1);
        switch (opc)
        {
            case 0b000:
                switch (ll)
            {
                case 0b01:
                    typo = "svc";
                    break;
                case 0b10:
                    typo = "hvc";
                    break;
                case 0b11:
                    typo = "smc";
                    break;
            }
                break;
            case 0b001:
                typo = "brk";
                break;
            case 0b010:
                typo = "hlt";
                break;
            case 0b101:
                switch (ll)
                {
                    case 0b01:
                        typo = "dcps1";
                        break;
                    case 0b10:
                        typo = "dcps2";
                        break;
                    case 0b11:
                        typo = "dcps3";
                        break;
                }
                break;
        }
        File.AppendAllText(op + "bytecarve.s", typo+", #"+im16);

    }

    public void test(uint word)
    {
        string typo;
        int sp = (int)extractBits(word, 24, 24);
        int rt = (int)extractBits(word, 0, 4);
        typo = sp == 0 ? "tbz" : "tbnz";
        uint st_seg = extractBits(word, 19, 23);
        uint nd_seg = extractBits(word, 31, 31);
        uint res=(st_seg << 5) | nd_seg;
        W




    }
    
}
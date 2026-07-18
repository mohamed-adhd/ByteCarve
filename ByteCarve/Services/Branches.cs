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
    

    public void process_it(uint word,ulong index)
    {
        uint top8 = extractBits(word, 24,31);
        uint top7 = extractBits(word, 25,31);
        uint top6 = extractBits(word, 25,30);
        uint top5 = extractBits(word, 26,30);
        if (top8 == 0b11010100) exceptions(word);
        if (top8 == 0b11010101) systems(word);
        if (top7 == 0b1101011)  uncond(word);
        if (top7 == 0b0101010)  cond(word);
        if (top6 == 0b011010)   compare(word);
        if (top6 == 0b011011)   test(word,index );
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

    public void test(uint word,ulong index)
    {
        string typo;
        int sp = (int)extractBits(word, 24, 24);
        int rt = (int)extractBits(word, 0, 4);
        typo = sp == 0 ? "tbz" : "tbnz";
        uint st_seg = extractBits(word, 19, 23);
        uint imm14 = extractBits(word, 5, 18);
        long signedImm14;
        if ((imm14 & 0x2000) != 0)
            signedImm14 = imm14 - 0x4000;
        else
            signedImm14 = imm14;
        long offset = signedImm14 << 2;
        long target = (long)index + offset;
        uint nd_seg = extractBits(word, 31, 31);
        uint res=(nd_seg << 5) | st_seg;
        string regPrefix = nd_seg == 0 ? "w" : "x";
        File.AppendAllText(op + "bytecarve.s",typo+" "+regPrefix+rt+", #"+target+", "+imm14.ToString());
    }

    public void compare(uint word)
    {
        string regsize = (int)extractBits(word, 31,31)==0? "w" : "x";
        string op =(int)extractBits(word, 24,24)==0? "cbz" : "cbnz";
        uint imm19=extractBits(word, 5,23);
        int rt=(int)extractBits(word, 0, 4);
        File.AppendAllText(op + "bytecarve.s", op+" "+regsize+rt+", "+(index+imm19));
    }

    public void systems(uint word)
    {
        int l   = (int)extractBits(word, 21, 21);
        int op0 = (int)extractBits(word, 19, 20);
        int op1 = (int)extractBits(word, 16, 18);
        int crn = (int)extractBits(word, 12, 15);
        int crm = (int)extractBits(word, 8, 11);
        int op2 = (int)extractBits(word, 5, 7);
        int rt  = (int)extractBits(word, 0, 4);
        string register = (op0, op1, crn, crm, op2) switch
        {
            (3, 3, 4, 2, 0) => "NZCV",
            (3, 3, 4, 2, 1) => "DAIF",
            (3, 0, 4, 2, 2) => "CurrentEL",
            (3, 3, 4, 4, 0) => "FPCR",
            (3, 3, 4, 4, 1) => "FPSR",
            _ => $"S{op0}_{op1}_C{crn}_C{crm}_{op2}"
        };
        string mnemonic = l == 1 ? "mrs" : "msr";
        string output = l == 1
            ? $"{mnemonic} x{rt}, {register}\n"
            : $"{mnemonic} {register}, x{rt}\n";
        File.AppendAllText(op + "bytecarve.s", output);
    }

    public void cond(uint word)
    {
        uint imm19 = extractBits(word,5, 23);
        uint cond = extractBits(word,0, 3);
        string s = cond switch
        {
            0x0=> "eq",
            0x1=> "ne",
            0x2 =>"cs",
            0x3 => "cc",
            0x4 =>"mi",
            0x5 =>"pl",
            0x6=> "vs",
            0x7=>"vc",
            0x8 => "hi",
            0x9 => "ls",
            0xA=> "ge",
            0xB=> "lt",
            0xC =>"gt",
            0xD =>"le",
            0xE =>"al",
            _   =>"nv"
        };
        File.AppendAllText(op + "bytecarve.s",s+" "+(index+imm19));

        
    }

    public void uncond(uint word)
    {
        uint imm19 = extractBits(word,0, 25);
        string opss=(int)extractBits(word, 31,31)==0? "b" : "bl";
        File.AppendAllText(op + "bytecarve.s",opss+" "+(index+imm19));

    }
    public string system_reg(uint word)
    {
        int Op0;
        int Op1;
        int CRn;
        int CRm;
        int Op2;
        string Name; 
    return "nah thanks";
}
}
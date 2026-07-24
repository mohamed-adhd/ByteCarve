namespace ByteCarve.Services;
using System;
using System.IO;
public class Loads
{
    private string op;
    ulong index;
    public Loads(ulong ts,string ops)
    {
        index = ts;
        op = ops;
    }
    static uint extractBits(uint word, int lo, int hi)
    {
        int width = hi - lo + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }

    public void process_it(byte[] data)
    {
        uint word = BitConverter.ToUInt32(data, 0);
        uint op0 = extractBits(word, 27, 29);

        if (op0 == 0b011)
            ldlt(word);

        else if (op0 == 0b101)
        {
            uint mode = extractBits(word, 23, 24);
            switch (mode)
            {
                case 0b01: ppi(word); break;
                case 0b10: pso(word); break;
                case 0b11: ppri(word); break;
            }
        }

        else if (op0 == 0b111)
        {
            int bit24 = (int)extractBits(word, 24, 24);

            if (bit24 == 1)
                ui(word);
            else
            {
                uint mode = extractBits(word, 10, 11);

                switch (mode)
                {
                    case 0b00:
                        usci(word);
                        break;

                    case 0b01:
                        opstid(word);
                        break;

                    case 0b11:
                        if (extractBits(word, 21, 21) == 0)
                            preid(word);
                        else
                            reg(word);
                        break;
                }
            }
        }
        

    }

    public void ldlt(uint word)
    {
        uint opc = extractBits(word, 30, 31);
        string mn = "";
        string rt = ((int)extractBits(word, 0, 4)).ToString();
        uint im9 = extractBits(word, 5, 23);
        switch (opc)
        {
            case 0b00:
                mn = "ldr w"+rt;
                break;
            case 0b01:
                mn = "ldr x"+rt;
                break;
            case 0b11:
                mn = "ldrsw x"+rt;
                break;
            case 0b10:
                mn = "prfm "+rt;
                break;
        }

        long shift = 64 - 19;
        uint s = (im9 << (int)shift) >> (int)shift;
        File.AppendAllText(op + "bytecarve.s", mn + ", "+s+index);

    }
    private static (string pre, int scale) Dpr(int opc, int v)
    {
        switch ((opc << 1) | v)
        {
            case 0: 
                return ("w", 2); 
            case 1: return ("s", 2);
            case 2: return ("x", 3);
            case 3: return ("d", 3);
            case 5: return ("q", 4);
        }
        throw new Exception("shi not found ");
    }
    
    public void ppi(uint word)
    {
        int opc = (int)extractBits(word, 30, 31);
        int l = (int)extractBits(word, 28, 28);
        int v = (int)extractBits(word, 29, 29);
        string mn = "";
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        int rt2 =(int)extractBits(word, 10, 14);
        uint im7 =extractBits(word, 15, 21);
        switch (l)
        {
            case 0:
                mn = "stp";
                break;
            case 1:
                mn = "ldp";
                break;
            
        }
        var (reg, scale) = Dpr(opc, v);
        string rt  = $"{reg}{rd}";
        string rt2s = $"{reg}{rt2}";
        string rns=$"{reg}{rn}";
        long shift = 64 - 19;
        uint s = (im7 << (int)shift) >> (int)shift;
        File.AppendAllText(
            op + "bytecarve.s",
            $"{mn} {rt}, {rt2s},[{rns}], #{s}\n"
        );
    }public void pso(uint word)
    {
        int opc = (int)extractBits(word, 30, 31);
        int l = (int)extractBits(word, 28, 28);
        int v = (int)extractBits(word, 29, 29);
        string mn = "";
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        int rt2 =(int)extractBits(word, 10, 14);
        uint im7 =extractBits(word, 15, 21);
        switch (l)
        {
            case 0:
                mn = "stp";
                break;
            case 1:
                mn = "ldp";
                break;
            
        }
        var (reg, scale) = Dpr(opc, v);
        string rt  = $"{reg}{rd}";
        string rt2s = $"{reg}{rt2}";
        string rns=$"{reg}{rn}";
        long shift = 64 - 19;
        uint s = (im7 << (int)shift) >> (int)shift;
        File.AppendAllText(
            op + "bytecarve.s",
            $"{mn} {rt}, {rt2s},[{rns} #{s}]\n"
        );
        
    }public void ppri(uint word)
    {
        int opc = (int)extractBits(word, 30, 31);
        int l = (int)extractBits(word, 28, 28);
        int v = (int)extractBits(word, 29, 29);
        string mn = "";
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        int rt2 =(int)extractBits(word, 10, 14);
        uint im7 =extractBits(word, 15, 21);
        switch (l)
        {
            case 0:
                mn = "stp";
                break;
            case 1:
                mn = "ldp";
                break;
            
        }
        var (reg, scale) = Dpr(opc, v);
        string rt  = $"{reg}{rd}";
        string rt2s = $"{reg}{rt2}";
        string rns=$"{reg}{rn}";
        long shift = 64 - 19;
        uint s = (im7 << (int)shift) >> (int)shift;
        File.AppendAllText(
            op + "bytecarve.s",
            $"{mn} {rt}, {rt2s},[{rns} #{s}]!\n"
        );
    }public void ui(uint word)
    {
        uint sz = extractBits(word, 30, 31);
        int v = (int)extractBits(word, 29, 29);
        uint opc = extractBits(word, 22, 23);
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        uint im12 = extractBits(word, 10, 21);
        string rtPrefix="", mn = "";
        switch ((sz << 2) | opc)
        {
            case 0b0000:
                mn = "strb ";
                break;
            case 0b0001:
                mn = "ldrb ";
                break;
            case 0b0010:
                mn = "ldrsb w ";
                break;
            case 0b0011:
                mn = "ldrsb x ";
                break;
            case 0b0100:
                mn = "strh ";
                break;
            case 0b0101:
                mn = "ldrh ";
                break;
            case 0b0111:
                mn = "ldrsh x";
                break;
            case 0b0110:
                mn = "ldrsh w";
                break;
            case 0b1000:
                mn = "strw ";
                break;
            case 0b1001:
                mn = "ldrw ";
                break;
            case 0b1010:
                mn = "ldrsw x";
                break;
            case 0b1011:
                mn = "shi_reseeerved";
                break;
            //here lies my own soul , after countless nights of sub-buckets designing and more than 50 redbull can drank, i got enough here , but aint gon lie  , mama aint raised a complainer not a  quitter , so continue type shi
            case 0b1100:
                mn = "str x";
                break;
            case 0b1101:
                mn = "ldr x";
                break;
            case 0b1111:
                mn = "shi_reseeerved";
                break;
            case 0b1110:
                mn = "shi_reseeerved";
                break;
        }
        ulong offset = (ulong)im12 << (int)sz;
        string bReg = rn == 31 ? "sp" : $"x{rn}";
        string tReg = $"{rtPrefix}{rd}";
        string ins =
            $"{mn} {tReg}, [{bReg}, #{offset}]";
        File.AppendAllText(
            op + "bytecarve.s",ins);
    }public void usci(uint word)
    {
        uint sz = extractBits(word, 30, 31);
        int v = (int)extractBits(word, 29, 29);
        uint opc = extractBits(word, 22, 23);
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        uint im12 = extractBits(word, 12, 20);
        string mn = "";
        string reg = "";
        switch ((sz << 2) | opc)
        {
            case 0b0000:
                mn = "strb";
                reg = "w";
                break;

            case 0b0001:
                mn = "ldrb";
                reg = "w";
                break;

            case 0b0010:
                mn = "ldrsb";
                reg = "w";
                break;

            case 0b0011:
                mn = "ldrsb";
                reg = "x";
                break;

            case 0b0100:
                mn = "strh";
                reg = "w";
                break;

            case 0b0101:
                mn = "ldrh";
                reg = "w";
                break;

            case 0b0110:
                mn = "ldrsh";
                reg = "w";
                break;

            case 0b0111:
                mn = "ldrsh";
                reg = "x";
                break;

            case 0b1000:
                mn = "str";
                reg = "w";
                break;

            case 0b1001:
                mn = "ldr";
                reg = "w";
                break;

            case 0b1010:
                mn = "ldrsw";
                reg = "x";
                break;

            case 0b1100:
                mn = "str";
                reg = "x";
                break;

            case 0b1101:
                mn = "ldr";
                reg = "x";
                break;
            default:
                throw new Exception("Reserved twin");
        }
        ulong offset = (ulong)im12 << (int)sz;
        File.AppendAllText(
            op + "bytecarve.s",$"{mn} {reg+rd}, [{reg+rn}, #{offset}]");
        
    }
    public void opstid(uint word)
    {
        uint sz = extractBits(word, 30, 31);
        int v = (int)extractBits(word, 29, 29);
        uint opc = extractBits(word, 22, 23);
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        uint im12 = extractBits(word, 12, 20);
        string mn = "";
        string reg = "";
        ulong offset = (ulong)im12 << (int)sz;
        switch ((sz << 2) | opc)
        {
            case 0b0000:
                mn = "strb";
                reg = "w";
                break;

            case 0b0001:
                mn = "ldrb";
                reg = "w";
                break;

            case 0b0010:
                mn = "ldrsb";
                reg = "w";
                break;

            case 0b0011:
                mn = "ldrsb";
                reg = "x";
                break;

            case 0b0100:
                mn = "strh";
                reg = "w";
                break;

            case 0b0101:
                mn = "ldrh";
                reg = "w";
                break;

            case 0b0110:
                mn = "ldrsh";
                reg = "w";
                break;

            case 0b0111:
                mn = "ldrsh";
                reg = "x";
                break;

            case 0b1000:
                mn = "str";
                reg = "w";
                break;

            case 0b1001:
                mn = "ldr";
                reg = "w";
                break;

            case 0b1010:
                mn = "ldrsw";
                reg = "x";
                break;

            case 0b1100:
                mn = "str";
                reg = "x";
                break;

            case 0b1101:
                mn = "ldr";
                reg = "x";
                break;

            default:
                throw new Exception("Reserved af" );
            
        }
        File.AppendAllText(
            op + "bytecarve.s",$"{mn} {reg+rd}, [{reg+rn}], #{offset}");
      
    }
    public void preid(uint word)
    {
        uint sz = extractBits(word, 30, 31);
        int v = (int)extractBits(word, 29, 29);
        uint opc = extractBits(word, 22, 23);
        int rd =(int)extractBits(word, 0, 4);
        int rn =(int)extractBits(word, 5, 9);
        uint im12 = extractBits(word, 10, 21);
        string rtPrefix="", mn = "";
        switch ((sz << 2) | opc){
            case 0b0000:
            mn = "strb ";
            break;
            case 0b0001:
            mn = "ldrb ";
            break;
            case 0b0010:
            mn = "ldrsb w ";
            break;
            case 0b0011:
            mn = "ldrsb x ";
            break;
            case 0b0100:
            mn = "strh ";
            break;
            case 0b0101:
            mn = "ldrh ";
            break;
            case 0b0111:
            mn = "ldrsh x";
            break;
            case 0b0110:
            mn = "ldrsh w";
            break;
            case 0b1000:
            mn = "strw ";
            break;
            case 0b1001:
            mn = "ldrw ";
            break;
            case 0b1010:
            mn = "ldrsw x";
            break;
            case 0b1011:
            mn = "shi_reseeerved";
            break;
            case 0b1100:
            mn = "str x";
            break;
            case 0b1101:
            mn = "ldr x";
            break;
            case 0b1111:
            mn = "shi_reseeerved";
            break;
            case 0b1110:
            mn = "shi_reseeerved";
            break;
        }
        ulong offset = (ulong)im12 << (int)sz;
        string bReg = rn == 31 ? "sp" : $"x{rn}";
        string tReg = $"{rtPrefix}{rd}";
        string ins =
            $"{mn} {tReg}, [{bReg}, #{offset}]!";
        File.AppendAllText(
            op + "bytecarve.s",ins);
    }
    public void reg(uint word)
        {
            uint sz = extractBits(word, 30, 31);
            int v = (int)extractBits(word, 29, 29);
            uint opc = extractBits(word, 22, 23);
            int rd =(int)extractBits(word, 0, 4);
            int rn =(int)extractBits(word, 5, 9);
            int rm = (int)extractBits(word, 16, 20);
            int s = (int)extractBits(word, 12, 12);
            string rtPrefix="", mn = "",reg="";
            switch ((sz << 2) | opc)
            {
                case 0b0000: mn = "strb";  reg = "w"; break;
                case 0b0001: mn = "ldrb";  reg = "w"; break;
                case 0b0010: mn = "ldrsb"; reg = "w"; break;
                case 0b0011: mn = "ldrsb"; reg = "x"; break;
                case 0b0100: mn = "strh";  reg = "w"; break;
                case 0b0101: mn = "ldrh";  reg = "w"; break;
                case 0b0110: mn = "ldrsh"; reg = "w"; break;
                case 0b0111: mn = "ldrsh"; reg = "x"; break;
                case 0b1000: mn = "str";   reg = "w"; break;
                case 0b1001: mn = "ldr";   reg = "w"; break;
                case 0b1010: mn = "ldrsw"; reg = "x"; break;
                case 0b1100: mn = "str";   reg = "x"; break;
                case 0b1101: mn = "ldr";   reg = "x"; break;
                default: throw new Exception("shi_is_Reserved");//finally done god damn it 
            }
            uint option = extractBits(word, 13, 15);
            string opt = "";
            switch (option)
            {
                case 0b010:
                    opt = "uxtw";
                    break;
                case 0b011:
                    opt = "lsl";
                    break;
                case 0b110:
                    opt = "sxtw";
                    break;
                case 0b111:
                    opt = "sxtx";
                    break;
            }
            if (s == 0)
            {
                File.AppendAllText(
                    op + "bytecarve.s",$"{mn} {reg+rd}, [{reg+rn}, #{reg+rm}]");
            }
            else
            {
                File.AppendAllText(
                    op + "bytecarve.s",$"{mn} {reg+rd}, [{reg+rn}, #{reg+rm}, {opt} #{rm+rn}]");
            }
        }
    
}
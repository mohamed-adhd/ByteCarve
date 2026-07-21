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
        int q = (int)extractBits(word, 30, 30);
        int u = (int)extractBits(word, 29, 29);
        uint sz =extractBits(word, 22, 23);
        uint opc = extractBits(word, 11, 15);
        string rd =((int)extractBits(word, 0, 4)).ToString();
        string rn = ((int)extractBits(word, 5, 9)).ToString();
        switch (opc)
        {
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
            case 0b00000:
                if (u==0)
                {
                    
                }
                else
                {
                    
                }
                break;
        }
        

    }

    public void fpdp1(uint word)
    {
        uint opc = extractBits(word, 15, 20);
        string typo = (int)extractBits(word, 22, 23)== 0 ? "s" : (int)extractBits(word, 22, 23) == 1 ? "d" : (int)extractBits(word, 22, 23) == 3 ? "h" : "?";
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        uint type   = extractBits(word, 22, 23);
        uint opcode = extractBits(word, 15, 20);
        string mn="";
        switch (opcode)
        {
            case 0b000000: mn = "fmov";   break;
            case 0b000001: mn = "fabs";   break;
            case 0b000010: mn = "fneg";   break;
            case 0b000011: mn = "fsqrt";  break;
            case 0b000100: mn = "fcvt";   break; 
            case 0b000101: mn = "fcvt";   break;
            case 0b000111: mn = "fcvt";   break; 
            case 0b001000: mn = "frintn"; break;
            case 0b001001: mn = "frintp"; break;
            case 0b001010: mn = "frintm"; break;
            case 0b001011: mn = "frintz"; break;
            case 0b001100: mn = "frinta"; break;
            case 0b001110: mn = "frintx"; break;
            case 0b001111: mn = "frinti"; break;
        }
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd +" , "+rn );

    }

    public void fpintcon(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        uint opc = extractBits(word, 16, 18);
        uint rmd = extractBits(word, 19, 20);
        uint type = extractBits(word, 22, 23);
        string mn = "";

        switch (rmd)
        {
            case 0b00:
                switch (opc)
                {
                    case 0b000: mn = "fcvtns"; break;
                    case 0b001: mn = "fcvtnu"; break;
                    case 0b010: mn = "scvtf"; break;
                    case 0b011: mn = "ucvtf"; break;
                    case 0b100: mn = "fcvtas"; break;
                    case 0b101: mn = "fcvtau"; break;
                    case 0b110: mn = "fmov"; break;
                    case 0b111: mn = "fmov"; break;
                    default: mn = "undefined"; break;
                }

                break;

            case 0b01:
                switch (opc)
                {
                    case 0b000: mn = "fcvtps"; break;
                    case 0b001: mn = "fcvtpu"; break;
                    case 0b110: mn = "fmov"; break;
                    case 0b111:
                        mn = "fmov";
                        break; // i had my brother type this switch for me lmao , git his ass working like a maid
                    default: mn = "undefined"; break;
                }

                break;

            case 0b10:
                switch (opc)
                {
                    case 0b000: mn = "fcvtms"; break;
                    case 0b001: mn = "fcvtmu"; break;
                    default: mn = "undefined"; break;
                }

                break;

            case 0b11:
                switch (opc)
                {
                    case 0b000: mn = "fcvtzs"; break;
                    case 0b001: mn = "fcvtzu"; break;
                    case 0b110: mn = "fjcvtzs"; break;
                    default: mn = "undefined"; break;
                }

                break;

        }
        bool intToFloat = (opc == 0b010 || opc== 0b011); 
        string Rns, Rds;
        if (intToFloat)
        {
            Rns = (extractBits(word,31,31) == 1 ? "x" : "w") + rn; 
            Rds = TypeToFpPrefix(type) + rd; 
        }
        else
        {
            Rns = TypeToFpPrefix(type) + rn;  
            Rds = (extractBits(word,31,31) == 1 ? "x" : "w") + rd;   
        }
        File.AppendAllText(op + "bytecarve.s", mn + " " + Rds +" , "+Rns );

    }
    string TypeToFpPrefix(uint type) => type switch
    {
        0b00 => "s",
        0b01 => "d",
        0b11 => "h",
        _ => "?"
    };
    

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
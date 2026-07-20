using System.Security.Permissions;

namespace ByteCarve.Services;
using System;
using System.IO;

public class DP_register
{
    private uint word;
    private ulong index;
    private string op;

    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo - hi + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }

    public void process_it(byte[] data)
    {
        uint word = BitConverter.ToUInt32(data, 0);
        uint op0 = extractBits(word, 30, 30);
        uint op1 = extractBits(word, 28, 28);
        uint op2 = extractBits(word, 21, 24);
        uint op3 = extractBits(word, 10, 15);
        switch ((int)op0)
        {
            case 0:
                if ((int)op1 == 0)
                {
                    logical(word);
                }

                break;
            case 1:
                switch ((int)op1)
                {
                    case 0:
                        if ((op3 & 0b111000) == 0b000000)
                        {
                            addsubex(word);
                        }
                        else
                        {
                            addsubshif(word);
                        }

                        break;
                    case 1:
                        if (op2.Equals(0b0100))
                        {
                            condselec(word);
                        }
                        else if (op2.Equals(0b0000) && op3.Equals(0b000000))
                        {
                            addsubcarr(word);
                        }
                        else if (op3.Equals(0b000010) && op2.Equals(0101))
                        {
                            rmif(word);
                        }
                        else if (op2.Equals(0b1000))
                        {
                            if (op0 == 0)
                                DP_sc2(word);   
                            else
                                DP_sc1(word); 
                        }
                        else if ((op2 & 0b1000) == 0b1000)
                        {
                            DP_sc3(word);
                        }
                        else if (op2.Equals(0b0010))
                        {
                            if ((op3 & 0b100000) == 0b000000)
                            {
                                condcompreg(word);
                            }
                            else if ((op3 & 0b111000) == 0b100000)
                            {
                                condcompimed(word);
                            }
                        }

                        break;
                    
                }
        }

    }

    public void logical(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        uint opc = extractBits(word, 29, 30);
        int n = (int)extractBits(word, 21, 21);
        uint shift = extractBits(word, 22, 23);
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        string rm = typo + (int)extractBits(word, 16, 20);
        uint im6 = extractBits(word, 10, 15);
        string mn = "";
        switch (n)
        {
            case 0:
                switch (opc)
                {
                    case 0b00:
                        mn = "and";
                        break;
                    case 0b10:
                        mn = "eor";
                        break;
                    case 0b11:
                        mn = "ands";
                        break;
                    case 0b01:
                        mn = "orr";
                        break;
                }

                break;
            case 1:
                switch (opc)
                {
                    case 0b00:
                        mn = "ric";
                        break;
                    case 0b10:
                        mn = "orn";
                        break;
                    case 0b11:
                        mn = "bics";
                        break;
                    case 0b01:
                        mn = "orn";
                        break;
                }

                break;
        }

        string sh = "";
        switch (shift)
        {
            case 0b00:
                sh = "lsl";
                break;
            case 0b10:
                sh = "asr";
                break;
            case 0b11:
                sh = "ror";
                break;
            case 0b01:
                sh = "lsr";
                break;
        }

        File.AppendAllText(op + "bytecarve.s", mn + " " + rd + ", " + rn + " ," + rm + " ," + sh + " #" + (int)im6);


    }

    public void condselec(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        int op = (int)extractBits(word, 30, 30);
        int o2 = (int)extractBits(word, 10, 10);
        uint shift = extractBits(word, 22, 23);
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        string rm = typo + (int)extractBits(word, 16, 20);
        uint im6 = extractBits(word, 10, 15);
        uint cond = extractBits(word, 12, 15);
        string mn = "";
        switch (op)
        {
            case 1:
                switch (o2)
                {
                    case 0:
                        mn = "csinv";
                        break;
                    case 1:
                        mn = "cseng";
                        break;
                }

                break;
            case 0:
                switch (o2)
                {
                    case 0:
                        mn = "csel";
                        break;
                    case 1:
                        mn = "csinc";
                        break;
                }

                break;
        }

        string s = cond switch
        {
            0x0 => "eq",
            0x1 => "ne",
            0x2 => "cs",
            0x3 => "cc",
            0x4 => "mi",
            0x5 => "pl",
            0x6 => "vs",
            0x7 => "vc",
            0x8 => "hi",
            0x9 => "ls",
            0xA => "ge",
            0xB => "lt",
            0xC => "gt",
            0xD => "le",
            0xE => "al",
            _ => "nv"
        };
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd + ", " + rn + " ," + rm + " ," + (int)im6);
    }

    public void DP_sc2(uint word)
    {
        
    }

    public void addsubshif(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        uint op = extractBits(word, 30, 30);
        int s = (int)extractBits(word, 29, 29);
        uint shift = extractBits(word, 22, 23);
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        string rm = typo + (int)extractBits(word, 16, 20);
        uint im6 = extractBits(word, 10, 15);
        string sh = "";
        switch (shift)
        {
            case 0b00:
                sh = "lsl";
                break;
            case 0b10:
                sh = "asr";
                break;
            case 0b01:
                sh = "lsr";
                break;
        }

        string mn = "";
        switch (op)
        {
            case 1:
                switch (s)
                {
                    case 0:
                        mn = "sub";
                        break;
                    case 1:
                        mn = "subs";
                        break;
                }

                break;
            case 0:
                switch (s)
                {
                    case 0:
                        mn = "add";
                        break;
                    case 1:
                        mn = "adds";
                        break;
                }

                break;
        }

        File.AppendAllText(op + "bytecarve.s", mn + " " + rd + ", " + rn + " ," + rm + " ," + sh + " #" + (int)im6);

    }

    public void addsubex(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        uint op = extractBits(word, 30, 30);
        int s = (int)extractBits(word, 29, 29);
        uint im3 = extractBits(word, 10, 12);
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        string rm = typo + (int)extractBits(word, 16, 20);
        string mn = "";
        uint wtvrman = extractBits(word, 13, 15);
        switch (op)
        {
            case 1:
                switch (s)
                {
                    case 0:
                        mn = "sub";
                        break;
                    case 1:
                        mn = "subs";
                        break;
                }

                break;
            case 0:
                switch (s)
                {
                    case 0:
                        mn = "add";
                        break;
                    case 1:
                        mn = "adds";
                        break;
                }

                break;
        }

        string ex = "";
        switch (wtvrman)
        {
            case 0b000:
                ex = "uxtb";
                break;
            case 0b001:
                ex = "uxth";
                break;
            case 0b010:
                ex = "uxtw";
                break;
            case 0b011:
                ex = "uxtx";
                break;
            case 0b100:
                ex = "sxtb";
                break;
            case 0b101:
                ex = "sxth";
                break;
            case 0b110:
                ex = "sxtw";
                break;
            case 0b111:
                ex = "sxtx";
                break;
        }
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd + ", " + rn + " ," + rm + " ," + ex + " #" + (int)im3);
        
    }
public void condcompimed(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        string mn = (int)extractBits(word, 30, 30) == 0 ? "ccmn" : "ccmp";
        string rd = typo + (int)extractBits(word, 0, 4);
        uint cond = extractBits(word, 12, 15);
        int nzcv = (int)extractBits(word, 5, 7);
        uint imm16 = extractBits(word, 16, 20);
        string s = cond switch
        {
            0x0 => "eq",
            0x1 => "ne",
            0x2 => "cs",
            0x3 => "cc",
            0x4 => "mi",
            0x5 => "pl",
            0x6 => "vs",
            0x7 => "vc",
            0x8 => "hi",
            0x9 => "ls",
            0xA => "ge",
            0xB => "lt",
            0xC => "gt",
            0xD => "le",
            0xE => "al",
            _ => "nv"
        };
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd +" , #"+(int)imm16 + ", #" +nzcv+" "+s);
    }
public void condcompreg(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        string mn = (int)extractBits(word, 30, 30) == 0 ? "ccmn" : "ccmp";
        string rd = typo + (int)extractBits(word, 0, 4);
        uint cond = extractBits(word, 12, 15);
        int nzcv = (int)extractBits(word, 5, 7);
        string rm =typo+(int) extractBits(word, 16, 20);
        string s = cond switch
        {
            0x0 => "eq",
            0x1 => "ne",
            0x2 => "cs",
            0x3 => "cc",
            0x4 => "mi",
            0x5 => "pl",
            0x6 => "vs",
            0x7 => "vc",
            0x8 => "hi",
            0x9 => "ls",
            0xA => "ge",
            0xB => "lt",
            0xC => "gt",
            0xD => "le",
            0xE => "al",
            _ => "nv"
        };
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd +" , #"+rm + ", #" +nzcv+" "+s);
        
    }public void DP_sc3(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        uint opc = extractBits(word,10, 15);
        string mn = "";
        switch (opc)
        {
            case 0b00010:
                mn = "udiv";
                break;
            case 0b00011:
                mn = "sdiv";
                break;
            case 0b00100:
                mn = "crc32b";
                break;
            case 0b00101:
                mn = "crc32h";
                break;
            case 0b00110:
                mn = "crc32w";
                break;
            case 0b00111:
                mn = "crc32x";
                break;
            case 0b01000:
                mn = "lsl";
                break;
            case 0b01001:
                mn = "lsr";
                break;
            case 0b01010:
                mn = "asr";
                break;
            case 0b01011:
                mn = "ror";
                break;
            case 0b10100:
                mn = "crc32cb";
                break;
            case 0b10101:
                mn = "crc32ch";
                break;
            case 0b10110:
                mn = "crc32cw";
                break;
            case 0b10111:
                mn = "crc32cx";
                break;
        }
        


    }public void DP_sc1(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        uint op1 =extractBits(word,16,20) ;
        uint op2 = extractBits(word, 10, 15);
        string rd = typo + (int)extractBits(word, 0, 4);
        string rn = typo + (int)extractBits(word, 5, 9);
        string mn = "";
        switch (op1)
        {
            case 0b00000:
                switch (op2)
                {
                    case 0b000000:
                        mn = "rbit";
                        break;
                    case 0b000001:
                        mn = "rev16";
                        break;
                    case 0b000010:
                        if ((int)extractBits(word, 31, 31) == 0)
                        {
                            mn = "rev";
                        }
                        else
                        {
                            mn = "rev32";
                        }
                        break;
                    //the sheer amount of redbulls and monster i consumed in this damn part of the fucking data processing bucket : 13 bottles so far 
                    case 0b000011:
                        if ((int)extractBits(word, 31, 31) == 0)
                        {
                            mn = "rev64";
                        }
                        else
                        {
                            mn = "undefined twin";
                        }

                        break;
                    case 0b000100:
                        mn = "clz";
                        break;
                    case 0b000101:
                        mn = "cls";//mercedes cls lol
                        break;
                }
                break;
            
        }
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd +" , "+rn );

        

    }public void rmif(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        uint im16=extractBits(word, 20, 15);
        uint mask = extractBits(word, 0, 3);
        string rm = typo + (int)extractBits(word, 5, 9);
        File.AppendAllText(op + "bytecarve.s", "rmif  " + rm +" , #"+im16+" ,#"+mask);

    }public void  addsubcarr(uint word)
    {
        string typo = (int)extractBits(word, 31, 31) == 1 ? "x" : "w";
        string rd = typo + (int)extractBits(word, 0, 4);
        string rm = typo + (int)extractBits(word, 16, 20);
        string rn = typo + (int)extractBits(word, 5, 9);
        string mn = "";
        switch ((int)extractBits(word, 30, 30))
        {
            case 1:
                switch ((int)extractBits(word, 29, 29))
                {
                    case 0:
                        mn = "sbc";
                        break;
                    case 1:
                        mn = "sbcs";
                        break;
                }

                break;
            case 0:
                switch ((int)extractBits(word, 29, 29))
                {
                    case 0:
                        mn = "adc";
                        break;
                    case 1:
                        mn = "adcs";
                        break;
                }

                break;
        }
        mn=(int)extractBits(word, 29, 29) == 0 ? mn+="s" : mn;
        File.AppendAllText(op + "bytecarve.s", mn + " " + rd +" , "+rm );

        

    }
}
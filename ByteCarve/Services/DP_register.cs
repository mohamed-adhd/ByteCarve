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
                        }else if (op2.Equals(0b0000) && op3.Equals(0b000000))
                        {
                            addsubcarr(word);
                        }else if (op3.Equals(0b000010) && op2.Equals(0101))
                        {
                            rmif(word);
                        }
                        else if (op2.Equals(0b1000))
                        {
                            DP_sc1(word);
                        }else if ((op2 & 0b1000) == 0b1000)
                        {
                            DP_sc3(word);
                        }else if (op2.Equals(0b0010))
                        {
                            if ((op3 & 0b100000) == 0b000000)
                            {
                                condcompreg(word);
                            }else if ((op3 & 0b111000) == 0b100000)
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

        string sh="";
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
        File.AppendAllText(op + "bytecarve.s", mn+" "+rd+", "+rn+" ,"+rm+" ,"+sh+" #"+(int)im6);


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
        uint cond=extractBits(word, 12, 15);
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
        File.AppendAllText(op + "bytecarve.s", mn+" "+rd+", "+rn+" ,"+rm+" ,"+(int)im6);
    }
    public void addsubshif(uint word)
    {
        return;
    }public void addsubex(uint word)
    {
        return;
    }public void condcompimed(uint word)
    {
        return;
    }public void condcompreg(uint word)
    {
        return;
    }public void DP_sc3(uint word)
    {
        return;
    }public void DP_sc1(uint word)
    {
        return;
    }public void rmif(uint word)
    {
        return;
    }public void  addsubcarr(uint word)
    {
        return;
    }
}
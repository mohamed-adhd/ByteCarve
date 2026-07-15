using System;
using Avalonia.Media.TextFormatting;
using System.IO;
namespace ByteCarve.Services;
using System.Collections;
using System.Linq;
public class DP_immediate
{
    private uint word;
    private ulong index;
    private string op;
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo-hi + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }
    public void process_it(byte[] tempo,ulong ts,string op)
    {
        this.op = op;
        index = ts;
        uint word=BitConverter.ToUInt32(tempo, 0);
        uint sig=extractBits(word,25, 23);
        if (sig == 0b001 || sig == 0b000)
        {
            pcaddress(word);
        }else if (sig == 0b010 || sig == 0b011)
        {
            addsubs(data);
        }else if (sig == 0b100)
        {
            logic(data);
        }else if (sig == 0b101)
        {
            movwid(data);
        }else if (sig == 0b110)
        {
            bitfild(data);
        }else if (sig == 0b111)
        {
            extrct(data);
        }
    }

    public void pcaddress(uint word)
    {
        if (((word >> 31) & 1 )== 0)
        {
            uint immlo=extractBits(word, 29, 30);
            int rgst = (int)extractBits(word, 0, 4);
            uint immhi=extractBits(word, 5, 23);
            int dist = (int)((immhi << 2) | immlo);
            if ((dist & (1 << 20))!= 0) 
            {
                dist |= unchecked((int)0xFFE00000);
            }

            File.AppendAllText(op + "bytecarve.s", index + ": adr x" + rgst + " #" + (index + 8));

        }
        else if (((word >> 31) & 1 )== 1)
        {
            uint immlo=extractBits(word, 29, 30);
            int rgst = (int)extractBits(word, 0, 4);
            uint immhi=extractBits(word, 5, 23);
            int dist = (int)((immhi << 2) | immlo);
            int scaled_dist = dist * 4096;
            ulong rad = index & ~0xFFFUL;
            File.AppendAllText(op + "bytecarve.s", rad + ": adrp x" + rgst + " #" + (rad+(ulong)scaled_dist));
        }
    }

    public void addsubs(uint word)
    {
        string typ = "";
        uint reg = extractBits(word, 31, 31);
        if ((int)reg == 0)
        { 
            typ="W";
        }
        else if ((int)reg == 1)
        {
            typ="X"
        }
        
    }
    
}
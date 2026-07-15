using System;
using Avalonia.Media.TextFormatting;

namespace ByteCarve.Services;
using System.Collections;
using System.Linq;
public class DP_immediate
{
    private uint word;
    private int index;
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo-hi + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }
    public void process_it(byte[] tempo,int ts)
    {
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
            //here i ll add a file.write(index+": adr x"+"rgst+" #"+(index+8)) 

        }
    }
    
}
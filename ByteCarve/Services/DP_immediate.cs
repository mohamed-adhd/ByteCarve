using System;
using Avalonia.Media.TextFormatting;

namespace ByteCarve.Services;
using System.Collections;
using System.Linq;
public class DP_immediate
{
    private byte[] data;
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = hi - lo + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }
    public void process_it(byte[] tempo)
    {
        data = tempo;
        uint word=BitConverter.ToUInt32(data, 0);
        uint sig=extractBits(word,25, 23);
        if (sig == 0b001 || sig == 0b000)
        {
            pcaddress(data);
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

    public void pcaddress()
    {
        
    }
    
}
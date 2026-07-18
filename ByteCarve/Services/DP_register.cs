namespace ByteCarve.Services;
using System;
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
        
        
    }
}
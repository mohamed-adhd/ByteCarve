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
                        }else if (op2.Equals(0b1000))
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
}
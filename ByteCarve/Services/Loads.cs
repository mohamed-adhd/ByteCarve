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
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo - hi + 1;
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
        
    }public void ppri(uint word)
    {
        
    }public void ui(uint word)
    {
        
    }public void usci(uint word)
    {
             
    }
    public void opstid(uint word)
    {
      
    }
    public void preid(uint word)
    {
                       
    }
    public void reg(uint word)
        {
            
        }
}
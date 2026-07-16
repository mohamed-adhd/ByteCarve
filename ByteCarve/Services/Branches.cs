namespace ByteCarve.Services;

public class Branches
{
    ulong index;
    public Branches(ulong ts)
    {
        index = ts;
    }
    static uint extractBits(uint word, int hi, int lo)
    {
        int width = lo - hi + 1;
        uint mask = (1u << width) - 1;
        return (word >> lo) & mask;
    }

    public void process_it(uint word)
    {
        uint top8 = extractBits(word, 24,31);
        uint top7 = extractBits(word, 25,31);
        uint top6 = extractBits(word, 25,30);
        uint top5 = extractBits(word, 26,30);
        if (top8 == 0b11010100) return exceptions(word);
        if (top8 == 0b11010101) return systems(word);
        if (top7 == 0b1101011)  return Uncond(word);
        if (top7 == 0b0101010)  return Cond(word);
        if (top6 == 0b011010)   return Compare(word);
        if (top6 == 0b011011)   return Test(word);
        if (top5 == 0b00101)    return UncondBranchImmediate(word);
        



    }
}
using System.Collections;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using static  System.IO.File;
namespace ByteCarve.Models;

public class disassembler
{
    public byte[] data,chunk;
    private BitArray bitchunk;
    private bool looking = true;
    public int index = 0;

    public disassembler(string path)
    {
        data=ReadAllBytes(path);
    }

    public void lord_have_mercy()
    {
        while (looking)
        {
            chunk = data.AsSpan(index,4).ToArray();
            BitArray result = new BitArray(new BitArray(chunk).Cast<bool>().Skip(25).Take(4).ToArray());
            byte[] bytes = new byte[(result.Length + 7)/8];// extracting a byte section , taking a bits chunk from it and then transforming it back into bytes  , yay i m having so much fun on 10:52 on a random fucking tuesday night
            result.CopyTo(bytes, 0);
            int sig = BinaryPrimitives.ReadInt32LittleEndian(bytes);
            
        
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    public void DP_immediate(byte[] data)
    {
        
    }
    public void Branches(byte[] data)
    {
        
    }
    public void Loads(byte[] data)
    {
        
    }
    public void DP_Register(byte[] data)
    {
        
    }
    public void DP_Scalar(byte[] data)
    {
        
    }
}
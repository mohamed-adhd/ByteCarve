using System.Collections;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using static  System.IO.File;
namespace ByteCarve.Models;
using ByteCarve.Services;
public class disassembler
{
    public List<int> loadgrp = [4, 6, 12, 14];
    public List<int> dpreggrp = [5,13];
    public List<int> dpsimmgrp = [7,15];
    public List<int> dpimngrp = [8,9];
    public List<int> branch = [10,11];
    public byte[] data, chunk;
    private BitArray bitchunk;
    private bool looking = true;
    public int index = 0;
    
    private DP_immediate dp_immediate;
    private DP_register dp_register;
    private DP_scalar dp_scalar;
    private Loads loads;
    private Branches branches;
    
    
    
    
    
    

    public disassembler(string path)
    {
        data = ReadAllBytes(path);
    }

    public void lord_have_mercy()
    {
        while (looking)
        {
            chunk = data.AsSpan(index, 4).ToArray();
            BitArray result = new BitArray(new BitArray(chunk).Cast<bool>().Skip(25).Take(4).ToArray());
            byte[]
                bytes = new byte[4]; // extracting a byte section , taking a bits chunk from it and then transforming it back into bytes  , yay i m having so much fun on 10:52 on a random fucking tuesday night
            result.CopyTo(bytes, 0);
            int sig = 0;
            for (int i = 0; i < 4; i++)
                if (result[i])
                {
                    sig |= (1 << i);
                }

            if (loadgrp.Contains(sig))
            {
                Loads(chunk);
            }else if (dpimngrp.Contains(sig))
            {
                DP_immediate(chunk);
            }else if (dpreggrp.Contains(sig))
            {
                DP_Register(chunk);
            }else if (dpsimmgrp.Contains(sig))
            {
                DP_Scalar(chunk);
            }else if (branch.Contains(sig))
            {
                Branches(chunk);
            }



        }
















    }
    }

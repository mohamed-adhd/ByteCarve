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
    public ulong index=0;
    
    private DP_immediate dp_immediate;
    private DP_register dp_register;
    private DP_scalar dp_scalar;
    private Loads loads;
    private Branches branches;
    private string op;







    public disassembler(string path,string op)
    {
        data = ReadAllBytes(path);
        this.op = op;
        dp_immediate = new DP_immediate();
        dp_register = new DP_register(op);
        dp_scalar = new DP_scalar(index, op);
        loads = new Loads(index, op);
        branches = new Branches(index, op);
    }

    public void lord_have_mercy()
    {
        while (index + 4 <= (ulong)data.Length)
        {
            chunk = data.AsSpan((int)index, 4).ToArray();
            BitArray result = new BitArray(new BitArray(chunk).Cast<bool>().Skip(25).Take(4).ToArray());
            byte[] bytes = new byte[4]; // extracting a byte section , taking a bits chunk from it and then transforming it back into bytes  , yay i m having so much fun on 10:52 on a random fucking tuesday night
            result.CopyTo(bytes, 0);
            int sig = 0;
            for (int i = 0; i < 4; i++)
                if (result[i])
                {
                    sig |= (1 << i);
                }

            if (loadgrp.Contains(sig))
            {
                loads.process_it(chunk);
            }else if (dpimngrp.Contains(sig))
            {
                dp_immediate.process_it(chunk, index,op);
            }else if (dpreggrp.Contains(sig))
            {
                dp_register.process_it(chunk);
            }else if (dpsimmgrp.Contains(sig))
            {
                dp_scalar.process_it(chunk);
            }else if (branch.Contains(sig))
            {
                branches.process_it(chunk,index);
            }
            index += 4;
        }
















    }
    }

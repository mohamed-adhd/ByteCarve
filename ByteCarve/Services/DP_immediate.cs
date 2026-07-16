using System;
using Avalonia.Media.TextFormatting;
using System.IO;
namespace ByteCarve.Services;
using System.Collections;
using System.Linq;
using Gee.External.Capstone;
using Gee.External.Capstone.Arm;

public class DP_immediate
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

    public void process_it(byte[] tempo, ulong ts, string op)
    {
        this.op = op;
        index = ts;
        uint word = BitConverter.ToUInt32(tempo, 0);
        uint sig = extractBits(word, 25, 23);
        if (sig == 0b001 || sig == 0b000)
        {
            pcaddress(word);
        }
        else if (sig == 0b010 || sig == 0b011)
        {
            addsubs(word);
        }
        else if (sig == 0b100)
        {
            logic(tempo);
        }
        else if (sig == 0b101)
        {
            movwide(word);
        }
        else if (sig == 0b110)
        {
            bitfild(word);
        }
        else if (sig == 0b111)
        {
            extrct(word);
        }
    }

    public void pcaddress(uint word)
    {
        if (((word >> 31) & 1) == 0)
        {
            uint immlo = extractBits(word, 29, 30);
            int rgst = (int)extractBits(word, 0, 4);
            uint immhi = extractBits(word, 5, 23);
            int dist = (int)((immhi << 2) | immlo);
            if ((dist & (1 << 20)) != 0)
            {
                dist |= unchecked((int)0xFFE00000);
            }

            File.AppendAllText(op + "bytecarve.s", index + ": adr x" + rgst + " #" + (index + 8));

        }
        else if (((word >> 31) & 1) == 1)
        {
            uint immlo = extractBits(word, 29, 30);
            int rgst = (int)extractBits(word, 0, 4);
            uint immhi = extractBits(word, 5, 23);
            int dist = (int)((immhi << 2) | immlo);
            int scaled_dist = dist * 4096;
            ulong rad = index & ~0xFFFUL;
            File.AppendAllText(op + "bytecarve.s", rad + ": adrp x" + rgst + " #" + (rad + (ulong)scaled_dist));
        }
    }

    public void addsubs(uint word)
    {
        string typ = "";
        string opr = "";
        string sourcet, dest;
        uint reg = extractBits(word, 31, 31);
        uint opru = extractBits(word, 30, 30);
        uint flag = extractBits(word, 29, 29);
        uint shflag = extractBits(word, 22, 23);
        int source = (int)extractBits(word, 5, 9);
        int destination = (int)extractBits(word, 0, 4);
        int imm12 = (int)extractBits(word, 10, 21);
        if (source == 31)
        {
            sourcet = "sp";
        }
        else
        {
            sourcet = source.ToString();
        }

        if (destination == 31)
        {
            dest = "sp";
        }
        else
        {
            dest = destination.ToString();
        }

        if ((int)reg == 0)
        {
            typ = "w";
        }
        else
        {
            typ = "x";
        }

        if ((int)opru== 0)
        {
            if ((int)flag == 1)
            {
                opr = "adds";
            }
            else
            {
                opr = "add";
            }

        }
        else
        {
            if ((int)flag == 1)
            {
                opr = "subs";
            }
            else
            {
                opr = "sub";
            }
        }

        if ((int)shflag == 1)
        {
            imm12 *= 4096;
        } 
        File.AppendAllText(op + "bytecarve.s", opr + " " +typ+ dest + ", " +typ+sourcet+", #"+imm12.ToString());

    }

    public void movwide(uint word)
    {
        string typ="";
        int opc=(int)extractBits(word,29,30);
        int slice = (int)extractBits(word, 21, 22);
        uint val=extractBits(word, 5,20);
        int rd = (int)extractBits(word, 0, 4);
        if ((int)extractBits(word, 31, 31) == 0)
        {
            if (opc == 00)
            {
                typ = "movn";
            }else if (opc == 10)
            {
                typ = "movz";
            }else if (opc == 11)
            {
                typ = "movk";
            }
        }
        string shiftText = (slice == 0) ? "" : $", lsl #{slice*16}";
        File.AppendAllText(op + "bytecarve.s", typ +" x"+rd.ToString()+val.ToString()+shiftText);
    }

    public void logic(byte[] word)
    {
        var dis = CapstoneDisassembler.CreateArmDisassembler(ArmDisassembleMode.Arm);
        var ins = dis.Disassemble(word,(long)index)[0];
        File.AppendAllText(op + "bytecarve.s",ins.ToString());
    }

    public void bitfild(uint word)
    {
        string line="";
        int reg = (int)extractBits(word, 31, 31);
        int tyo = (int)extractBits(word,29, 30);
        int rt=(int)extractBits(word, 16, 21);
        int es=(int)extractBits(word, 10, 15);
        int src=(int)extractBits(word, 5, 9);
        int dst=(int)extractBits(word, 0, 4);
        switch (tyo)
        {
            case 0:
                if (rt == 0 && es == 7)
                {
                    line="SXTB "+dst+", "+src;
                }else if (rt == 0 && es == 15)
                {
                    line="SXTH "+dst+", "+src;
                }else if (rt == 0 && es == 31)
                {
                    line="SXTW "+dst+", "+src;
                }else if ((es == 31 && reg == 0) || (es == 63 && reg == 1))
                {
                    line = "ASR" + dst + ", " + src + " #" + rt;
                }
                
                break;
            case 1:
                if (rt == 0 && es == 7)
                {
                    line="UXTB "+dst+", "+src;
                }else if (rt == 0 && es == 15)
                {
                    line="UXTH "+dst+", "+src;
                }else if ((es == 31 && reg == 0) || (es == 63 && reg == 1))
                {
                    line = "LSR" + dst + ", " + src + " #" + rt;
                }else if (es == rt - 1)
                {
                    int regsize = (reg == 0) ? 32 : 64;
                    line = "LSL " + dst + "," + src + ", #" + (regsize - rt);
                }
                break;
            case 10:
                line = "BFM" + dst + ", " + src + ", #" + rt + ", #" + es;
                break;
        }
        
    }

    public void extrct(uint word)
    {
        int sf = (int)extractBits(word, 31, 31);
        int rm = (int)extractBits(word, 16, 20);
        

    }
    
}
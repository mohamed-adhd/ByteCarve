using System;
using System.Buffers.Binary;
using static System.Text.Encoding;
using Tmds.DBus.Protocol;
using System.IO;
using System.Collections.Generic;
namespace ByteCarve.Models;
using static  System.IO.File;
using System.Linq;
public class carver

{
    public List<byte[]> images = new();    
    public int pngStart,pngEnd;
    private int index = 0;
    private byte[] file;
    int value = 0;
    private static readonly byte[] PngSig={0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A};
    byte[] cur8; 
    Boolean looking = true;
    string type = "";
    public carver(string path)
    {
        file = ReadAllBytes(path);
        
    }

    public void Carvethashi()
    
    {
      
        while (index <= file.Length - PngSig.Length)
        {
            cur8=file.AsSpan(index, 8).ToArray();
            if (cur8.SequenceEqual(PngSig))
            {
                pngStart = index;
                int cp= pngStart + 8;
                while (looking)
                {
                    if (cp + 12 > file.Length)
                        break;
                    uint len = BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(cp, 4));
                    type = (ASCII.GetString(file, cp+4, 4));
                    if (type !="IEND")
                    {
                        cp+= 12 + (int)len;
                    }
                    else
                    {
                        pngEnd=index+20 + (int)len;
                        looking = true;
                        images.Add(file.AsSpan(pngStart, pngEnd-pngStart).ToArray());
                    }
                }
                
            }

            index++;

        }
        
    }

    
}
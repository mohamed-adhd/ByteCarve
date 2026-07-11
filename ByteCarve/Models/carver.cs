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
    string type = "";
    public carver(string path)
    {
        file = ReadAllBytes(path);
        
    }

    public void Carvethashi()
    
    {
      
        while (type != "IEND")
        {
            cur8=file.AsSpan(index, 8).ToArray();
            if (cur8.SequenceEqual(PngSig))
            {
                pngStart = index+8;
                while (type != "IEND")
                {
                    uint len = BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(pngStart + 8, 4));
                    type = (ASCII.GetString(file, pngStart + 4, 4));
                    if (type == "IHDR")
                    {
                        images.Add(file.AsSpan(pngStart+16, (int)len).ToArray());
                        
                    }
                }
                
            }

            index++;

        }
        
    }

    
}
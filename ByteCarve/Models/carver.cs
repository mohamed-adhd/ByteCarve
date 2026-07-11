using System;
using static System.Text.Encoding;
using System.IO;
using System.Collections.Generic;
namespace ByteCarve.Models;
using static  System.IO.File;

public class carver

{
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
      cur8=file.AsSpan(index, 8).ToArray();
        while (type != "IEND")
        {
            if (cur8.SequenceEqual(PngSig))
            {
                pngStart = index+8;
                while (type != "IEND")
                {
                    byte[] temp=file.AsSpan(pngStart+8, 8).ToArray();
                    int len = int.Parse(ASCII.GetString(file, pngStart, 4));
                    string type = (ASCII.GetString(file, pngStart + 4, 4));
                    if (type == "IHDR")
                    {
                        
                    }




                }
            }
            
        }
        
    }

    public void parser(int s)
    {
        int cp=pngStart+
    }
}
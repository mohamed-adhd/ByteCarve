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
    public int jpgStart,jpgEnd;
    private int index = 0;
    private byte[] file;
    int value = 0;
    private static readonly byte[] PngSig={0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A};
    private static readonly byte[] JpgSig = {0xFF, 0xD8, 0xFF};
    
    byte[] cur8; 
    byte[] cur3;
    bool looking = true;
    string type = "";
    public carver(string path)
    {
        file = ReadAllBytes(path);
    }
    public int Carvethashi()
    {
      
        while (index <= file.Length - PngSig.Length)
        {
            cur8=file.AsSpan(index, 8).ToArray();
            cur3=file.AsSpan(index, 3).ToArray();
            if (cur8.SequenceEqual(PngSig))
            {
                pngStart = index;
                int cp= pngStart + 8;
                looking = true;
                while (looking)
                {
                    if (cp + 12 > file.Length)
                        break;
                    uint len = BinaryPrimitives.ReadUInt32BigEndian(file.AsSpan(cp, 4));
                    type = (ASCII.GetString(file, cp+4, 4));
                    if (type !="IEND")
                    {
                        long nextCp = (long)cp + 12 + len;
                        if (nextCp > file.Length)
                            break;
                        cp = (int)nextCp;
                    }
                    else
                    {
                        pngEnd=cp+ 12+ checked((int)len);
                        index = pngEnd - 1;
                        looking = false;
                        images.Add(file.AsSpan(pngStart, pngEnd-pngStart).ToArray());
                        
                    }
                }
                
            }else if (cur3.SequenceEqual(JpgSig))
            {
                bool inscan = false;
                jpgStart = index;
                int cp=jpgStart + 2;
                looking = true;
                while (looking)
                {
                    if (inscan)
                    {
                        
                    }
                    else
                    {
                        byte[] temp = file.AsSpan(cp, 2).ToArray();
                        if (cp + 4 > file.Length)
                            break;
                        uint len = BinaryPrimitives.ReadUInt16BigEndian(file.AsSpan(cp + 2, 2));
                        byte[] Jpgend = {0xff,0xD9};
                        if (!temp.SequenceEqual(Jpgend)){
                            long nextCp = (long)cp + 2 + len;
                            if (nextCp > file.Length)
                                break;
                            cp = (int)nextCp;
                        }
                        else
                        {
                            jpgEnd=cp+ 2;
                            index = jpgEnd - 1;
                            looking = false;
                            images.Add(file.AsSpan(jpgStart, jpgEnd-jpgStart).ToArray());
                        
                        }
                    }
                }
            }

            index++;

        }

        return images.Count;
    }

    public void write(string path)
    {
        int i = 0;
        foreach (Byte[] s in images)
        {
            WriteAllBytes(path + "image_" + i+".png", s);
        }
       
    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LowLevelApiUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodingExample();
            DecodingExample();
        }

        static void EncodingExample()
        {
            byte[] sourceFile = File.ReadAllBytes(@"C:\tmp\diff\test.7z");
            List<byte> result = new List<byte>();

            using (VCDiffDotNet.LowLevel.Encoder encoder = new VCDiffDotNet.LowLevel.Encoder())
            {
                encoder.InitEncoder(sourceFile, true, true, false, false);

                byte[] partialOutput;
                var ret = encoder.StartEncoding(out partialOutput);
                result.AddRange(partialOutput);

                byte[] partOfTargetFile = File.ReadAllBytes(@"C:\tmp\diff\test-new.7z");
                ret = encoder.EncodeChunk(partOfTargetFile, out partialOutput);
                result.AddRange(partialOutput);

                ret = encoder.FinishEncoding(out partialOutput);
                result.AddRange(partialOutput);
            }

            byte[] patchFile = result.ToArray();
            File.WriteAllBytes(@"C:\tmp\diff\test.patch", patchFile);
        }

        static void DecodingExample()
        {
            byte[] sourceFile = File.ReadAllBytes(@"C:\tmp\diff\test.7z");
            List<byte> result = new List<byte>();

            using (VCDiffDotNet.LowLevel.Decoder decoder = new VCDiffDotNet.LowLevel.Decoder())
            {
                int maxSize = 1 << 24;
                decoder.InitDecoder(sourceFile, true, maxSize, maxSize);
                decoder.StartDecoding();

                byte[] partialOutput;
                byte[] partOfPatchFile = File.ReadAllBytes(@"C:\tmp\diff\test.patch");
                decoder.DecodeChunk(partOfPatchFile, out partialOutput);
                result.AddRange(partialOutput);

                decoder.FinishDecoding();
            }

            byte[] targetFile = result.ToArray();
            File.WriteAllBytes(@"C:\tmp\diff\test-patched.7z", targetFile);
        }
    }
}
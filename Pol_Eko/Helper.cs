using System;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

public static class Helper
{
    public static long Main(string bzip2FilePath)
    {
        long totalUncompressedSize = 0;

        try
        {
            var compressedDataByteArray = File.ReadAllBytes(bzip2FilePath);

            using (var mstream = new MemoryStream(compressedDataByteArray))
            using (var unzipstream = new BZip2InputStream(mstream))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = unzipstream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalUncompressedSize += bytesRead;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error calculating BZip2 uncompressed size: {ex.Message}");
        }

        return totalUncompressedSize;
    }
}
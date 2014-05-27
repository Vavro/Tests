using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JobManagement;

namespace FullTextIndexTests.IFilter
{
    public class ParseHelper
    {
        public static string ParseIFilter(Stream s, string fileName)
        {
            var job = new Job();
            job.AddCurrentProces();

            // Get an IFilter for a file or file extension
            IFilter filter = null;
            var fileExtension = Path.GetExtension(fileName);
            FilterReturnCodes result = NativeMethods.LoadIFilter(fileExtension, null, ref filter);
            if (result != FilterReturnCodes.S_OK)
            {
                Marshal.ThrowExceptionForHR((int)result);
            }

            // Copy the content to global memory
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            IntPtr nativePtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativePtr, buffer.Length);

            // Create a COM stream
            System.Runtime.InteropServices.ComTypes.IStream comStream;
            NativeMethods.CreateStreamOnHGlobal(nativePtr, true, out comStream);

            // Load the contents to the iFilter using IPersistStream interface
            var persistStream = (IPersistStream)filter;
            persistStream.Load(comStream);


            const FilterInit iflags = FilterInit.IFILTER_INIT_CANON_HYPHENS |
                                                      FilterInit.IFILTER_INIT_CANON_PARAGRAPHS |
                                                      FilterInit.IFILTER_INIT_CANON_SPACES |
                                                      FilterInit.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES |
                                                      FilterInit.IFILTER_INIT_HARD_LINE_BREAKS |
                                                      FilterInit.IFILTER_INIT_FILTER_OWNED_VALUE_OK |
                                                      FilterInit.IFILTER_INIT_INDEXING_ONLY;

            // Initialize iFilter
            FilterFlags filterFlags;
            FilterReturnCodes result2 = filter.Init(
               iflags, 0, IntPtr.Zero, out filterFlags);

            return ExtractTextFromIFilter(filter);
        }

        public static string ExtractTextFromIFilter(IFilter filter)
        {
            var sb = new StringBuilder();

            while (true)
            {
                StatChunk chunk;
                var result = filter.GetChunk(out chunk);

                if (result == FilterReturnCodes.S_OK)
                {
                    if (chunk.flags == ChunkState.CHUNK_TEXT)
                    {
                        switch (chunk.breakType)
                        {
                            case ChunkBreaktype.CHUNK_NO_BREAK:
                                break;
                            case ChunkBreaktype.CHUNK_EOW:
                                sb.Append(" ");
                                break;
                            case ChunkBreaktype.CHUNK_EOS:
                            case ChunkBreaktype.CHUNK_EOP:
                            case ChunkBreaktype.CHUNK_EOC:
                                sb.Append(Environment.NewLine);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        sb.Append(ExtractTextFromChunk(filter, chunk));
                    }

                    continue;
                }

                if (result == FilterReturnCodes.FILTER_E_END_OF_CHUNKS)
                {
                    return sb.ToString();
                }

                Marshal.ThrowExceptionForHR((int)result);
            }
        }

        public static string ExtractTextFromChunk(IFilter filter, StatChunk chunk)
        {
            var sb = new StringBuilder();

            var result = FilterReturnCodes.S_OK;
            while (result == FilterReturnCodes.S_OK)
            {
                int sizeBuffer = 16384;
                var buffer = new StringBuilder(sizeBuffer);
                result = filter.GetText(ref sizeBuffer, buffer);

                if ((result == FilterReturnCodes.S_OK) || (result == FilterReturnCodes.FILTER_S_LAST_TEXT))
                {
                    if ((sizeBuffer > 0) && (buffer.Length > 0))
                    {
                        sb.Append(buffer.ToString(0, sizeBuffer));
                    }
                }

                if (result == FilterReturnCodes.FILTER_E_NO_TEXT)
                {
                    return string.Empty;
                }

                if ((result == FilterReturnCodes.FILTER_S_LAST_TEXT) || (result == FilterReturnCodes.FILTER_E_NO_MORE_TEXT))
                {
                    return sb.ToString();
                }
            }

            return sb.ToString();
        }
    }
}

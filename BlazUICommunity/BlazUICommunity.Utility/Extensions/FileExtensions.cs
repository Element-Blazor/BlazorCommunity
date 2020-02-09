using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blazui.Community.Utility.Extensions
{
  public static  class FileExtensions
    {
        /// <summary>
        /// 读入到字节数组中比较(ReadOnlySpan)
        /// </summary>
        /// <param name="newStream">新文件流</param>
        /// <param name="targetStream">目标文件流</param>
        /// <returns></returns>
        public static bool CompareByReadOnlySpan( this Stream newStream , Stream targetStream)
        {
            const int BYTES_TO_READ = 1024 * 10;

            //using ( FileStream fs1 = System.IO.File.Open(file1 , FileMode.Open) )
            //using ( FileStream fs2 = System.IO.File.Open(file2 , FileMode.Open) )
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];
                while ( true )
                {
                    int len1 = newStream.Read(one , 0 , BYTES_TO_READ);
                    int len2 = targetStream.Read(two , 0 , BYTES_TO_READ);
                    // 字节数组可直接转换为ReadOnlySpan
                    if ( !( ( ReadOnlySpan<byte> ) one ).SequenceEqual(( ReadOnlySpan<byte> ) two) ) return false;
                    if ( len1 == 0 || len2 == 0 ) break;  // 两个文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }


        /// <summary>
        /// 读入到字节数组中比较(ReadOnlySpan)
        /// </summary>
        /// <param name="file1">文件路径1</param>
        /// <param name="file2">文件路径2</param>
        /// <returns></returns>
        public static bool CompareByReadOnlySpan( string file1, string file2)
        {
            const int BYTES_TO_READ = 1024 * 10;

            using (FileStream fs1 = File.Open(file1, FileMode.Open))
            using (FileStream fs2 = File.Open(file2, FileMode.Open))
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, BYTES_TO_READ);
                    int len2 = fs2.Read(two, 0, BYTES_TO_READ);
                    // 字节数组可直接转换为ReadOnlySpan
                    if (!((ReadOnlySpan<byte>)one).SequenceEqual((ReadOnlySpan<byte>)two)) return false;
                    if (len1 == 0 || len2 == 0) break;  // 两个文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }
    }
}

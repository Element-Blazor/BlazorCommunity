using System;
using System.Text;

namespace BlazorCommunity.Api.Service
{
    public class CodeService : ICodeService
    {
        public string GenerateNumberCode(int length = 6)
        {
            if (length > 8 || length < 4)
                throw new ArgumentException("length mast bettween 4-8");

            var Code = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                Code.Append(r.Next(0, 10));
            }
            return Code.ToString();
        }

        public string GenerateNumberLetterCode(int length = 4)
        {
            if (length > 8 || length < 4)
                throw new ArgumentException("length mast bettween 4-8");
            var Code = "";
            var random = new Random((int)DateTime.Now.Ticks);
            const string textArray = "23456789ABCDEFGHGKLMNPQRSTUVWXYZ";
            for (var i = 0; i < length; i++)
            {
                Code += textArray.Substring(random.Next() % textArray.Length, 1);
            }
            return Code;
        }
    }
}
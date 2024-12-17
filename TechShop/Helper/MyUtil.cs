using System.Text;
namespace TechShop.Helper
{
    public class MyUtil
    {
        public static String GenerateRandomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJJKLZXCVBNMK!@ ";
            var sb = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++) {
                sb.Append(pattern[random.Next(0,pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}

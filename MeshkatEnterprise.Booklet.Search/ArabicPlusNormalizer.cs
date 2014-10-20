using System;

namespace MeshkatEnterprise.Booklet.Search
{
    internal class ArabicPlusNormalizer
    {
        public String Normalize(String input, int len)
        {
            return input.Replace('ک', 'ك').Replace('ی', 'ي');
        }
    }
}
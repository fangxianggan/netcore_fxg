using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObjectSort
{
    class TestDemo : IComparable<TestDemo>
    {
        public string CloumnName { set; get; }

        public List<int> CloumnValues { set; get; } = new List<int>();

        public TestDemo(string name)
        {
            this.CloumnName = name;
        }
        public int CompareTo(TestDemo other)
        {
            if (this.CloumnName.Length >= 1)
            {
                ASCIIEncoding aSCII = new ASCIIEncoding();
                int temp = aSCII.GetBytes(this.CloumnName.Substring(0, 1))[0];
                int temp1 = aSCII.GetBytes(other.CloumnName.Substring(0, 1))[0];
                if (temp > temp1)
                {
                    return 1;
                }
                else if (temp < temp1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}

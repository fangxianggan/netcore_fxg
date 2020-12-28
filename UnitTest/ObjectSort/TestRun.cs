using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ObjectSort
{
    /// <summary>
    /// 面试题
    /// </summary>
    class TestRun
    {
        public TestRun()
        {
            List<TestDemo> list = new List<TestDemo>();
            string abc = "Beth,Charles,Danielle,Adam,Eric\n17945,10091,10088,3907,10132\n2,12,13,48,11";
            string[] cols = abc.Split('\n');
            string[] headTitle = cols[0].Split(',');

            int temp = 0;
            foreach (var item in headTitle)
            {
                TestDemo test = new TestDemo(item);
                for (int i = 1; i <= cols.Length - 1; i++)
                {
                    string[] v = cols[i].Split(',');
                    for (int j = 0; j <= v.Length - 1; j++)
                    {
                        if (temp == j)
                        {
                            int value = Convert.ToInt32(v[j]);
                            test.CloumnValues.Add(value);
                        }
                    }
                }
                list.Add(test);
                temp++;
            }

            list.Sort();
            StringBuilder str = new StringBuilder();
            StringBuilder vstr = new StringBuilder();

            for (int i = 0; i <= list.Count - 1; i++)
            {
                str = str.Append(list[i].CloumnName + " ");
            }
            for (int k = 0; k <= list[1].CloumnValues.Count - 1; k++)
            {
                for (int j = 0; j <= list.Count - 1; j++)
                {
                    string cc = list[j].CloumnValues[k].ToString();
                    vstr = vstr.Append(cc + " ");
                }
                vstr = vstr.Append("\n");
            }

            Console.WriteLine(str.ToString() + "\n" + vstr.ToString());
        }
    }
}

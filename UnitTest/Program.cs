using log4net;
using log4net.Config;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitTest.a;
using UnitTest.BuliderPattern;
using UnitTest.DesignModel.DecoratorMode;
using UnitTest.DIPDesign;

namespace UnitTest
{
    class Program
    {

        static void Main(string[] args)
        {

            var repository = LogManager.CreateRepository("CoreLog4");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));//配置文件路径可以自定义




            #region 前台线程 专属线程
            //ThreadClass thread = new ThreadClass();
            //thread.QianTaiThread();
            #endregion

            #region 后台线程 线程池工作
            //ThreadPoolClass threadPool = new ThreadPoolClass();
            //threadPool.Test3();
            #endregion

            //thread.Test1();
            //Console.WriteLine("1");
            //thread.Test2();
            //Console.WriteLine("2");
            //Console.ReadLine();

            //ObservelModelClass modelClass = new ObservelModelClass();
            //modelClass.test();

            //UserService user = new UserService();
            //user.SendNotify();

            //#region 网上测试题
            ////string abc = "Beth,Charles,Danielle,Adam,Eric\n17945,10091,10088,3907,10132\n2,12,13,48,11";
            ////var res = aa(abc);
            //var a = new int[] { 4, 2, 2, 3, 1, 4, 7, 8, 6, 9 };
            //int res = bb(a);
            //var b = new int[] { 9 };
            //int res2 = bb(b);

            //ProtectedA protectedClass = new ProtectedA();
            //protectedClass.aaTest();
            //#endregion


            #region 装饰器模式

            //TestClass test = new TestClass();
            //test.TestData();


            #endregion

            //#region 建造者模式

            //TestRun run = new TestRun();

            //#endregion

            //#region 观察者模式

            //UnitTest.ObserverPattern.TestRun run = new ObserverPattern.TestRun();

            //#endregion

            //#region 装饰者模式

            //UnitTest.DecoratorPattern.TestRun run = new DecoratorPattern.TestRun();

            //#endregion

            //#region 代理模式

            //UnitTest.ProxyPattern.TestRun run = new ProxyPattern.TestRun();

            //#endregion

            //#region 责任链模式

            //UnitTest.ChainPattern.TestRun run = new ChainPattern.TestRun();

            //#endregion

            //#region 对象比较大小

            //UnitTest.ObjectSort.TestRun run = new ObjectSort.TestRun();

            //#endregion

            #region 单例模式 

            UnitTest.Singleton.TaskRun run = new Singleton.TaskRun();

            #endregion

            Console.WriteLine();
          
            Console.ReadKey();
        }

        public static string aa(string abc)
        {
            string res = "";
            string[] rows = abc.Split('\n');
            string[] rowsCloumn = rows[0].Split(',');
            string[] tempCloumn = rows[0].Split(',');
            Array.Sort(rowsCloumn);
            List<int> indexList = new List<int>();
            for (int i = 0; i <= rows.Length - 1; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j <= rowsCloumn.Length - 1; j++)
                    {
                        var value = rowsCloumn[j];
                        indexList.Add(tempCloumn.ToList().FindIndex(p => p == value));
                        res += value;
                        if (j == rowsCloumn.Length - 1)
                        {
                            res += "\n";
                        }
                        else
                        {
                            res += ',';
                        }
                    }
                }
                else
                {

                    var rowsData = rows[i].Split(',');
                    for (int m = 0; m <= indexList.Count() - 1; m++)
                    {
                        var value = rowsData[indexList[m]];
                        res += value;
                        if (m == rowsData.Length - 1)
                        {
                            if (i != rows.Length - 1)
                            {
                                res += "\n";
                            }
                        }
                        else
                        {
                            res += ',';
                        }
                    }
                }
            }
            return res;





        }

        public static int bb(int[] items)
        {
            int result = -1;
            for (int i=0;i<=items.Length-1;i++)
            {
                var value = items[i];
                var flag = true;

                for (int j=i+1;j<=items.Length-1;j++)
                {
                    var temp = items[j];
                    if (temp < value)
                    {
                        flag = false;
                        break;
                    }
                }

                for (int k=i-1;k>=0;k--)
                {
                    var temp = items[k];
                    if (temp>value) {
                        flag = false;
                        break;
                    }
                }

                if (flag) {
                    result = i;
                }
            }
            return result;
        }

    }
}

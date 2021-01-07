using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.Singleton
{
    public class TaskRun
    {
        public TaskRun()
        {
            {
                Console.WriteLine("***************单例模式********************");
                //{
                    
                //    for (int i = 0; i < 10; i++)
                //    {
                       
                //            SingletonPattern singleton = SingletonPattern.CreateInstance();
                //            singleton.Id = i;
                //            singleton.Name = "hhh" + i;
                //            singleton.Class.ClassID = i + i;
                //            singleton.Class.ClassName = "class" + i;
                //            singleton.PrintData();
                       
                //    }
                //}


                {
                    SingletonPattern singleton1 = SingletonPattern.CreateInstance();
                    SingletonPattern singleton2 = SingletonPattern.CreateInstance();
                    SingletonPattern singleton3 = SingletonPattern.CreateInstance();
                    singleton1.Id = 1;
                    singleton1.Name = "hhh" + 1;
                    singleton1.Class.ClassID = 11;
                    singleton1.Class.ClassName = "class" + 1;
                    

                    singleton2.Id = 2;
                    singleton2.Name = "hhh" + 2;
                    singleton2.Class.ClassID = 22;
                    singleton2.Class.ClassName = "class" + 2;
                   

                    singleton3.Id = 3;
                    singleton3.Name = "hhh" + 3;
                    singleton3.Class = new Class()
                    {
                        ClassID = 33,
                        ClassName = "class" + 3
                    };


                    singleton1.PrintData();
                    singleton2.PrintData();
                    singleton3.PrintData();
                  
                    
                }



                Console.WriteLine("*******************************************");
            }
        }
    }
}

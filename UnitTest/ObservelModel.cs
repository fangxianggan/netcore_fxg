using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{

    //抽象主题类
    public interface ISubject
    {

        //添加观察者 送零食的加进来，老板来了通知你
        void Add(Observer observer);
        //删除观察者 不送零食的秘书小妹就不通知了
        void Remove(Observer observer);
        //主题状态
        string SubjectState { get; set; }
        //通知方法
        void Notify();
    }


    /// <summary>
    /// 秘书2
    /// </summary>
    public class Mishu2 : ISubject
    {
        List<Observer> observersList = new List<Observer>();

        public Action doEvent;
        public string SubjectState { get; set; }

        public void Add(Observer observer)
        {
            observersList.Add(observer);
        }

        public void Notify()
        {
            doEvent();
        }

        public void Remove(Observer observer)
        {
            observersList.Remove(observer);
        }
    }

    //具体主题 ，秘书类
    public class Mishu : ISubject
    {
        //秘书要知道通知哪些同事
        private IList<Observer> observers = new List<Observer>();

        public void Add(Observer observer)
        {
            observers.Add(observer);
        }
        public void Remove(Observer observer)
        {
            observers.Remove(observer);
        }
        public string SubjectState { get; set; }
        public void Notify()
        {
            foreach (Observer o in observers)
            {
                o.Update();
            }
        }
    }



    //抽象观察者
    public abstract class Observer
    {
        //名字
        protected string name;
        //观察者要知道自己订阅了那个主题
        protected ISubject sub;
        public Observer(string name, ISubject sub)
        {
            this.name = name;
            this.sub = sub;
        }
        //接受到通知后的更新方法
        public abstract void Update();
    }

    //看股票的同事
    public class StockObserver : Observer
    {
        public StockObserver(string name, ISubject sub) : base(name, sub) { }
        public override void Update()
        {
            Console.WriteLine($"通知内容：{sub.SubjectState},反应：{name}关闭股票行情，继续工作！");
        }

        public void Notice1()
        {
            Console.WriteLine($"通知内容：{sub.SubjectState},反应：{name}关闭股票行情，继续工作！");
        }
    }
    //看NBA的同事
    public class NBAObserver : Observer
    {
        public NBAObserver(string name, ISubject sub) : base(name, sub) { }
        public override void Update()
        {
            Console.WriteLine($"通知内容：{sub.SubjectState},反应：{name}关闭NBA直播，继续工作！");
        }

        public void Notice2()
        {
            Console.WriteLine($"通知内容：{sub.SubjectState},反应：{name}继续观看NBA直播，半个小时后工作！");
        }
    }

    public class ObservelModelClass
    {
        public void test()
        {
            Mishu mishu = new Mishu();
            //新建同事 观察者角色
            Observer tongshi1 = new StockObserver("巴菲特", mishu);
            Observer tongshi2 = new NBAObserver("麦迪", mishu);
            //秘书小妹要知道哪些同事要通知（主题要知道所有订阅了自己的观察者）
            mishu.Add(tongshi1);
            mishu.Add(tongshi2);
            //主题状态更改了
            mishu.SubjectState = "老板回来了！";
            //调用主题的通知方法
            mishu.Notify();

            Mishu2 mishu2 = new Mishu2();
            //新建同事 观察者角色
            StockObserver tongshi11 = new StockObserver("巴菲特", mishu2);
            NBAObserver tongshi22 = new NBAObserver("麦迪", mishu2);
            mishu2.doEvent += tongshi11.Notice1;
            mishu2.doEvent += tongshi22.Notice2;

            //秘书小妹要知道哪些同事要通知（主题要知道所有订阅了自己的观察者）
            mishu2.Add(tongshi11);
            mishu2.Add(tongshi22);
            //主题状态更改了
            mishu2.SubjectState = "老板又走了！";
            //调用主题的通知方法
            mishu2.Notify();


           Console.ReadKey();
        }
    }
}

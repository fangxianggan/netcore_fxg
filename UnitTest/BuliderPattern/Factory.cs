using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.BuliderPattern
{
    public class Factory
    {
        protected Bulider _bulider;
        public Factory(Bulider bulider)
        {
            _bulider = bulider;
        }

        public void Construct()
        {
            _bulider.DoListen();
            _bulider.DoLook();
            _bulider.DoWrite();
            _bulider.DoRead();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;


namespace DataManager
{
    public class Demo : BaseModel
    {
        public string Data1 { get; set; }
        public int Data2 { get; set; }
        public bool Data3 { get; set; }
    }

    public class DemoSheet : Sheet<Demo> { }
}

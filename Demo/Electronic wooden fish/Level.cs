using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_wooden_fish
{
    internal class Level
    {
        public Dictionary<int, string> LevelFo { get; set; }
        public Dictionary<int, string> LevelDao { get; set; }
        public List<string> stringsFo { get; set; }
        public List<string> stringsDao { get; set; }

        public Level()
        {
            LevelFo = new Dictionary<int, string>();
            LevelFo.Add(1, "结缘");
            LevelFo.Add(2, "正见");
            LevelFo.Add(3, "正修");
            LevelFo.Add(4, "正行");

            LevelDao = new Dictionary<int, string>();
            LevelDao.Add(1, "炼精化气");
            LevelDao.Add(2, "炼气化神");
            LevelDao.Add(3, "炼神还虚");
            LevelDao.Add(4, "炼虚合道");

            stringsFo = new List<string>();
            stringsFo.Add("唵嘛呢叭咪吽");
            stringsFo.Add("嗡嘛智牟耶萨列德");

            stringsDao = new List<string>();
            stringsDao.Add("守一");
            stringsDao.Add("吐纳");
            stringsDao.Add("导引");
            stringsDao.Add("行气");
            stringsDao.Add("存神");
            stringsDao.Add("坐忘");
            stringsDao.Add("心斋");
            stringsDao.Add("存神");

        }
    }
}

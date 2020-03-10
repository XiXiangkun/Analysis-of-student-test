using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp
{
    class 学生
    {
        public object[] score;//分数
        public string num;//学号
        public int total_grades = 0;//总成绩
        public int bj;

        public 学生(int _num,int _bj,int y_score,int s_score,int e_score)
        {
            num = _num.ToString();
            score = new object[3];
            bj = _bj;
            score[0] = y_score;
            score[1] = s_score;
            score[2] = e_score;
            total_grades = y_score + s_score + e_score;
        }
        public void get_info()
        {
            Console.WriteLine("学号：{0}；语文成绩：{1}；数学成绩：{2}；英语成绩：{3}；总成绩：{4}",num,score[0],score[1],score[2],total_grades);
        }
    }
}

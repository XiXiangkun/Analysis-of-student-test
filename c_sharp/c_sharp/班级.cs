using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp
{
    class 班级
    {
        public int num;// 班级？
        public 教师[] tea;
        public 学生[] stu;
        public int total_num;//班级总人数
        public float total_ave = 0;
        public float[] sub_ave;
        public float[] sub_pas=new float[3];

        public 班级(int _num)
        {
            num = _num;
        }
        public void count_ave()
        {
            int n = 0;
            for(int i = 0; i < stu.Length; i++)
            {
                n += stu[i].total_grades;
            }
            total_ave = n / total_num;
            //Console.WriteLine("#####");
            //Console.WriteLine(total_ave);
        }
        public void get_s(int s_num)
        {

            for(int i = 0; i < total_num; i++)
            {
                if (stu[i].num == s_num.ToString())
                {
                    Console.WriteLine("学生学号：{0}；班级：{1}；总成绩：{2}", stu[i].num, stu[i].bj, stu[i].total_grades);
                }
            }
        }
        public void count_sub_ave()
        {
            int[] sub_total = { 0, 0, 0 };
            int[] pas_number = { 0, 0, 0 };
            for(int i = 0; i < total_num; i++)
            {
                sub_total[0] += int.Parse(stu[i].score[0].ToString());
                if (int.Parse(stu[i].score[0].ToString()) >= 90)
                {
                    pas_number[0]++;
                }
                sub_total[1] += int.Parse(stu[i].score[1].ToString());
                if (int.Parse(stu[i].score[1].ToString()) >= 90)
                {
                    pas_number[1]++;
                }
                sub_total[2] += int.Parse(stu[i].score[2].ToString());
                if (int.Parse(stu[i].score[2].ToString()) >= 90)
                {
                    pas_number[2]++;
                }
            }

            sub_ave = new float[3];
            sub_ave[0] = (float)sub_total[0] / total_num;
            sub_ave[1] = (float)sub_total[1] / total_num;
            sub_ave[2] = (float)sub_total[2] / total_num;
            sub_pas[0] = (float)pas_number[0] / total_num;
            sub_pas[1] = (float)pas_number[1] / total_num;
            sub_pas[2] = (float)pas_number[2] / total_num;
        }

    }
}

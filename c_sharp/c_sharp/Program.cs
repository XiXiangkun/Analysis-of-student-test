using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Vevisoft.Excel.Core;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;

namespace c_sharp
{
    class Program
    {
        //导入Excel的数据
        public static string[] names;
        public static string[] subjects;
        public static object[,] tables;
        public static object[,] total;
        public static int[] stu_num = new int[8];
        public static int[] gra_total = new int[8];
        public static object[,] gra_ave;
        public static int[] y_pass_num = new int[8];
        public static int[] s_pass_num = new int[8];
        public static int[] e_pass_num = new int[8];
        public static object[,] y_pass_per;
        public static object[,] s_pass_per;
        public static object[,] e_pass_per;

        static void Main(string[] args)
        {

            IWorkbook workbook = null;  //新建IWorkbook对象
            string fileName = "..//..//..//..//grades.xlsx";
            FileStream fileStream = new FileStream(@"..//..//..//..//grades.xlsx", FileMode.Open, FileAccess.Read);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook
            }
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook
            }
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据

            //保存成绩
            row = sheet.GetRow(0);
            names = new string[sheet.LastRowNum + 1];
            subjects = new string[row.LastCellNum];
            tables = new object[sheet.LastRowNum, row.LastCellNum - 1];

            for (int i = 0; i < sheet.LastRowNum + 1; i++)  //对工作表每一行
            {
                row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        string cellValue = row.GetCell(j).ToString(); //获取i行j列数据
                        if (i == 0)
                        {
                            subjects[j] = cellValue;
                        }
                        else
                        {
                            if (j == 0)
                            {
                                names[i] = cellValue;
                            }
                            else
                            {
                                tables[i - 1, j - 1] = int.Parse(cellValue);
                            }
                        }
                    }
                }
            }
            fileStream.Close();
            workbook.Close();
            Console.WriteLine("导入数据成功！");
            Console.WriteLine("################################");
            for (int i = 1; i < names.Length; i++)
            {
                //计算班级人数
                stu_num[int.Parse(tables[i - 1, 0].ToString()) - 1]++;
            }
            

            班级[] classes = new 班级[8];
            int num_count = 0;
            for (int i = 0; i < 8; i++)
            {
                classes[i]=new 班级(i+1);
                //classes[i].tea = new 教师[3];
                classes[i].stu = new 学生[stu_num[i]];
                for (int k = 0; k < stu_num[i]; k++)
                {
                    classes[i].stu[k] = new 学生(int.Parse(names[num_count + 1].ToString()), int.Parse(tables[num_count, 0].ToString()), int.Parse(tables[num_count, 1].ToString()), int.Parse(tables[num_count, 2].ToString()), int.Parse(tables[num_count, 3].ToString()));
                    num_count++;
                }
            }
            for(int i = 0; i < 4; i++)
            {
                classes[i * 2].tea = new 教师[3];
                classes[i * 2].tea[0] = new 教师((i * 2 + 1).ToString() + (i * 2 + 2).ToString(), "语文");
                classes[i * 2].tea[1] = new 教师((i * 2 + 1).ToString() + (i * 2 + 2).ToString(), "数学");
                classes[i * 2].tea[2] = new 教师((i * 2 + 1).ToString() + (i * 2 + 2).ToString(), "英语");
                classes[i * 2 + 1].tea = classes[i * 2].tea;
            }
            //排名总成绩
            object[,] sor = new object[sheet.LastRowNum, 2];
            object[,] sor_total_ave = new object[8, 2];
            object[,] sor_sub_ave = new object[8, 4];
            object[,] sor_sub_tt_ave = new object[4, 4];

            for (int i = 0; i < 8; i++)
            {
                classes[i].total_num = stu_num[i];
                classes[i].count_ave();
                classes[i].count_sub_ave();
            }

            for(int i=0;i< sheet.LastRowNum; i++)
            {
                sor[i, 0] = names[i + 1];
                sor[i, 1] = int.Parse(tables[i, 1].ToString()) + int.Parse(tables[i, 2].ToString()) + int.Parse(tables[i, 3].ToString());
            }
            Order.Orderby(sor, new int[] { 1 }, 1);
            Console.WriteLine("总成绩前10名的同学为：");
            for(int i = 0; i < 10; i++)
            {
                for(int j=0;j<8;j++)
                {
                    classes[j].get_s(int.Parse(sor[i, 0].ToString()));
                }
            }
            Console.WriteLine("################################");

            for (int i = 0; i < 8; i++)
            {
                sor_total_ave[i, 0] = i + 1;
                sor_total_ave[i, 1] = classes[i].total_ave;
            }
            Order.Orderby(sor_total_ave, new int[] { 1 }, 1);
            Console.WriteLine("班级总平均分前三排名为：");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("班级排名：{0}；班级：{1}；班级平均成绩：{2}", i+1, sor_total_ave[i,0], sor_total_ave[i, 1]);
            }
            Console.WriteLine("################################");
            for (int i = 0; i < 8; i++)
            {
                sor_sub_ave[i, 0] = i + 1;
                sor_sub_ave[i, 1] = classes[i].sub_ave[0];
                sor_sub_ave[i, 2] = classes[i].sub_ave[1];
                sor_sub_ave[i, 3] = classes[i].sub_ave[2];
            }
            Order.Orderby(sor_sub_ave, new int[] { 1 }, 1);
            Console.WriteLine("语文单科平均分排名为：");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("排名：{0}；班级：{1}；单科平均成绩：{2}", i + 1, sor_sub_ave[i, 0], sor_sub_ave[i, 1]);
            }
            Order.Orderby(sor_sub_ave, new int[] { 2 }, 1);
            Console.WriteLine("数学单科平均分排名为：");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("排名：{0}；班级：{1}；单科平均成绩：{2}", i + 1, sor_sub_ave[i, 0], sor_sub_ave[i, 2]);
            }
            Order.Orderby(sor_sub_ave, new int[] { 3 }, 1);
            Console.WriteLine("英语单科平均分排名为：");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("排名：{0}；班级：{1}；单科平均成绩：{2}", i + 1, sor_sub_ave[i, 0], sor_sub_ave[i, 3]);
            }
            Console.WriteLine("################################");
            for (int i = 0; i < 4; i++)
            {
                sor_sub_tt_ave[i, 0] = i;
                sor_sub_tt_ave[i, 1] = ((classes[i * 2].sub_ave[0]) * (classes[i * 2].total_num) + (classes[i * 2 + 1].sub_ave[0]) * (classes[i * 2 + 1].total_num)) / ((classes[i * 2].total_num) + (classes[i * 2 + 1].total_num));
                sor_sub_tt_ave[i, 2] = ((classes[i * 2].sub_ave[1]) * (classes[i * 2].total_num) + (classes[i * 2 + 1].sub_ave[1]) * (classes[i * 2 + 1].total_num)) / ((classes[i * 2].total_num) + (classes[i * 2 + 1].total_num));
                sor_sub_tt_ave[i, 3] = ((classes[i * 2].sub_ave[2]) * (classes[i * 2].total_num) + (classes[i * 2 + 1].sub_ave[2]) * (classes[i * 2 + 1].total_num)) / ((classes[i * 2].total_num) + (classes[i * 2 + 1].total_num));
            }
            Order.Orderby(sor_sub_tt_ave, new int[] { 1 }, 1);
            Console.WriteLine("语文单科教学评分排名为：");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("排名：{0}；老师班级：{1}、{2}；单科平均成绩：{3}", i + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 2, sor_sub_tt_ave[i, 1]);
            }
            Order.Orderby(sor_sub_tt_ave, new int[] { 2 }, 1);
            Console.WriteLine("数学单科教学评分排名为：");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("排名：{0}；老师班级：{1}、{2}；单科平均成绩：{3}", i + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 2, sor_sub_tt_ave[i, 2]);
            }
            Order.Orderby(sor_sub_tt_ave, new int[] { 3 }, 1);
            Console.WriteLine("英语单科教学评分排名为：");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("排名：{0}；老师班级：{1}、{2}；单科平均成绩：{3}", i + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 1, int.Parse(sor_sub_tt_ave[i, 0].ToString()) * 2 + 2, sor_sub_tt_ave[i, 3]);
            }
            Console.WriteLine("################################");
            Console.WriteLine("各班各科及格率为：");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("班级：{0}；科目及格率，语文：{1}；数学：{2}；英语：{3}", i + 1, classes[i].sub_pas[0], classes[i].sub_pas[1], classes[i].sub_pas[2]);
            }
            Console.ReadLine();
        }
    }
    
}

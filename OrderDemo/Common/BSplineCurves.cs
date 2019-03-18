using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderDemo.Common
{
    public class BSplineCurves
    {
        // 从 C++ DLL 中导出B样条处理函数
        [DllImport(@"C:\AzLathe.dll", EntryPoint = "DensifyBSpline", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr DensifyBSpline(string sDataPnts, double precision);


        // 接口函数，使用该函数即可将B样条数据密化
        // 参数说明：
        // [dataPnts]   3~5个B样条型值点（待拟合的点）
        // [precision]  拟合精度，如 precision = 0.01， 返回100个点；precision = 0.001，则返回1000个点
        // 函数返回：密化的B样条数据点
        public static List<Point3D> DensifyCurve(List<Point3D> dataPnts, double precision)
        {
            // 将传入点数据 转化成 字符串
            string sDataPnts = string.Empty;
            for (int i = 0; i < dataPnts.Count; i++)
            {
                string str = string.Format("{0},{1},{2}", dataPnts[i].x, dataPnts[i].y, dataPnts[i].z);
                if (i != dataPnts.Count - 1)
                {
                    str += ",";
                }
                sDataPnts += str;
            }

            IntPtr ptr = DensifyBSpline(sDataPnts, precision);
            string retStr = Marshal.PtrToStringUni(ptr);

            // 将输出的点数据转化成字符串
            string[] strs = retStr.Split(',');
            int cnt = strs.Length;
            if (cnt % 3 != 0)
            {
                List<Point3D> retPnts = new List<Point3D>();
                double x, y, z;
                double x1, y1, z1;
                for (int i = 0; i < cnt - 16; i = i + 3)
                {
                    x = double.Parse(strs[i]);
                    y = double.Parse(strs[i + 1]);
                    z = double.Parse(strs[i + 2]);
                    retPnts.Add(new Point3D(x, y, z));
                }
                x1 = double.Parse(strs[300]);
                y1 = double.Parse(strs[301]);
                z1 = 0.000;
                retPnts.Add(new Point3D(x1, y1, z1));
                return retPnts;
                //  throw new Exception("B样条数据密化似乎出了一点问题");
            }
            else
            {
                List<Point3D> retPnts = new List<Point3D>();
                double x, y, z;
                for (int i = 0; i < cnt; i = i + 3)
                {
                    x = double.Parse(strs[i]);
                    y = double.Parse(strs[i + 1]);
                    z = 0.000;//直接用0.000赋值给z感觉更好一点
                    //z = double.Parse(strs[i+2]);//之前用的程序语句，
                    retPnts.Add(new Point3D(x, y, z));
                }
                return retPnts;
            }
        }
    }

    public class Point3D
    {
        public double x;
        public double y;
        public double z;
        public Point3D(double ix, double iy, double iz)
        {
            x = ix;
            y = iy;
            z = iz;
        }
    }
}

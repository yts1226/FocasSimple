using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FocasSimple2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CmdConnect(object sender, RoutedEventArgs e)
        {
            ushort h;
            short ret;
            string strVal;

            Focas1.ODBAXIS CncAxis = new Focas1.ODBAXIS();
            Focas1.ODBM MarcoVal = new Focas1.ODBM();
            Focas1.IODBPMC0 bytPMC = new Focas1.IODBPMC0();

            //tbIpAddress.Text="192.168.12.149";

            // 建立连接
            ret = Focas1.cnc_allclibhndl3(tbIpAddress.Text, ushort.Parse(tbPort.Text), 3, out h);
            
            if (ret == Focas1.EW_OK)
            {
                // 读取绝对位置坐标
                Focas1.cnc_absolute2(h, -1, 4 + 4 * Focas1.MAX_AXIS, CncAxis);
                strVal = string.Format("{0:#########}", Math.Abs(CncAxis.data[0]));
                strVal = strVal.Insert(strVal.Length-3, ".");
                tbAbsolutX.Text = strVal;
                
                // 读取机械位置坐标
                Focas1.cnc_machine(h, -1, 4 + 4 * Focas1.MAX_AXIS, CncAxis);
                strVal = string.Format("{0:#########}", Math.Abs(CncAxis.data[0]));
                tbMachineX.Text = strVal.Insert(strVal.Length - 3, ".");

                // 读取用户宏变量 #500 数值
                Focas1.cnc_rdmacro(h, 500, 10, MarcoVal);
                strVal = string.Format("{0:d9}", Math.Abs(MarcoVal.mcr_val));
                if (0 < MarcoVal.dec_val) strVal = strVal.Insert(9 - MarcoVal.dec_val, ".");
                if (MarcoVal.mcr_val < 0) strVal = "-" + strVal;
                tbVariable500.Text = strVal;

                // 读取PMC D200 数值
                Focas1.pmc_rdpmcrng(h, 9, 0, 200, 200, 9, bytPMC);
                tbPmcD0200.Text = bytPMC.cdata[0].ToString();

                // 释放 library handle
                Focas1.cnc_freelibhndl(h);
            }
            else
            {
                MessageBox.Show("ret = "+ret.ToString());
            }
        }

        private void CmdExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

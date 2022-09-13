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
        ushort h;
        short ret;
        int MacroVal;
        short MacroDec;

        Focas1.ODBAXIS CncAxis = new Focas1.ODBAXIS();
        Focas1.ODBM CncMacro = new Focas1.ODBM();
        Focas1.IODBPMC0 bytPMC = new Focas1.IODBPMC0();

        public MainWindow()
        {
            InitializeComponent();
            tbIpAddress.Text = "192.168.12.149";
        }

        private void CmdRead(object sender, RoutedEventArgs e)
        {
            // 建立连接，IP地址，端口默认8193，连接延时3s
            // 获得library handle
            ret = Focas1.cnc_allclibhndl3(tbIpAddress.Text, ushort.Parse(tbPort.Text), 3, out h);
            
            if (ret == Focas1.EW_OK)
            {
                // 读取绝对位置坐标
                Focas1.cnc_absolute2(h, -1, 4 + 4 * Focas1.MAX_AXIS, CncAxis);
                // 显示，暂时默认三位小数
                tbAbsolutX.Text = String.Format("{0:#####0.000}", (double)CncAxis.data[0] / 1000);

                // 读取机械位置坐标
                Focas1.cnc_machine(h, -1, 4 + 4 * Focas1.MAX_AXIS, CncAxis);
                // 显示，暂时默认三位小数
                tbMachineX.Text = String.Format("{0:#####0.000}", (double)CncAxis.data[0] / 1000);

                // 读取用户宏变量 #500 数值，保留4位小数
                Focas1.cnc_rdmacro(h, 500, 10, CncMacro);
                tbVariable500.Text = String.Format("{0:########0.0000}", CncMacro.mcr_val * Math.Pow(0.1, CncMacro.dec_val));

                // 读取PMC D200 数值
                Focas1.pmc_rdpmcrng(h, 9, 0, 200, 200, 9, bytPMC);
                tbPmcD0200.Text = bytPMC.cdata[0].ToString();

                // 释放 library handle
                Focas1.cnc_freelibhndl(h);
            }
            else
            {
                // 检查返回值异常
                MessageBox.Show("ret = "+ret.ToString());
            }
        }

        private void CmdWrite(object sender, RoutedEventArgs e)
        {
            // 建立连接，IP地址，端口默认8193，连接延时3s
            ret = Focas1.cnc_allclibhndl3(tbIpAddress.Text, ushort.Parse(tbPort.Text), 3, out h);

            if (ret == Focas1.EW_OK)
            {
                // 写入用户宏变量 #500 数值
                // 此处限定4位小数，四舍五入
                MacroVal = (int)Math.Round(double.Parse(tbVariable500.Text) * 10000);
                MacroDec = 4;
                Focas1.cnc_wrmacro(h, 500, 10, MacroVal, MacroDec);

                // 写入PMC D200 数值
                bytPMC.cdata[0] = Convert.ToByte(tbPmcD0200.Text);
                bytPMC.type_a = 9;
                bytPMC.type_d = 0;
                bytPMC.datano_s = 200;
                bytPMC.datano_e = 200;
                ret = Focas1.pmc_wrpmcrng(h, 9, bytPMC);

                // 释放 library handle
                Focas1.cnc_freelibhndl(h);
            }
            else
            {
                MessageBox.Show("ret = " + ret.ToString());
            }

        }

        private void CmdExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

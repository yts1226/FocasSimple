using System;
using System.Collections.Generic;
using System.Linq;
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

            Focas1.ODBST buf = new Focas1.ODBST();
            //Focas1.ODBAXIS buf2 = new Focas1.ODBAXIS();

            ret = Focas1.cnc_allclibhndl3("192.168.12.149", 8193, 3, out h);

            if (ret == Focas1.EW_OK)
            {
                Focas1.cnc_statinfo(h, buf);
                //Focas1.cnc_absolute(h, -1, 4 + 4 * Focas1.MAX_AXIS, buf2);
                //Console.WriteLine("1:{0,8}\n2:{1,8}\n3:{2,8}", buf2.data[0], buf2.data[1], buf2.data[2]);
                Focas1.cnc_freelibhndl(h);
            }
            else
            {
                Console.WriteLine("ERROR!({0})", ret);
            }
        }

        private void CmdExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

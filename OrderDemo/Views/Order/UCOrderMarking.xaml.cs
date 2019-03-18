using OrderDemo.ViewModels;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThoughtWorks.QRCode.Codec;

namespace OrderDemo.Views
{
    /// <summary>
    /// UCOrderMarking.xaml 的交互逻辑
    /// </summary>
    public partial class UCOrderMarking : UserControl
    {
        public UCOrderMarking()
        {
            InitializeComponent();
            this.DataContext = UserViewModel.InstanceUserViewModel;
        }
        private void txtPrintContentQRCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThoughtWorks.QRCode.Codec.QRCodeEncoder encoder = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
            //encoder.QRCodeBackgroundColor = System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);//白色
            encoder.QRCodeBackgroundColor = System.Drawing.Color.FromArgb(0x00, 0xBF, 0xBF, 0xBF);//和背景颜色一样的透明色
            encoder.QRCodeForegroundColor = System.Drawing.Color.FromArgb(0xFF, 0x00, 0x00, 0x00);//黑色
            //设置编码格式
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方法
            //设置规模和版本
            encoder.QRCodeScale = 9;//大小
            encoder.QRCodeVersion = 4;//版本
            //设置错误校验（错误更正）的级别
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Bitmap bp = encoder.Encode(tbxPrintContent.Text.ToString(), Encoding.GetEncoding("GB2312"));
            BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(bp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            imagePrintContentQRCode.Source = bs;
        }
    }
}

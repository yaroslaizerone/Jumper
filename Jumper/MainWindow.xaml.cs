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

namespace Jumper
{
    public class Helper
    {
        public static Entities ent;
        internal static bool flag = false;
        internal static int prioritet = 0;

        public static Entities GetContext()
        {
            if (ent == null)
            {
                ent = new Entities();
            }
            return ent;
        }
    }
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame.Content = new PageOne(frame);
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                PageOne pg = (PageOne)e.Content;
                pg.Load();
            }
            catch { };
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO.Ports;


namespace Attenuator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AttenuatorControl attenuatorControl;
        CheckBox[] cbAtt1;
        CheckBox[] cbAtt2;
        CheckBox[] cbAtt3;
        CheckBox[] cbAtt4;

        TextBox[] tbAttValues;
        
        Button[] btnUp;
        Button[] btnDown;

        bool isTbHandle;

        public MainWindow()
        {
            attenuatorControl = new AttenuatorControl();
            InitializeComponent();
            InitControlArrayAtt();
            InitEvents();
        }



        private void InitEvents()
        {
            for (int i = 0; i < 6; i++)
            {
                cbAtt1[i].Click += CbHandler;
                cbAtt2[i].Click += CbHandler;
                cbAtt3[i].Click += CbHandler;
                cbAtt4[i].Click += CbHandler;
            }

            for (int i = 0; i < 4; i++)
            {
                btnUp[i].Click += btnHandler;
                btnDown[i].Click += btnHandler;
                btnUp[i].MouseWheel += btnHandlerScroll;
                btnDown[i].MouseWheel += btnHandlerScroll;
            }

            cmbBxPort.DropDownOpened += CmbBxPort_DropDownOpened;
            cmbBxPort.SelectionChanged += CmbBxPort_SelectionChanged;
            tglBtnOpenPort.Click += TglBtnOpenPort_Click;
        }

        private void TglBtnOpenPort_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Cursor = Cursors.Wait;
                bool isConnect = attenuatorControl.Connect();
                if (isConnect)
                {
                    cmbBxPort.IsEditable = false;
                    tglBtnOpenPort.Content = "Закрыть";
                    tglBtnOpenPort.IsChecked = true;
                }
                else
                {
                    //SetCheckIcon(statusConnection);
                    tglBtnOpenPort.Content = "Открыть";
                    tglBtnOpenPort.IsChecked = false;
                }
                Cursor = default;
            }
            catch (Exception)
            {
                //SetCanselIcon(statusConnection);
                tglBtnOpenPort.Content = "Открыть";
                tglBtnOpenPort.IsChecked = false;
                Cursor = default;
            }
        }

        private void CmbBxPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBxPort.SelectedItem != null)
            {
                attenuatorControl.NameComPort = Convert.ToString(cmbBxPort.SelectedItem);
            }
        }

        private void CmbBxPort_DropDownOpened(object sender, EventArgs e)
        {
            string[] ports = attenuatorControl.SearchComPorts();
            cmbBxPort.Items.Clear();
            foreach (var item in ports)
            {
                cmbBxPort.Items.Add(item);
            }
        }

        private void InitControlArrayAtt()
        {
            cbAtt1 = new CheckBox[6];
            cbAtt2 = new CheckBox[6];
            cbAtt3 = new CheckBox[6];
            cbAtt4 = new CheckBox[6];

            cbAtt1[0] = checkBoxAtt1bit0;
            cbAtt1[1] = checkBoxAtt1bit1;
            cbAtt1[2] = checkBoxAtt1bit2;
            cbAtt1[3] = checkBoxAtt1bit3;
            cbAtt1[4] = checkBoxAtt1bit4;
            cbAtt1[5] = checkBoxAtt1bit5;

            cbAtt2[0] = checkBoxAtt2bit0;
            cbAtt2[1] = checkBoxAtt2bit1;
            cbAtt2[2] = checkBoxAtt2bit2;
            cbAtt2[3] = checkBoxAtt2bit3;
            cbAtt2[4] = checkBoxAtt2bit4;
            cbAtt2[5] = checkBoxAtt2bit5;

            cbAtt3[0] = checkBoxAtt3bit0;
            cbAtt3[1] = checkBoxAtt3bit1;
            cbAtt3[2] = checkBoxAtt3bit2;
            cbAtt3[3] = checkBoxAtt3bit3;
            cbAtt3[4] = checkBoxAtt3bit4;
            cbAtt3[5] = checkBoxAtt3bit5;

            cbAtt4[0] = checkBoxAtt4bit0;
            cbAtt4[1] = checkBoxAtt4bit1;
            cbAtt4[2] = checkBoxAtt4bit2;
            cbAtt4[3] = checkBoxAtt4bit3;
            cbAtt4[4] = checkBoxAtt4bit4;
            cbAtt4[5] = checkBoxAtt4bit5;

            for (int j = 0; j < 6; j++)
            {
                cbAtt1[j].Tag = new TwoInt(0, j);
            }

            for (int j = 0; j < 6; j++)
            {
                cbAtt2[j].Tag = new TwoInt(1, j);
            }

            for (int j = 0; j < 6; j++)
            {
                cbAtt3[j].Tag = new TwoInt(2, j);
            }

            for (int j = 0; j < 6; j++)
            {
                cbAtt4[j].Tag = new TwoInt(3, j);
            }


            tbAttValues = new TextBox[4];
            tbAttValues[0] = tbAttenuator1UpDown;
            tbAttValues[1] = tbAttenuator2UpDown;
            tbAttValues[2] = tbAttenuator3UpDown;
            tbAttValues[3] = tbAttenuator4UpDown;

            for (int i = 0; i < 4; i++)
            {
                tbAttValues[i].Tag = i;
            }

            
            btnUp = new Button[4];
            btnDown = new Button[4];
           
            btnUp[0] = btnUpTextBoxAtt1;
            btnUp[1] = btnUpTextBoxAtt2;
            btnUp[2] = btnUpTextBoxAtt3;
            btnUp[3] = btnUpTextBoxAtt4;

            btnDown[0] = btnDownTextBoxAtt1;
            btnDown[1] = btnDownTextBoxAtt2;
            btnDown[2] = btnDownTextBoxAtt3;
            btnDown[3] = btnDownTextBoxAtt4;



            //-------------------------------------------------------------------------------тут
            for (int j = 0; j < 4; j++)
            {
                btnUp[j].Tag = new TwoInt(1, j);
            }

            for (int j = 0; j < 4; j++)
            {
                btnDown[j].Tag = new TwoInt(-1, j);
            }

        }


        private void CbHandler(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            TwoInt t = cb.Tag as TwoInt;

            CheckBox[] cbArray;
            if (t.a == 0)
            {
                cbArray = cbAtt1;
            }
            else if (t.a == 1)
            {
                cbArray = cbAtt2;
            }
            else if (t.a == 2)
            {
                cbArray = cbAtt3;
            }
            else 
            {
                cbArray = cbAtt4;
            }


            int valueAtt = Convert.ToByte(
                           Convert.ToInt32(cbArray[0].IsChecked) |                             //if (checkBoxAtt1bit0.IsChecked == true) newValue += 1;
                           Convert.ToInt32(cbArray[1].IsChecked) << 1 |                        //if (checkBoxAtt1bit1.IsChecked == true) newValue += 2;
                           Convert.ToInt32(cbArray[2].IsChecked) << 2 |                        //if (checkBoxAtt1bit2.IsChecked == true) newValue += 4;
                           Convert.ToInt32(cbArray[3].IsChecked) << 3 |                        //if (checkBoxAtt1bit3.IsChecked == true) newValue += 8;
                           Convert.ToInt32(cbArray[4].IsChecked) << 4 |                        //if (checkBoxAtt1bit4.IsChecked == true) newValue += 16;
                           Convert.ToInt32(cbArray[5].IsChecked) << 5);                        //if (checkBoxAtt1bit5.IsChecked == true) newValue += 32;
            
            tbAttValues[t.a].Text = Convert.ToString(Convert.ToDouble(valueAtt)/2);

            double valueAllAtt = 0;
            for (int i = 0; i < tbAttValues.Length; i++)
            {
                valueAllAtt += Convert.ToDouble(tbAttValues[i].Text);
            }
            tbAttenuatorAllUpDown.Text = Convert.ToString(valueAllAtt);
        }

        private void btnHandler(object sender, RoutedEventArgs e)
        {
            Button tb = sender as Button;
            CheckBox cb = sender as CheckBox;

            TwoInt t = tb.Tag as TwoInt;

            Button[] btnArray;
            if (t.a == 1)
            {
                btnArray = btnUp;
            }
            else if (t.a == -1)
            {
                btnArray = btnDown;
            }

            CheckBox[] cbArray;
            if (t.b == 0)
            {
                cbArray = cbAtt1;
            }
            else if (t.b == 1)
            {
                cbArray = cbAtt2;
            }
            else if (t.b == 2)
            {
                cbArray = cbAtt3;
            }
            else
            {
                cbArray = cbAtt4;
            }


            double valueAtt = Convert.ToDouble(tbAttValues[t.b].Text);
            valueAtt += 0.5*t.a;
            if (valueAtt>=0 && valueAtt<=31.5)
            {
                tbAttValues[t.b].Text = Convert.ToString(valueAtt);

                uint value = (uint)(valueAtt * 2);
               
                for (int i = 0; i < cbArray.Length; i++)
                {
                    cbArray[i].IsChecked = BitOperator.BitSetted(value, i);
                }

                double valueAllAtt = 0;
                for (int i = 0; i < tbAttValues.Length; i++)
                {
                    valueAllAtt += Convert.ToDouble(tbAttValues[i].Text);
                }
                tbAttenuatorAllUpDown.Text = Convert.ToString(valueAllAtt);
            }
        }


        private void btnHandlerScroll(object sender, MouseWheelEventArgs e)
        {
            Button tb = sender as Button;
            CheckBox cb = sender as CheckBox;

            TwoInt t = tb.Tag as TwoInt;

            Button[] btnArray;
            if (t.a == 1)
            {
                btnArray = btnUp;
            }
            else if (t.a == -1)
            {
                btnArray = btnDown;
            }

            CheckBox[] cbArray;
            if (t.b == 0)
            {
                cbArray = cbAtt1;
            }
            else if (t.b == 1)
            {
                cbArray = cbAtt2;
            }
            else if (t.b == 2)
            {
                cbArray = cbAtt3;
            }
            else
            {
                cbArray = cbAtt4;
            }


            double valueAtt = Convert.ToDouble(tbAttValues[t.b].Text);
            if (e.Delta > 0)
            {
                valueAtt += 0.5 ;
            }
            else 
            {
                valueAtt -= 0.5;
            }

            if (valueAtt >= 0 && valueAtt <= 31.5)
            {
                tbAttValues[t.b].Text = Convert.ToString(valueAtt);

                uint value = (uint)(valueAtt * 2);

                for (int i = 0; i < cbArray.Length; i++)
                {
                    cbArray[i].IsChecked = BitOperator.BitSetted(value, i);
                }

                double valueAllAtt = 0;
                
                for (int i = 0; i < tbAttValues.Length; i++)
                {
                    valueAllAtt += Convert.ToDouble(tbAttValues[i].Text);
                }
                tbAttenuatorAllUpDown.Text = Convert.ToString(valueAllAtt);
            }
        }






    }


    public class TwoInt
    {
        public int a;
        public int b;

        public TwoInt(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }
}

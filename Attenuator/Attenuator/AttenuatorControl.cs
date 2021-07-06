using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attenuator
{
    public class AttenuatorControl
    {
        SerialPort _comPort;
        private bool _isConnected;
        private string _portName;

        public AttenuatorControl()
        {
            _comPort = null;
            _isConnected = false;
        }

        public bool IsConnected => _isConnected;
        public string NameComPort
        {
            get { return _portName; }
            set
            {
                if (!IsConnected)
                {
                    _portName = value;
                }
            }
        }


        public string[] SearchComPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Connect()
        {
            if (_comPort == null)
            {
                _comPort = new SerialPort(_portName, 115200, Parity.None, 8, StopBits.One);
                _comPort.WriteTimeout = 3000;
                _comPort.ReadTimeout = 3000;
            }
            else
                Disconnect();
            try
            {
                _comPort.PortName = _portName;
                _comPort.Open();

                _isConnected = TestConnection();
            }
            catch
            {
                _isConnected = false;
            }
            return _isConnected;
        }


        public void Disconnect()
        {
            if (_comPort != null)
            {
                _comPort.Close();
                _isConnected = false;
            }
        }



        private bool TestConnection()
        {
            int byteReadNumber = 1;
            byte[] bytesToWrite = { 200 };
            byte[] bytesToRead = new byte[byteReadNumber];

            try
            {
                _comPort.Write(bytesToWrite, 0, 1);
                _comPort.Read(bytesToRead, 0, 1);
            }
            catch (Exception)
            {
                return false;
            }
            


            if (!(bytesToWrite[0] == bytesToRead[0]))
            {
                return false;
            }

            return true;
        }
    }
}

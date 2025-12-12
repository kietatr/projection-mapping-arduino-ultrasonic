using UnityEngine;
using System.IO.Ports;

// To use this, need to:
// - Go to Unity project's Edit > Project Settings > Player > Other Settings 
// - Find "Api Compatibility Level" and set to ".NET Framework"
// - Go to Unity project's Edit > Preferences > External Tools
// - Click on "Regenerate Project Files"
public class ArduinoSerialConnector : MonoBehaviour
{
    [SerializeField] string m_SerialPortName = "/dev/ttyACM0";
    [SerializeField] int m_BaudRate = 115200;

    SerialPort m_SerialPort;

    void Awake()
    {
        m_SerialPort = new SerialPort(m_SerialPortName, m_BaudRate);
    }

    void Start()
    {
        m_SerialPort.Open();
        // m_SerialPort.ReadTimeout = 100;
    }

    void OnDisable()
    {
        m_SerialPort.Close();
    }

    void OnApplicationQuit()
    {
        m_SerialPort.Close();
    }

    public string ReadData()
    {
        return m_SerialPort.ReadLine();
    }

    public void WriteData(string data)
    {
        m_SerialPort.WriteLine(data);
    }
}

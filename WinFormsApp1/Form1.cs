namespace WinFormsApp1
{
    
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
        UserControl1 userControl1;
        TcpServer tcpServer;

      
        private void Form1_Load(object sender, EventArgs e)
        {
            userControl1 = new UserControl1();
            panel1.Controls.Add(userControl1);
            panel1.Show();
            tcpServer = new TcpServer();
            tcpServer.StartServer();
            tcpServer.ReceiveMes += Setvalue;
            tcpServer.ReceiveMes += SetLab2Value;
        }

        public void Setvalue(string text)
        {
            if (this.InvokeRequired)
            {
                Change _myInvoke = new Change(Setvalue);
                this.Invoke(_myInvoke, new object[] { text });
            }
            else
            {
                label1.Text = text;
            }
        }


        public void SetLab2Value(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(SetLab2Value, text );
            }
            else
            {
                label2.Text = text;
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            tcpServer.SendMessage("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpServer.SendMessage("2");
        }

        
    }
}
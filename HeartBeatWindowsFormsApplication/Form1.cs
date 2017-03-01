using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeartBeatWindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public StringBuilder outputLabel;

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            outputLabel = new StringBuilder();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                Thread.Sleep(5000);
                Application.Exit();
            }
        }

       

        private int _Ping()
        {
            try
            {
                var urlList = new List<string>()
                {
                    "http://portal-demo.travelogyclub.com/India",
                    "http://portal-demo.travelogyclub.com/",
                    "http://portal-demo.travelogyclub.com/Circuit/Tanzania",
                    "http://portal-demo.travelogyclub.com/Circuit/Russia"
                };

                int x = new Random().Next(4);
                string url = urlList[x];
                string msg;
                msg = string.Format("Heartbeat log ... pinging URL: {0}\n", url);
                outputLabel.Append(msg);
                WebRequest myReq = WebRequest.Create(url);
                myReq.Timeout = 5000;
                WebResponse response = myReq.GetResponse();
                msg = string.Format("Heartbeat log TIME: {0} RESPONSE: {1}\n", DateTime.Now, ((HttpWebResponse)response).StatusDescription);
                outputLabel.Append(msg);

                return 0;
            }
            catch (Exception ex)
            {
                outputLabel.AppendFormat("Heartbeat log ERROR: Time: {0} -- Error Message: {1}\n", DateTime.Now, ex.Message);
                return -1;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while(1 == 1)
            {
                outputLabel.Clear();
                outputLabel.Append("Starting to ping ... \n");                

                for (int i = 0; i < 10; i++)
                {
                    _Ping();
                    worker.ReportProgress(i * 9);
                    Thread.Sleep(30000);
                }
            }
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelOutput.Text = outputLabel.ToString();
            this.Invalidate();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invalidate();
        }
    }
}

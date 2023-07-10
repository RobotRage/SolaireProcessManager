using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SolaireProcessManager
{
    public partial class SolaireProcessManager : Form
    {
        public struct checkboxData
        {
            public int PID;
            public string name;
            public string cert;
            public bool selected;
            public Process proc;
        }

        checkboxData[] processesStruct;

        Process[] processes = Process.GetProcesses();

        Process[] staticProcesses = Process.GetProcesses();

        string rtbText;

        public SolaireProcessManager()
        {
            InitializeComponent();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {

            update();
           // List<int> killed =  new List<int>();
            for (int i = 0; i < processesStruct.Length; i++)
            {
                if (processesStruct[i].selected)
                {
                    processesStruct[i].proc.Kill();
                    timer1.Start();
                }
            }
            
            reloadList();

            /*
            rtbText = "";

            bool failed = false;
            for (int i = 0; i < processesStruct.Length; i++)
            {
                if (killed.Contains(processesStruct[i].PID))
                {
                    failed = true;
                    rtbText += "FAILED : Failed to close " + processesStruct[i].name + "\n";
                }                
            }
            if(!failed)
            {
                rtbText += "SUCCESS\n";
            }
            rtbOutput.Text = rtbText;
            */
        }

        int savedScroll = 0;

        void reloadList()
        {
            savedScroll = clbProcesses.TopIndex;
            clbProcesses.Items.Clear();
            processes = Process.GetProcesses();
            staticProcesses = Process.GetProcesses();
            int index = 0;
            //init struct
            processesStruct = new checkboxData[processes.Count()];


            foreach (Process process in processes.OrderBy(m=>m.ProcessName))
            {
                processesStruct[index].PID = process.Id;
                processesStruct[index].name = process.ProcessName;
                processesStruct[index].selected = false;
                processesStruct[index].proc = process;

                clbProcesses.Items.Add(process.ProcessName);

                index++;
            }
            
            clbProcesses.TopIndex = savedScroll;
        }

        //on start
        private void SolaireProcessManager_Load(object sender, EventArgs e)
        {
            reloadList();
            timer2.Start();
        }

        void update()
        {
            for (int i = 0; i < processesStruct.Length; i++)
            {
                if (clbProcesses.GetItemCheckState(i) == CheckState.Checked)
                {
                    processesStruct[i].selected = true;
                }
                else
                {
                    processesStruct[i].selected = false;
                }
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            update();
            rtbText = "";
            update();
            for (int i=0; i < processesStruct.Length; i++)
            {
                if (processesStruct[i].selected)
                {
                    try
                    {
                        rtbText += "Name: " + processesStruct[i].name + " " + "PID: " + processesStruct[i].PID + " PATH: " + processesStruct[i].proc.MainModule.FileName + "\n";
                    }
                    catch
                    {

                    }

                }
            }
            rtbOutput.Text = rtbText;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            for (int i=0; i < processesStruct.Length; i++)
            {
                string templower = processesStruct[i].name.ToLower();
                if (templower.Contains(txtSearch.Text.ToLower()))
                {
                    clbProcesses.TopIndex = i;
                }
            }           
        }

        private void tslRefresh_Click(object sender, EventArgs e)
        {
            reloadList();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //reloadList();
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Process[] checkProcess = Process.GetProcesses();
            //staticProcesses


            if (checkProcess.Count() != staticProcesses.Count())
            {
                 //reloadList();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            update();

            for (int i = 0; i < processesStruct.Length; i++)
            {
                if (processesStruct[i].selected)
                {
                    try
                    {
                        string path = processesStruct[i].proc.MainModule.FileName;
                        processesStruct[i].proc.Kill();
                        Process.Start(path);
                    }
                    catch
                    {

                    }

                }
            }

            reloadList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;

namespace AutoF2
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const int VK_F2 = 0x71;

        private System.Timers.Timer autoTimer = new System.Timers.Timer(1000);
        private IntPtr targetHwnd = IntPtr.Zero;

        public MainForm()
        {
            InitializeComponent();
            LoadWindowList();
            autoTimer.Elapsed += AutoTimer_Elapsed;
        }

        private void AutoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (targetHwnd != IntPtr.Zero)
            {
                PostMessage(targetHwnd, WM_KEYDOWN, (IntPtr)VK_F2, IntPtr.Zero);
                PostMessage(targetHwnd, WM_KEYUP, (IntPtr)VK_F2, IntPtr.Zero);
            }
        }

        private void LoadWindowList()
        {
            comboWindows.Items.Clear();
            foreach (Process p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    comboWindows.Items.Add(p.MainWindowTitle);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (comboWindows.SelectedItem != null)
            {
                string windowTitle = comboWindows.SelectedItem.ToString();
                targetHwnd = FindWindow(null, windowTitle);
                autoTimer.Start();
                lblStatus.Text = "Đang gửi F2...";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            autoTimer.Stop();
            lblStatus.Text = "Đã dừng.";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWindowList();
        }
    }
}
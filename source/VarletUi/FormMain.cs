﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using Variety;
using static System.String;
using static System.Windows.Forms.Application;
using Timer = System.Threading.Timer;

namespace VarletUi
{
    public partial class FormMain : Form
    {
        private static bool RunMinimized { get; set; }

        public delegate void InvokeDelegate();

        public FormMain(string parameter = "normal")
        {
            InitializeComponent();
            if (!File.Exists(Globals.AppConfigFile))  {
                Config.Initialize(Globals.AppConfigFile);
            }
            if (parameter != "/minimized") return;
            RunMinimized = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitializeWindow();
            CheckAvailablePhp();
            CheckServiceStatus();
            if (!RunMinimized) return;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            BeginInvoke(new MethodInvoker(Close));
            RunMinimized = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason.Equals(CloseReason.UserClosing)) {
                base.OnFormClosing(e);
                e.Cancel = true;
                (new TrayContext()).ShowTrayIconNotification();
                Hide();
            } else  {
                ExitThread();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            (new TrayContext()).ExitApplication();
        }

        private void InitializeWindow()
        {
            Text = "Varlet v" + Globals.AppVersion + " build " + Globals.AppBuildNumber;
            btnServices.Text = "Start Services";
            comboPhpVersion.Enabled = true;
            lblReloadHttpd.Enabled = false;
            lblReloadSmtp.Enabled = false;
        }

        private void CheckServiceStatus() {
            btnServices.Enabled = true;
            if (Services.IsInstalled((Globals.HttpServiceName))) {
                pictStatusHttpd.BackColor = Color.Red;
                if (Services.IsRunning(Globals.HttpServiceName)) {
                    pictStatusHttpd.BackColor = Color.Green;
                    btnServices.Text = "Stop Services";
                    comboPhpVersion.Enabled = false;
                    lblReloadHttpd.Enabled = true;
                    Services.IsHttpServiceRun = true;
                }
            }
            if (Services.IsInstalled((Globals.SmtpServiceName))) {
                pictStatusSmtp.BackColor = Color.Red;
                if (Services.IsRunning(Globals.SmtpServiceName)) {
                    pictStatusSmtp.BackColor = Color.Green;
                    lblReloadSmtp.Enabled = true;
                    btnServices.Text = "Stop Services";
                    Services.IsSmtpServiceRun = true;
                }
            }
        }

        private void SwitchServiceStatus()
        {
            if ((Services.IsHttpServiceRun == true) || (Services.IsSmtpServiceRun == true))
            {
                btnServices.Text = "Stopping Services";
                btnServices.Enabled = false;
            } else  {
                btnServices.Text = "Starting Services";
                btnServices.Enabled = false;
            }
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            SwitchServiceStatus();
            switch (btnServices.Text)
            {
                case "Stop Services":
                    Services.Stop(Globals.HttpServiceName);
                    Services.Stop(Globals.SmtpServiceName);
                    Services.IsHttpServiceRun = false;
                    Services.IsSmtpServiceRun = false;
                    break;
                case "Start Services":
                    Services.Start(Globals.HttpServiceName);
                    Services.Start(Globals.SmtpServiceName);
                    Services.IsHttpServiceRun = false;
                    Services.IsSmtpServiceRun = false;
                    break;
            }
            Refresh();
        }

        private void btnTerminal_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Common.DirProgramFiles(@"\PowerShell"))) {
                var proc = new Process {StartInfo = {
                    FileName = "pwsh.exe",
                    Arguments = "-NoLogo -WorkingDirectory \"" + Globals.WwwDirectory + "\"",
                    UseShellExecute = false
                }};
                proc.Start();
            } else  {
                var proc = new Process {StartInfo = {
                    FileName = "cmd.exe",
                    Arguments = "/k \"cd /d " + Globals.WwwDirectory + "\"",
                    UseShellExecute = false
                }};
                proc.Start();
            }
        }

        private void CheckAvailablePhp()
        {
            var pkgPhp = Common.GetAppPath() + @"\pkg\php";
            if (!Directory.Exists(pkgPhp)) return;
            foreach (var t in Directory.GetDirectories(pkgPhp))  {
                comboPhpVersion.Items.Add(Path.GetFileName(t));
            }
            comboPhpVersion.SelectedIndex = !IsNullOrEmpty(Config.Get("SelectedPhpVersion")) ?
                comboPhpVersion.FindStringExact(Config.Get("SelectedPhpVersion")) : 0;
        }

        private void lblAbout_Click(object sender, EventArgs e)
        {
            Common.OpenUrl("https://github.com/riipandi/varlet");
        }

        private void lblHostFile_Click(object sender, EventArgs e)
        {
            try {
                var file = Environment.SystemDirectory + @"\drivers\etc\hosts";
                Common.OpenWithNotepad(file, true);
            } catch (FormatException) {
                // do something here
            }
        }

        public void lblSettings_Click(object sender, EventArgs e)
        {
            // new FormSettings().ShowDialog();
            MessageBox.Show("Not yet implemented!");
        }

        private void lblLogfileHttpd_Click(object sender, EventArgs e)
        {
            var file = Common.GetAppPath() + @"\tmp\httpd_error.log";
            if (!File.Exists(file))  {
                MessageBox.Show("File "+file+" not found!");
            } else  {
                Common.OpenWithNotepad(file);
            }
        }

        private void lblLogfileSmtp_Click(object sender, EventArgs e)
        {
            var file = Common.GetAppPath() + @"\tmp\mailhogservice.err.log";
            if (!File.Exists(file))  {
                MessageBox.Show("File "+file+" not found!");
            } else  {
                Common.OpenWithNotepad(file);
            }
        }

        private void lblPhpIni_Click(object sender, EventArgs e)
        {
            var file = Common.GetAppPath() + @"\pkg\php\"+comboPhpVersion.Text+@"\php.ini";
            if (!File.Exists(file))  {
                MessageBox.Show("File "+file+" not found!");
            } else  {
                Common.OpenWithNotepad(file);
            }
        }

        private void lblReloadHttpd_Click(object sender, EventArgs e)
        {
            Services.Restart(Globals.HttpServiceName);
        }

        private void lblReloadSmtp_Click(object sender, EventArgs e)
        {
            Services.Restart(Globals.SmtpServiceName);
        }

        private void comboPhpVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Set("SelectedPhpVersion", comboPhpVersion.Text);
        }
    }
}

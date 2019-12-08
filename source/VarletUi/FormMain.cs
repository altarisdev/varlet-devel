﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Variety;
using static System.String;
using static System.Windows.Forms.Application;

namespace VarletUi
{
    public partial class FormMain : Form
    {
        private static bool RunMinimized { get; set; }

        public FormMain(string parameter = "normal")
        {
            InitializeComponent();
            Config.Initialize();
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

        private void InitializeWindow()
        {
            Text = "Varlet v" + Globals.AppVersion + " build " + Globals.AppBuildNumber;
            btnServices.Text = "Start Services";
            comboPhpVersion.Enabled = true;
            lblReloadHttpd.Enabled = false;
            lblReloadSmtp.Enabled = false;
        }

        private static void CheckServiceStatus() {
            // do something
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

        private void btnServices_Click(object sender, EventArgs e)
        {
            if (Services.IsHttpServiceRun == false) {
                Services.Start(Globals.HttpServiceName);
                Services.IsHttpServiceRun = false;
                Services.IsSmtpServiceRun = false;
            }
        }

        private void StartingService()
        {
            pictStatusHttpd.BackColor = Color.Green;
            btnServices.Text = "Stop Services";
            comboPhpVersion.Enabled = false;
            lblReloadHttpd.Enabled = true;
            lblReloadSmtp.Enabled = true;
        }

        private void StoppingService()
        {
            for (int I = 0; I <= 10; I++) {
                btnServices.Text = "Stopping Services";
                pictStatusHttpd.BackColor = Color.Red;
                pictStatusSmtp.BackColor = Color.Red;
            }
        }

        private void btnTerminal_Click(object sender, EventArgs e)
        {
            var wwwDir = Common.GetAppPath() + @"\www";
            try
            {
                if (Directory.Exists(Common.DirProgramFiles(@"\PowerShell"))) {
                    var proc = new Process {StartInfo =
                    {
                        FileName = "pwsh.exe",
                        Arguments = "-NoLogo -WorkingDirectory " + wwwDir,
                        UseShellExecute = true
                    }};
                    proc.Start();
                } else  {
                    var proc = new Process {StartInfo =
                    {
                        FileName = "cmd.exe",
                        Arguments = "/k \"cd /d " + wwwDir + "\"",
                        UseShellExecute = true
                    }};
                    proc.Start();
                }
            } catch (FormatException) {
                // do something here
            }
        }

        private void CheckAvailablePhp()
        {
            var pkgPhp = Common.GetAppPath() + @"\pkg\php";
            try
            {
                if (!Directory.Exists(pkgPhp)) return;
                foreach (var t in Directory.GetDirectories(pkgPhp))  {
                    comboPhpVersion.Items.Add(Path.GetFileName(t));
                }
                comboPhpVersion.SelectedIndex = !IsNullOrEmpty(Config.Get("Services", "PhpVersion")) ?
                        comboPhpVersion.FindStringExact(Config.Get("Services", "PhpVersion")) : 0;
            }
            catch (FormatException)
            {
                // do something here
            }
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
            new FormSettings().ShowDialog();
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
            Config.Set("Services", "PhpVersion", comboPhpVersion.Text);
        }
    }
}

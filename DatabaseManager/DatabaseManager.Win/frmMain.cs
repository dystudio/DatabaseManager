﻿using DatabaseInterpreter.Core;
using DatabaseInterpreter.Utility;
using DatabaseManager.Helper;
using DatabaseManager.Model;
using System;
using System.Windows.Forms;

namespace DatabaseManager
{
    public partial class frmMain : Form, IObserver<FeedbackInfo>
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.InitControls();

            TreeView.CheckForIllegalCrossThreadCalls = false;

            FeedbackHelper.EnableLog = SettingManager.Setting.EnableLog;
            FeedbackHelper.EnableDebug = true;
        }

        private void InitControls()
        {
            this.navigator.OnShowContent += this.ShowDbObjectContent;
            this.navigator.OnFeedback += this.Feedback;
        }

        private void ShowDbObjectContent(DatabaseObjectDisplayInfo content)
        {
            this.ucContent.Visible = true;
            this.ucContent.ShowContent(content);
        }

        private void Feedback(FeedbackInfo info)
        {
            if (info.InfoType != FeedbackInfoType.Info)
            {
                MessageBox.Show(info.Message);
            }
            else
            {               
                this.tsslMessage.Text = info.Message;                  
            }
        }

        private void tsmiSetting_Click(object sender, EventArgs e)
        {
            frmSetting frmSetting = new frmSetting();
            frmSetting.ShowDialog();
        }

        private void btnGenerateScripts_Click(object sender, EventArgs e)
        {
            frmGenerateScripts frmGenerateScripts = new frmGenerateScripts();
            frmGenerateScripts.ShowDialog();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            frmConvert frmConvert = new frmConvert();
            frmConvert.ShowDialog();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (FormEventCenter.OnSave != null)
                {
                    FormEventCenter.OnSave();
                }
            }
        }

        public void OnNext(FeedbackInfo value)
        {
            this.Feedback(value);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}

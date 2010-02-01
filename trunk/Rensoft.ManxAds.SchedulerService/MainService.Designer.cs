namespace Rensoft.ManxAds.SchedulerService
{
    partial class MainService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.initialBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            // 
            // initialBackgroundWorker
            // 
            this.initialBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.initialBackgroundWorker_DoWork);
            this.initialBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.initialBackgroundWorker_RunWorkerCompleted);
            // 
            // MainService
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.ComponentModel.BackgroundWorker initialBackgroundWorker;

    }
}

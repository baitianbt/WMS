namespace WMS.UI
{
    partial class MainForm
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
            
            // Dispose timer
            if (timerClock != null)
            {
                timerClock.Stop();
                timerClock.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainMenuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            masterDataMenu = new ToolStripMenuItem();
            productsMenuItem = new ToolStripMenuItem();
            warehousesMenuItem = new ToolStripMenuItem();
            inventoryMenu = new ToolStripMenuItem();
            inventoryManagementMenuItem = new ToolStripMenuItem();
            operationsMenu = new ToolStripMenuItem();
            inboundMenuItem = new ToolStripMenuItem();
            outboundMenuItem = new ToolStripMenuItem();
            reportsMenu = new ToolStripMenuItem();
            inventoryReportMenuItem = new ToolStripMenuItem();
            movementReportMenuItem = new ToolStripMenuItem();
            systemMenu = new ToolStripMenuItem();
            userManagementMenuItem = new ToolStripMenuItem();
            changePasswordMenuItem = new ToolStripMenuItem();
            settingsMenuItem = new ToolStripMenuItem();
            logViewerMenuItem = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            lblUserInfo = new ToolStripStatusLabel();
            toolStripSeparator = new ToolStripSeparator();
            lblDateTime = new ToolStripStatusLabel();
            timerClock = new System.Windows.Forms.Timer(components);
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            mainMenuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.ImageScalingSize = new Size(20, 20);
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, masterDataMenu, inventoryMenu, operationsMenu, reportsMenu, systemMenu });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Padding = new Padding(9, 3, 0, 3);
            mainMenuStrip.Size = new Size(1200, 30);
            mainMenuStrip.TabIndex = 0;
            mainMenuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { exitMenuItem });
            fileMenu.Name = "fileMenu";
            fileMenu.Size = new Size(53, 24);
            fileMenu.Text = "文件";
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(224, 26);
            exitMenuItem.Text = "退出";
            // 
            // masterDataMenu
            // 
            masterDataMenu.DropDownItems.AddRange(new ToolStripItem[] { productsMenuItem, warehousesMenuItem });
            masterDataMenu.Name = "masterDataMenu";
            masterDataMenu.Size = new Size(83, 24);
            masterDataMenu.Text = "基础数据";
            // 
            // productsMenuItem
            // 
            productsMenuItem.Name = "productsMenuItem";
            productsMenuItem.Size = new Size(224, 26);
            productsMenuItem.Text = "产品";
            productsMenuItem.TextAlign = ContentAlignment.MiddleRight;
            productsMenuItem.Click += OpenProductForm;
            // 
            // warehousesMenuItem
            // 
            warehousesMenuItem.Name = "warehousesMenuItem";
            warehousesMenuItem.Size = new Size(224, 26);
            warehousesMenuItem.Text = "仓库";
            warehousesMenuItem.Click += OpenWarehouseForm;
            // 
            // inventoryMenu
            // 
            inventoryMenu.DropDownItems.AddRange(new ToolStripItem[] { inventoryManagementMenuItem });
            inventoryMenu.Name = "inventoryMenu";
            inventoryMenu.Size = new Size(92, 24);
            inventoryMenu.Text = "Inventory";
            // 
            // inventoryManagementMenuItem
            // 
            inventoryManagementMenuItem.Name = "inventoryManagementMenuItem";
            inventoryManagementMenuItem.Size = new Size(262, 26);
            inventoryManagementMenuItem.Text = "Inventory Management";
            inventoryManagementMenuItem.Click += OpenInventoryForm;
            // 
            // operationsMenu
            // 
            operationsMenu.DropDownItems.AddRange(new ToolStripItem[] { inboundMenuItem, outboundMenuItem });
            operationsMenu.Name = "operationsMenu";
            operationsMenu.Size = new Size(104, 24);
            operationsMenu.Text = "Operations";
            // 
            // inboundMenuItem
            // 
            inboundMenuItem.Name = "inboundMenuItem";
            inboundMenuItem.Size = new Size(221, 26);
            inboundMenuItem.Text = "Inbound Orders";
            // 
            // outboundMenuItem
            // 
            outboundMenuItem.Name = "outboundMenuItem";
            outboundMenuItem.Size = new Size(221, 26);
            outboundMenuItem.Text = "Outbound Orders";
            // 
            // reportsMenu
            // 
            reportsMenu.DropDownItems.AddRange(new ToolStripItem[] { inventoryReportMenuItem, movementReportMenuItem });
            reportsMenu.Name = "reportsMenu";
            reportsMenu.Size = new Size(81, 24);
            reportsMenu.Text = "Reports";
            // 
            // inventoryReportMenuItem
            // 
            inventoryReportMenuItem.Name = "inventoryReportMenuItem";
            inventoryReportMenuItem.Size = new Size(227, 26);
            inventoryReportMenuItem.Text = "Inventory Report";
            // 
            // movementReportMenuItem
            // 
            movementReportMenuItem.Name = "movementReportMenuItem";
            movementReportMenuItem.Size = new Size(227, 26);
            movementReportMenuItem.Text = "Movement Report";
            // 
            // systemMenu
            // 
            systemMenu.DropDownItems.AddRange(new ToolStripItem[] { userManagementMenuItem, changePasswordMenuItem, settingsMenuItem, logViewerMenuItem });
            systemMenu.Name = "systemMenu";
            systemMenu.Size = new Size(76, 24);
            systemMenu.Text = "System";
            // 
            // userManagementMenuItem
            // 
            userManagementMenuItem.Name = "userManagementMenuItem";
            userManagementMenuItem.Size = new Size(226, 26);
            userManagementMenuItem.Text = "User Management";
            // 
            // changePasswordMenuItem
            // 
            changePasswordMenuItem.Name = "changePasswordMenuItem";
            changePasswordMenuItem.Size = new Size(226, 26);
            changePasswordMenuItem.Text = "Change Password";
            // 
            // settingsMenuItem
            // 
            settingsMenuItem.Name = "settingsMenuItem";
            settingsMenuItem.Size = new Size(226, 26);
            settingsMenuItem.Text = "System Settings";
            // 
            // logViewerMenuItem
            // 
            logViewerMenuItem.Name = "logViewerMenuItem";
            logViewerMenuItem.Size = new Size(226, 26);
            logViewerMenuItem.Text = "Log Viewer";
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblUserInfo, toolStripSeparator, lblDateTime });
            statusStrip.Location = new Point(0, 666);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(2, 0, 21, 0);
            statusStrip.Size = new Size(1200, 26);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // lblUserInfo
            // 
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(107, 20);
            lblUserInfo.Text = "Current user: ";
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 26);
            // 
            // lblDateTime
            // 
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(83, 20);
            lblDateTime.Text = "Date Time";
            // 
            // timerClock
            // 
            timerClock.Interval = 1000;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 30);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            splitContainer1.Size = new Size(1200, 636);
            splitContainer1.SplitterDistance = 208;
            splitContainer1.TabIndex = 3;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(208, 636);
            treeView1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 692);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip);
            Controls.Add(mainMenuStrip);
            IsMdiContainer = true;
            MainMenuStrip = mainMenuStrip;
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "WMS - Warehouse Management System";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masterDataMenu;
        private System.Windows.Forms.ToolStripMenuItem productsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warehousesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryMenu;
        private System.Windows.Forms.ToolStripMenuItem inventoryManagementMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationsMenu;
        private System.Windows.Forms.ToolStripMenuItem inboundMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outboundMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsMenu;
        private System.Windows.Forms.ToolStripMenuItem inventoryReportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movementReportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemMenu;
        private System.Windows.Forms.ToolStripMenuItem userManagementMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logViewerMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblUserInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripStatusLabel lblDateTime;
        private System.Windows.Forms.Timer timerClock;
        private SplitContainer splitContainer1;
        private TreeView treeView1;
    }
} 
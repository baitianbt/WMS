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
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterDataMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.productsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warehousesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryManagementMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.inboundMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outboundMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movementReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.userManagementMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logViewerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblUserInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.lblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            
            // mainMenuStrip
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileMenu,
                this.masterDataMenu,
                this.inventoryMenu,
                this.operationsMenu,
                this.reportsMenu,
                this.systemMenu
            });
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            
            // fileMenu
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.exitMenuItem
            });
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            
            // exitMenuItem
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler((s, e) => System.Windows.Forms.Application.Exit());
            
            // masterDataMenu
            this.masterDataMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.productsMenuItem,
                this.warehousesMenuItem
            });
            this.masterDataMenu.Name = "masterDataMenu";
            this.masterDataMenu.Size = new System.Drawing.Size(82, 20);
            this.masterDataMenu.Text = "Master Data";
            
            // productsMenuItem
            this.productsMenuItem.Name = "productsMenuItem";
            this.productsMenuItem.Size = new System.Drawing.Size(134, 22);
            this.productsMenuItem.Text = "Products";
            this.productsMenuItem.Click += new System.EventHandler(this.OpenProductForm);
            
            // warehousesMenuItem
            this.warehousesMenuItem.Name = "warehousesMenuItem";
            this.warehousesMenuItem.Size = new System.Drawing.Size(134, 22);
            this.warehousesMenuItem.Text = "Warehouses";
            this.warehousesMenuItem.Click += new System.EventHandler(this.OpenWarehouseForm);
            
            // inventoryMenu
            this.inventoryMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.inventoryManagementMenuItem
            });
            this.inventoryMenu.Name = "inventoryMenu";
            this.inventoryMenu.Size = new System.Drawing.Size(69, 20);
            this.inventoryMenu.Text = "Inventory";
            
            // inventoryManagementMenuItem
            this.inventoryManagementMenuItem.Name = "inventoryManagementMenuItem";
            this.inventoryManagementMenuItem.Size = new System.Drawing.Size(196, 22);
            this.inventoryManagementMenuItem.Text = "Inventory Management";
            this.inventoryManagementMenuItem.Click += new System.EventHandler(this.OpenInventoryForm);
            
            // operationsMenu
            this.operationsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.inboundMenuItem,
                this.outboundMenuItem
            });
            this.operationsMenu.Name = "operationsMenu";
            this.operationsMenu.Size = new System.Drawing.Size(77, 20);
            this.operationsMenu.Text = "Operations";
            
            // inboundMenuItem
            this.inboundMenuItem.Name = "inboundMenuItem";
            this.inboundMenuItem.Size = new System.Drawing.Size(166, 22);
            this.inboundMenuItem.Text = "Inbound Orders";
            
            // outboundMenuItem
            this.outboundMenuItem.Name = "outboundMenuItem";
            this.outboundMenuItem.Size = new System.Drawing.Size(166, 22);
            this.outboundMenuItem.Text = "Outbound Orders";
            
            // reportsMenu
            this.reportsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.inventoryReportMenuItem,
                this.movementReportMenuItem
            });
            this.reportsMenu.Name = "reportsMenu";
            this.reportsMenu.Size = new System.Drawing.Size(59, 20);
            this.reportsMenu.Text = "Reports";
            
            // inventoryReportMenuItem
            this.inventoryReportMenuItem.Name = "inventoryReportMenuItem";
            this.inventoryReportMenuItem.Size = new System.Drawing.Size(168, 22);
            this.inventoryReportMenuItem.Text = "Inventory Report";
            
            // movementReportMenuItem
            this.movementReportMenuItem.Name = "movementReportMenuItem";
            this.movementReportMenuItem.Size = new System.Drawing.Size(168, 22);
            this.movementReportMenuItem.Text = "Movement Report";
            
            // systemMenu
            this.systemMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.userManagementMenuItem,
                this.changePasswordMenuItem,
                this.settingsMenuItem,
                this.logViewerMenuItem
            });
            this.systemMenu.Name = "systemMenu";
            this.systemMenu.Size = new System.Drawing.Size(57, 20);
            this.systemMenu.Text = "System";
            
            // userManagementMenuItem
            this.userManagementMenuItem.Name = "userManagementMenuItem";
            this.userManagementMenuItem.Size = new System.Drawing.Size(168, 22);
            this.userManagementMenuItem.Text = "User Management";
            
            // changePasswordMenuItem
            this.changePasswordMenuItem.Name = "changePasswordMenuItem";
            this.changePasswordMenuItem.Size = new System.Drawing.Size(168, 22);
            this.changePasswordMenuItem.Text = "Change Password";
            
            // settingsMenuItem
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(168, 22);
            this.settingsMenuItem.Text = "System Settings";
            
            // logViewerMenuItem
            this.logViewerMenuItem.Name = "logViewerMenuItem";
            this.logViewerMenuItem.Size = new System.Drawing.Size(168, 22);
            this.logViewerMenuItem.Text = "Log Viewer";
            
            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblUserInfo,
                this.toolStripSeparator,
                this.lblDateTime
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            
            // lblUserInfo
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(79, 17);
            this.lblUserInfo.Text = "Current user: ";
            
            // toolStripSeparator
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 22);
            
            // lblDateTime
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(65, 17);
            this.lblDateTime.Text = "Date Time";
            
            // timerClock
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler((s, e) => this.UpdateDateTimeLabel());
            
            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "WMS - Warehouse Management System";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
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
    }
} 
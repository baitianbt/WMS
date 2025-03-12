-- Create the WMS database
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'WMS')
BEGIN
    CREATE DATABASE WMS;
END
GO

USE WMS;
GO

-- Create Products table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Products](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Code] [nvarchar](50) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [Category] [nvarchar](50) NULL,
        [Price] [decimal](18, 2) NOT NULL,
        [Unit] [nvarchar](20) NULL,
        [Barcode] [nvarchar](50) NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create Warehouses table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Warehouses]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Warehouses](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Code] [nvarchar](50) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Address] [nvarchar](200) NULL,
        [Contact] [nvarchar](50) NULL,
        [Phone] [nvarchar](20) NULL,
        [Description] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_Warehouses] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create Inventories table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Inventories](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ProductId] [int] NOT NULL,
        [WarehouseId] [int] NOT NULL,
        [Location] [nvarchar](50) NULL,
        [Quantity] [decimal](18, 2) NOT NULL,
        [MinQuantity] [decimal](18, 2) NOT NULL,
        [MaxQuantity] [decimal](18, 2) NOT NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_Inventories] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Inventories_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]),
        CONSTRAINT [FK_Inventories_Warehouses] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
    );
END
GO

-- Create InboundOrders table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InboundOrders]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[InboundOrders](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [OrderNo] [nvarchar](50) NOT NULL,
        [WarehouseId] [int] NOT NULL,
        [SupplierName] [nvarchar](100) NULL,
        [SupplierContact] [nvarchar](50) NULL,
        [SupplierPhone] [nvarchar](20) NULL,
        [OrderDate] [datetime] NOT NULL,
        [ExpectedArrivalDate] [datetime] NOT NULL,
        [ActualArrivalDate] [datetime] NULL,
        [Status] [nvarchar](20) NOT NULL,
        [Remarks] [nvarchar](500) NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_InboundOrders] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_InboundOrders_Warehouses] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
    );
END
GO

-- Create InboundOrderDetails table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InboundOrderDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[InboundOrderDetails](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [InboundOrderId] [int] NOT NULL,
        [ProductId] [int] NOT NULL,
        [ExpectedQuantity] [decimal](18, 2) NOT NULL,
        [ActualQuantity] [decimal](18, 2) NOT NULL,
        [Location] [nvarchar](50) NULL,
        [Status] [nvarchar](20) NOT NULL,
        [Remarks] [nvarchar](500) NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_InboundOrderDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_InboundOrderDetails_InboundOrders] FOREIGN KEY([InboundOrderId]) REFERENCES [dbo].[InboundOrders] ([Id]),
        CONSTRAINT [FK_InboundOrderDetails_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    );
END
GO

-- Create OutboundOrders table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OutboundOrders]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[OutboundOrders](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [OrderNo] [nvarchar](50) NOT NULL,
        [WarehouseId] [int] NOT NULL,
        [CustomerName] [nvarchar](100) NULL,
        [CustomerContact] [nvarchar](50) NULL,
        [CustomerPhone] [nvarchar](20) NULL,
        [ShippingAddress] [nvarchar](200) NULL,
        [OrderDate] [datetime] NOT NULL,
        [ExpectedShippingDate] [datetime] NOT NULL,
        [ActualShippingDate] [datetime] NULL,
        [Status] [nvarchar](20) NOT NULL,
        [Remarks] [nvarchar](500) NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_OutboundOrders] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_OutboundOrders_Warehouses] FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
    );
END
GO

-- Create OutboundOrderDetails table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OutboundOrderDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[OutboundOrderDetails](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [OutboundOrderId] [int] NOT NULL,
        [ProductId] [int] NOT NULL,
        [RequestedQuantity] [decimal](18, 2) NOT NULL,
        [ActualQuantity] [decimal](18, 2) NOT NULL,
        [Location] [nvarchar](50) NULL,
        [Status] [nvarchar](20) NOT NULL,
        [Remarks] [nvarchar](500) NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_OutboundOrderDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_OutboundOrderDetails_OutboundOrders] FOREIGN KEY([OutboundOrderId]) REFERENCES [dbo].[OutboundOrders] ([Id]),
        CONSTRAINT [FK_OutboundOrderDetails_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    );
END
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [Password] [nvarchar](100) NOT NULL,
        [Salt] [nvarchar](50) NOT NULL,
        [FullName] [nvarchar](100) NULL,
        [Email] [nvarchar](100) NULL,
        [Phone] [nvarchar](20) NULL,
        [Role] [int] NOT NULL,
        [IsActive] [bit] NOT NULL,
        [LoginFailedCount] [int] NOT NULL,
        [LastLoginTime] [datetime] NULL,
        [CreatedTime] [datetime] NOT NULL,
        [UpdatedTime] [datetime] NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    -- 添加唯一索引
    CREATE UNIQUE INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
END
GO

-- Create SystemLogs table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SystemLogs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [LogLevel] [int] NOT NULL,
        [Message] [nvarchar](max) NOT NULL,
        [Exception] [nvarchar](max) NULL,
        [UserId] [int] NULL,
        [Username] [nvarchar](50) NULL,
        [LogTime] [datetime] NOT NULL,
        [Source] [nvarchar](100) NULL,
        [IPAddress] [nvarchar](50) NULL,
        CONSTRAINT [PK_SystemLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    -- 添加索引
    CREATE INDEX [IX_SystemLogs_LogTime] ON [dbo].[SystemLogs]([LogTime]);
    CREATE INDEX [IX_SystemLogs_LogLevel] ON [dbo].[SystemLogs]([LogLevel]);
    CREATE INDEX [IX_SystemLogs_UserId] ON [dbo].[SystemLogs]([UserId]);
END
GO

-- Create UserLoginHistory table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLoginHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserLoginHistory](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [int] NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [LoginTime] [datetime] NOT NULL,
        [LogoutTime] [datetime] NULL,
        [IPAddress] [nvarchar](50) NULL,
        [LoginStatus] [bit] NOT NULL, -- 1: 成功, 0: 失败
        [FailReason] [nvarchar](200) NULL,
        CONSTRAINT [PK_UserLoginHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_UserLoginHistory_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id])
    );
    
    -- 添加索引
    CREATE INDEX [IX_UserLoginHistory_UserId] ON [dbo].[UserLoginHistory]([UserId]);
    CREATE INDEX [IX_UserLoginHistory_LoginTime] ON [dbo].[UserLoginHistory]([LoginTime]);
END
GO

-- Insert sample data
-- Sample Products
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Products])
BEGIN
    INSERT INTO [dbo].[Products] ([Code], [Name], [Description], [Category], [Price], [Unit], [Barcode], [CreatedTime], [UpdatedTime])
    VALUES
        ('P001', 'Laptop', 'High-performance laptop', 'Electronics', 1200.00, 'Unit', 'BAR001', GETDATE(), GETDATE()),
        ('P002', 'Smartphone', 'Latest smartphone model', 'Electronics', 800.00, 'Unit', 'BAR002', GETDATE(), GETDATE()),
        ('P003', 'Office Chair', 'Ergonomic office chair', 'Furniture', 150.00, 'Unit', 'BAR003', GETDATE(), GETDATE()),
        ('P004', 'Desk', 'Office desk', 'Furniture', 200.00, 'Unit', 'BAR004', GETDATE(), GETDATE()),
        ('P005', 'Printer', 'Color laser printer', 'Electronics', 350.00, 'Unit', 'BAR005', GETDATE(), GETDATE());
END
GO

-- Sample Warehouses
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Warehouses])
BEGIN
    INSERT INTO [dbo].[Warehouses] ([Code], [Name], [Address], [Contact], [Phone], [Description], [IsActive], [CreatedTime], [UpdatedTime])
    VALUES
        ('W001', 'Main Warehouse', '123 Main St, City', 'John Doe', '123-456-7890', 'Main storage facility', 1, GETDATE(), GETDATE()),
        ('W002', 'Electronics Warehouse', '456 Tech Blvd, City', 'Jane Smith', '123-456-7891', 'Electronics storage', 1, GETDATE(), GETDATE()),
        ('W003', 'Furniture Warehouse', '789 Furniture Ave, City', 'Bob Johnson', '123-456-7892', 'Furniture storage', 1, GETDATE(), GETDATE());
END
GO

-- Sample Inventories
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Inventories])
BEGIN
    INSERT INTO [dbo].[Inventories] ([ProductId], [WarehouseId], [Location], [Quantity], [MinQuantity], [MaxQuantity], [CreatedTime], [UpdatedTime])
    VALUES
        (1, 1, 'A1-01', 50.00, 10.00, 100.00, GETDATE(), GETDATE()),
        (2, 1, 'A1-02', 75.00, 15.00, 150.00, GETDATE(), GETDATE()),
        (3, 3, 'B2-01', 30.00, 5.00, 50.00, GETDATE(), GETDATE()),
        (4, 3, 'B2-02', 20.00, 5.00, 40.00, GETDATE(), GETDATE()),
        (5, 2, 'C3-01', 15.00, 3.00, 30.00, GETDATE(), GETDATE());
END
GO

-- Sample Users (密码: admin123, manager123, operator123, viewer123)
-- 注意: 这里的密码是明文，实际应用中应该使用加密后的密码
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Users])
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [FullName], [Email], [Phone], [Role], [IsActive], [LoginFailedCount], [LastLoginTime], [CreatedTime], [UpdatedTime])
    VALUES
        ('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'WMS-SALT', '系统管理员', 'admin@example.com', '13800000000', 1, 1, 0, NULL, GETDATE(), GETDATE()),
        ('manager', '6ee4a469cd4e91053847f5d3fcb61dbcc91e8f0ef10be7748da4c4a1ba382d17', 'WMS-SALT', '仓库经理', 'manager@example.com', '13800000001', 2, 1, 0, NULL, GETDATE(), GETDATE()),
        ('operator', 'a798b44a5aa88b7a54f0f15b3b1d4854e4b601e840f8418e3f160d87b5c2b5af', 'WMS-SALT', '操作员', 'operator@example.com', '13800000002', 3, 1, 0, NULL, GETDATE(), GETDATE()),
        ('viewer', '4a5d9c99a3b1d0d61cb166b100ff35db4889d55bf91a95d5f24c6b6c77f36aed', 'WMS-SALT', '查看者', 'viewer@example.com', '13800000003', 4, 1, 0, NULL, GETDATE(), GETDATE());
END
GO 
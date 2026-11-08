CREATE TABLE [Categories] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [DiningTables] (
    [Id] uniqueidentifier NOT NULL,
    [Number] int NOT NULL,
    [Capacity] int NOT NULL,
    [IsAvailable] bit NOT NULL,
    CONSTRAINT [PK_DiningTables] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Roles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Dishes] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(300) NULL,
    [Price] decimal(10,2) NOT NULL,
    [IsActive] bit NOT NULL,
    [CategoryId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Dishes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dishes_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Employees] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(32) NOT NULL,
    [Telephone] nvarchar(20) NULL,
    [Email] nvarchar(50) NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [DiningTableId] uniqueidentifier NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_DiningTables_DiningTableId] FOREIGN KEY ([DiningTableId]) REFERENCES [DiningTables] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Orders_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Bills] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [PaidAt] datetime2 NOT NULL,
    [PaymentMethod] int NOT NULL,
    [Total] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_Bills] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bills_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [OrderDetails] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [DishId] uniqueidentifier NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderDetails_Dishes_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dishes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO


CREATE UNIQUE INDEX [IX_Bills_OrderId] ON [Bills] ([OrderId]);
GO


CREATE INDEX [IX_Dishes_CategoryId] ON [Dishes] ([CategoryId]);
GO


CREATE INDEX [IX_Employees_RoleId] ON [Employees] ([RoleId]);
GO


CREATE INDEX [IX_OrderDetails_DishId] ON [OrderDetails] ([DishId]);
GO


CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
GO


CREATE INDEX [IX_Orders_DiningTableId] ON [Orders] ([DiningTableId]);
GO


CREATE INDEX [IX_Orders_EmployeeId] ON [Orders] ([EmployeeId]);
GO



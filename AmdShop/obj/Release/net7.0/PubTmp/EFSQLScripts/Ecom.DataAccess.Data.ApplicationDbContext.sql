IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Authors] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Companies] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [StreetAddress] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Roles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [ISBN] nvarchar(max) NOT NULL,
        [AuthorId] int NOT NULL,
        [ListPrice] float NOT NULL,
        [Price] float NOT NULL,
        [Price50] float NOT NULL,
        [Price100] float NOT NULL,
        [CategoryId] int NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [Users] (
        [Id] nvarchar(450) NOT NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NULL,
        [StreetAddress] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [State] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        [CompanyId] uniqueidentifier NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [RoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [ProductImages] (
        [Id] uniqueidentifier NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [OrderHeaders] (
        [Id] int NOT NULL IDENTITY,
        [ApplicationUserId] nvarchar(450) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [ShippingDate] datetime2 NOT NULL,
        [OrderTotal] float NOT NULL,
        [OrderStatus] nvarchar(max) NULL,
        [PaymentStatus] nvarchar(max) NULL,
        [TrackingNumber] nvarchar(max) NULL,
        [Carrier] nvarchar(max) NULL,
        [PaymentDate] datetime2 NOT NULL,
        [PaymentDueDate] datetime2 NOT NULL,
        [SessionId] nvarchar(max) NULL,
        [PaymentIntentId] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [StreetAddress] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [State] nvarchar(max) NOT NULL,
        [PostalCode] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_OrderHeaders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderHeaders_Users_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [ShoppingCarts] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] uniqueidentifier NOT NULL,
        [Count] int NOT NULL,
        [ApplicationUserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ShoppingCarts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ShoppingCarts_Users_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [UserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [UserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [UserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] int NOT NULL IDENTITY,
        [OrderHeaderId] int NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Count] int NOT NULL,
        [Price] float NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_OrderHeaders_OrderHeaderId] FOREIGN KEY ([OrderHeaderId]) REFERENCES [OrderHeaders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] ON;
    EXEC(N'INSERT INTO [Authors] ([Id], [Name])
    VALUES (1, N''AMD''),
    (2, N''INTEL''),
    (3, N''MSI''),
    (4, N''ASROCK''),
    (5, N''ASUS''),
    (6, N''GIGABYTE''),
    (7, N''HKC''),
    (8, N''AOC''),
    (9, N''E-DRA''),
    (10, N''DAREU''),
    (11, N''CORSAIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [DisplayOrder], [Name])
    VALUES (1, 1, N''Mainboard''),
    (2, 2, N''CPU''),
    (3, 3, N''VGA''),
    (4, 4, N''Monitor''),
    (5, 5, N''Keyboard''),
    (6, 6, N''Mouse'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'Name', N'PhoneNumber', N'PostalCode', N'State', N'StreetAddress') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] ON;
    EXEC(N'INSERT INTO [Companies] ([Id], [City], [Name], [PhoneNumber], [PostalCode], [State], [StreetAddress])
    VALUES (''0b3346f8-438a-42a7-a5e8-51fe3f96d808'', N''Lala land'', N''Readers Club'', N''1113335555'', N''99999'', N''NY'', N''999 Main St''),
    (''9437abb9-a386-4ba8-b237-51ed595b4113'', N''Tech City'', N''Tech Solution'', N''6669990000'', N''12121'', N''IL'', N''123 Tech St''),
    (''9a92dfa5-30c0-40a0-84fa-b69bee8f73fc'', N''Vid City'', N''Vivid Books'', N''7779990000'', N''66666'', N''IL'', N''999 Vid St'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'Name', N'PhoneNumber', N'PostalCode', N'State', N'StreetAddress') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'CategoryId', N'Description', N'ISBN', N'ListPrice', N'Price', N'Price100', N'Price50', N'Title') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [AuthorId], [CategoryId], [Description], [ISBN], [ListPrice], [Price], [Price100], [Price50], [Title])
    VALUES (''04e7318d-61bb-455d-931b-4baf081e8f99'', 10, 5, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWD22EK87 '', 500000.0E0, 490000.0E0, 410000.0E0, 450000.0E0, N'' DAREU EK87 - Black''),
    (''091d512e-9706-4d9b-9823-58eb7f83f118'', 10, 5, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDCA87SWALLOW'', 1500000.0E0, 1490000.0E0, 1410000.0E0, 1450000.0E0, N''DAREU A87 SWALLOW (PBT, Brown/ Red CHERRY switch) ''),
    (''1db6c932-e9ee-4ffd-bbf0-92ef23061982'', 1, 3, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAW570OPP'', 2000000.0E0, 1900000.0E0, 1650000.0E0, 1750000.0E0, N''RX 570''),
    (''1fc51323-7607-4a7f-aaf7-05f09b0b3dfb'', 1, 3, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOX5600X'', 3000000.0E0, 2900000.0E0, 2650000.0E0, 2750000.0E0, N''RX 6600XT''),
    (''33005ad9-ddd8-4dbb-b025-669d781f4b5f'', 3, 1, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWDA520MM'', 1000000.0E0, 900000.0E0, 650000.0E0, 750000.0E0, N''A520M PRO''),
    (''33ed75de-1f7f-41aa-bb3d-fe2ab5f9d265'', 3, 1, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDX570AM'', 7000000.0E0, 6900000.0E0, 6650000.0E0, 6750000.0E0, N''X570 ACE''),
    (''380825b1-2650-4b48-8cc2-d9c35a509ba8'', 1, 3, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWD550OO'', 1000000.0E0, 900000.0E0, 650000.0E0, 750000.0E0, N''RX 550''),
    (''38aed6b0-ab77-4d79-83ff-a9d3adc46758'', 10, 6, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAW57EM901X'', 600000.0E0, 590000.0E0, 510000.0E0, 550000.0E0, N'' DareU EM901X RGB Superlight  ''),
    (''3ef348a2-d648-4563-aa0e-e6569bc93d84'', 10, 5, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOEK8100'', 900000.0E0, 890000.0E0, 810000.0E0, 850000.0E0, N'' DAREU EK8100 100KEY (RGB, Blue/ Brown/ Red D switch)  ''),
    (''401da5d4-6f94-44ea-a4c7-3489db8056e8'', 9, 4, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAW57EGM24F75'', 2000000.0E0, 1900000.0E0, 1650000.0E0, 1750000.0E0, N''E-DRA EGM24F75 (23.8/FHD/IPS/75Hz/1ms) ''),
    (''4271654f-fa30-4a61-89bb-561b61d37eb7'', 4, 1, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAWB550A'', 2000000.0E0, 1900000.0E0, 1650000.0E0, 1750000.0E0, N''B550M STEEL LEGEND''),
    (''457a2725-0870-4d56-9706-a5fd11a83fc1'', 11, 6, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDCQM65SE'', 1500000.0E0, 1490000.0E0, 1410000.0E0, 1450000.0E0, N''Corsair M65 RGB ULTRA Black''),
    (''5b3d98f9-efa7-4503-a9b1-fbde14fc94de'', 10, 6, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWD24A918 '', 500000.0E0, 490000.0E0, 410000.0E0, 450000.0E0, N'' DAREU A918 - BLACK (PixArt PMW3335)''),
    (''8361d4af-7be2-4f20-b544-d54c1d149f90'', 1, 2, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAW3600'', 2000000.0E0, 1900000.0E0, 1650000.0E0, 1750000.0E0, N''RYZEN 5 3600''),
    (''88142b2b-d710-46c1-87b0-ca4fb9e8ee6e'', 1, 1, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWD3100'', 1000000.0E0, 900000.0E0, 650000.0E0, 750000.0E0, N''RYZEN 3 3100''),
    (''8b614a6d-52e9-4093-9948-00ccd45ca6d8'', 8, 4, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDCQ32G3SE'', 7000000.0E0, 6900000.0E0, 6650000.0E0, 6750000.0E0, N''AOC CQ32G3SE/74 (31.5/QHD/VA/165HZ/1MS) ''),
    (''9cbcc2e0-0dbf-437f-92c7-c18e96852a2d'', 10, 5, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''CAW57EEK810'', 600000.0E0, 590000.0E0, 510000.0E0, 550000.0E0, N'' EK810 Pink (USB/Pink Led/Blue/Brown/Red switch)  ''),
    (''a180a819-51e9-46c9-b4e0-6e406a6fc658'', 5, 1, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOX570A'', 3000000.0E0, 2900000.0E0, 2650000.0E0, 2750000.0E0, N''X570 TUF''),
    (''b0ac6bbc-3c5a-4787-89ac-34b43b1eaa61'', 1, 2, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDX5900XX'', 7000000.0E0, 6900000.0E0, 6650000.0E0, 6750000.0E0, N''RYZEN 9 5900X''),
    (''b1317290-360b-4984-b45a-3b8138a11b39'', 5, 4, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOX249H'', 3000000.0E0, 2900000.0E0, 2650000.0E0, 2750000.0E0, N''Asus VY249HE (23.8/FHD/IPS/75Hz/1ms) ''),
    (''c273f848-a068-42b4-9baf-382079bb52a1'', 7, 4, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''SWD22F220'', 1000000.0E0, 900000.0E0, 650000.0E0, 750000.0E0, N''HKC ANT-22F220 (22/FHD/75Hz/1ms)''),
    (''dd8d66e3-3d59-415c-8e82-e727b3e215bd'', 10, 6, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOA960YELLOW'', 900000.0E0, 890000.0E0, 810000.0E0, 850000.0E0, N'' DAREU A960 YELLOW - ULTRALIGHT ''),
    (''ea4d6b02-3eee-430a-a160-e4d5f5c7c943'', 1, 3, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''XNDX5900XX'', 7000000.0E0, 6900000.0E0, 6650000.0E0, 6750000.0E0, N''RX 6800XT''),
    (''eb5f667a-201a-4727-a367-edc3f914b32d'', 1, 2, CONCAT(CAST(N''Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''Nunc malesuada viverra ipsum sit amet tincidunt. ''), N''RITOX5600X'', 3000000.0E0, 2900000.0E0, 2650000.0E0, 2750000.0E0, N''RYZEN 5 5600X'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'CategoryId', N'Description', N'ISBN', N'ListPrice', N'Price', N'Price100', N'Price50', N'Title') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderHeaderId] ON [OrderDetails] ([OrderHeaderId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_OrderHeaders_ApplicationUserId] ON [OrderHeaders] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_Products_AuthorId] ON [Products] ([AuthorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_ShoppingCarts_ApplicationUserId] ON [ShoppingCarts] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_ShoppingCarts_ProductId] ON [ShoppingCarts] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    CREATE INDEX [IX_Users_CompanyId] ON [Users] ([CompanyId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230618043009_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230618043009_init', N'7.0.5');
END;
GO

COMMIT;
GO


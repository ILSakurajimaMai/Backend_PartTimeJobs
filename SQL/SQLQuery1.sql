USE [PartTimeJobs];
GO

-- 1. Tạo bảng Permissions (Danh sách quyền)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[Permissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [auth].[Permissions](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Code] [nvarchar](100) NOT NULL, -- Ví dụ: JobPosts.Create
        [Description] [nvarchar](255) NULL,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Permissions_Code] UNIQUE ([Code])
    );
END
GO

-- 2. Tạo bảng RolePermissions (Bảng trung gian Role - Permission)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[auth].[RolePermissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [auth].[RolePermissions](
        [RoleId] [int] NOT NULL,
        [PermissionId] [int] NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
        CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId]) REFERENCES [auth].[Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PermissionId]) REFERENCES [auth].[Permissions] ([Id]) ON DELETE CASCADE
    );
END
GO
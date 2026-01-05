DECLARE @AdminRoleId INT = 1;
DECLARE @EmployerRoleId INT = 2;
DECLARE @StudentRoleId INT = 3;

-- Biến tạm để lấy ID permission
DECLARE @PermId INT;

-- =======================================================
-- 1. CẤP QUYỀN CHO ADMIN (Full quyền)
-- =======================================================
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @AdminRoleId, Id FROM [auth].[Permissions];

-- =======================================================
-- 2. CẤP QUYỀN CHO EMPLOYER (Nhà tuyển dụng)
-- =======================================================
-- Nhóm JobPosts
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @EmployerRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'JobPosts.View',
    'JobPosts.Create',
    'JobPosts.Update',
    'JobPosts.Delete',
    'JobPosts.ChangeStatus'
);

-- Nhóm Applications (Chỉ xem ứng viên và duyệt)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @EmployerRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Applications.ViewByJob',
    'Applications.UpdateStatus'
);

-- Nhóm Companies
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @EmployerRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Companies.View',
    'Companies.ManageMy'
);

-- Nhóm Profiles (Xem ứng viên)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @EmployerRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Profiles.View'
);

-- =======================================================
-- 3. CẤP QUYỀN CHO STUDENT (Sinh viên/Người tìm việc)
-- =======================================================
-- Nhóm JobPosts (Chỉ xem)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @StudentRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'JobPosts.View'
);

-- Nhóm Applications (Nộp đơn, rút đơn, xem lịch sử)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @StudentRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Applications.Create',
    'Applications.Withdraw',
    'Applications.ViewMy'
);

-- Nhóm Companies (Chỉ xem và đăng ký mới nếu cần)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @StudentRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Companies.View',
    'Companies.Register'
);

-- Nhóm Profiles (Quản lý hồ sơ mình)
INSERT INTO [auth].[RolePermissions] ([RoleId], [PermissionId])
SELECT @StudentRoleId, Id FROM [auth].[Permissions] WHERE Code IN (
    'Profiles.ManageMy',
    'Profiles.View'
);

GO
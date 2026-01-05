DELETE FROM [auth].[RolePermissions];
DELETE FROM [auth].[Permissions];
DBCC CHECKIDENT ('[auth].[Permissions]', RESEED, 0);
GO

-- Insert danh sách quyền
INSERT INTO [auth].[Permissions] ([Code], [Description]) VALUES
-- Job Posts (Tin tuyển dụng)
('JobPosts.View', N'Xem tin tuyển dụng'),
('JobPosts.Create', N'Đăng tin tuyển dụng mới'),
('JobPosts.Update', N'Cập nhật tin tuyển dụng'),
('JobPosts.Delete', N'Xóa tin tuyển dụng'),
('JobPosts.ChangeStatus', N'Thay đổi trạng thái tin tuyển dụng'),

-- Applications (Ứng tuyển)
('Applications.ViewMy', N'Xem lịch sử ứng tuyển của cá nhân'),
('Applications.ViewByJob', N'Xem danh sách ứng viên của tin đăng'),
('Applications.Create', N'Nộp đơn ứng tuyển'),
('Applications.Withdraw', N'Rút đơn ứng tuyển'),
('Applications.UpdateStatus', N'Cập nhật trạng thái ứng viên (Duyệt/Từ chối)'),

-- Companies (Công ty)
('Companies.View', N'Xem thông tin công ty'),
('Companies.ManageMy', N'Quản lý thông tin công ty của mình'),
('Companies.Register', N'Đăng ký công ty mới'),

-- Profiles (Hồ sơ cá nhân)
('Profiles.View', N'Xem hồ sơ ứng viên'),
('Profiles.ManageMy', N'Quản lý hồ sơ cá nhân của mình'),

-- System/Admin (Quản trị)
('System.AccessLogs', N'Xem nhật ký hệ thống'),
('System.ManageUsers', N'Quản lý người dùng');
GO
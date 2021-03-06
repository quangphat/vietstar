USE [fintechcom_vn_PortalNew]
GO
SET IDENTITY_INSERT [dbo].[RolePermission] ON 

INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (1, 1, N'admin', N'profile', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (2, 1, N'admin', N'mcprofile', 0)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (3, 1, N'admin', N'mcprofile.submit', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (4, 1, N'admin', N'employee', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (5, 1, N'admin', N'revoke.import', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (6, 1, N'admin', N'courier', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (7, 1, N'admin', N'revoke', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (8, 1, N'admin', N'revoke.export', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (9, 1, N'admin', N'mcprofile', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (10, 1, N'admin', N'mcprofile.export', NULL)
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission], [IsDeleted]) VALUES (11, 1, N'admin', N'product', NULL)
SET IDENTITY_INSERT [dbo].[RolePermission] OFF

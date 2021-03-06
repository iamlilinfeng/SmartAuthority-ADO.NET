/****** Object:  Table [dbo].[authority_account]    Script Date: 12/04/2016 20:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authority_account]') AND type in (N'U'))
DROP TABLE [dbo].[authority_account]
GO
/****** Object:  Table [dbo].[authority_account_role]    Script Date: 12/04/2016 20:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authority_account_role]') AND type in (N'U'))
DROP TABLE [dbo].[authority_account_role]
GO
/****** Object:  Table [dbo].[authority_operate]    Script Date: 12/04/2016 20:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authority_operate]') AND type in (N'U'))
DROP TABLE [dbo].[authority_operate]
GO
/****** Object:  Table [dbo].[authority_role]    Script Date: 12/04/2016 20:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authority_role]') AND type in (N'U'))
DROP TABLE [dbo].[authority_role]
GO
/****** Object:  Table [dbo].[authority_role_operate]    Script Date: 12/04/2016 20:29:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authority_role_operate]') AND type in (N'U'))
DROP TABLE [dbo].[authority_role_operate]
GO
/****** Object:  StoredProcedure [dbo].[prPager]    Script Date: 12/04/2016 20:29:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[prPager]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[prPager]
GO
/****** Object:  StoredProcedure [dbo].[prPager]    Script Date: 12/04/2016 20:11:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[prPager]
	@PageSize INT, ----每页记录数
	@CurrentCount INT, ----当前记录数量（页码*每页记录数）
	@TableName NVARCHAR(200), ----表名称
	@Where NVARCHAR(max), ----查询条件
	@Order NVARCHAR(800),----排序条件
	@TotalCount INT OUTPUT ----记录总数
AS
	DECLARE @sqlSelect    NVARCHAR(max) ----局部变量（sql语句），查询记录集
	DECLARE @sqlGetCount  NVARCHAR(max) ----局部变量（sql语句），取出记录集总数
	
	
	SET @sqlSelect = 'SELECT * FROM  ' 
	    + '    (SELECT ROW_NUMBER()  OVER( ORDER BY ' + @Order +
	    ' ) AS RowNumber,* '
	    + '        FROM ' + @TableName 
	    + '        WHERE ' + @Where 
	    + '     ) as  T2 ' 
	    + ' WHERE T2.RowNumber<= ' + STR(@CurrentCount + @PageSize) +
	    ' AND T2.RowNumber>' + STR(@CurrentCount) 
	
	SET @sqlGetCount = 'SELECT @TotalCount = COUNT(*) FROM ' + @TableName 
	    + ' WHERE ' + @Where
	
	
	EXEC (@sqlSelect) 
	EXEC SP_EXECUTESQL @sqlGetCount,
	     N'@TotalCount INT OUTPUT',
	     @TotalCount OUTPUT
GO
/****** Object:  Table [dbo].[authority_role_operate]    Script Date: 12/04/2016 20:11:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authority_role_operate](
	[authority_role_operate_id] [int] IDENTITY(1,1) NOT NULL,
	[authority_role_id] [int] NOT NULL,
	[authority_operate_id] [int] NOT NULL,
 CONSTRAINT [PK_authority_role_operate] PRIMARY KEY CLUSTERED 
(
	[authority_role_operate_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 1)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 2)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 3)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 4)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 5)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 6)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 7)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 8)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 9)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 10)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 11)
INSERT [dbo].[authority_role_operate] ([authority_role_id], [authority_operate_id]) VALUES (1, 12)
/****** Object:  Table [dbo].[authority_role]    Script Date: 12/04/2016 20:11:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[authority_role](
	[authority_role_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](20) NOT NULL,
	[describe] [varchar](80) NOT NULL,
	[create_time] [datetime] NOT NULL,
	[enable] [bit] NOT NULL,
 CONSTRAINT [PK_AUTHORITY_ROLE] PRIMARY KEY CLUSTERED 
(
	[authority_role_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[authority_role] ON
INSERT [dbo].[authority_role] ([authority_role_id], [name], [describe], [create_time], [enable]) VALUES (1, N'超级管理员', N'超级管理员', CAST(0x0000A41300000000 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[authority_role] OFF
/****** Object:  Table [dbo].[authority_operate]    Script Date: 12/04/2016 20:11:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[authority_operate](
	[authority_operate_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](40) NOT NULL,
	[control] [varchar](40) NOT NULL,
	[action] [varchar](40) NOT NULL,
	[icon] [varchar](40) NOT NULL,
	[visible] [bit] NOT NULL,
	[order] [int] NOT NULL,
	[parent_id] [int] NOT NULL,
	[create_time] [datetime] NOT NULL,
	[enable] [bit] NOT NULL,
	[level] [int] NOT NULL,
 CONSTRAINT [PK_AUTHORITY_OPERATE] PRIMARY KEY CLUSTERED 
(
	[authority_operate_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'系统管理', N'#', N'#', N'fa-cogs', 1, 100, 0, getdate(), 1, 1)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'角色判断', N'Authority', N'Index', N'fa-cog', 0, 0, 1,  getdate(), 1, 2)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'修改密码', N'Authority', N'ChangePassword', N'fa-key', 1, 100, 1,  getdate(), 1, 2)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'账户管理', N'Authority', N'AccountList', N'fa-male', 1, 1, 1,  getdate(), 1, 2)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'账户新增', N'Authority', N'AccountEdit', N'fa-cog', 0, 0, 4,  getdate(), 1, 3)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'账户禁用', N'Authority', N'AccountDisable', N'fa-cog', 0, 0, 1,  getdate(), 1, 3)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'账户启用', N'Authority', N'AccountEnable', N'fa-cog', 0, 0, 1,  getdate(), 1, 3)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'角色管理', N'Authority', N'RoleList', N'fa-eye-slash', 1, 2, 1,  getdate(), 1, 2)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'角色编辑', N'Authority', N'RoleEdit', N'fa-cog', 0, 0, 8,  getdate(), 1, 3)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'角色用户', N'Authority', N'RoleAccount', N'fa-cog', 0, 0, 8,  getdate(), 1, 3)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'功能管理', N'Authority', N'OperateList', N'fa-sliders', 1, 3, 1, getdate(), 1, 2)
INSERT [dbo].[authority_operate] ([name], [control], [action], [icon], [visible], [order], [parent_id], [create_time], [enable], [level]) VALUES (N'功能编辑', N'Authority', N'OperateEdit', N'fa-cog', 0, 1, 11, getdate(),1, 3)
/****** Object:  Table [dbo].[authority_account_role]    Script Date: 12/04/2016 20:11:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authority_account_role](
	[authority_account_role_id] [int] IDENTITY(1,1) NOT NULL,
	[authority_role_id] [int] NOT NULL,
	[authority_account_id] [int] NOT NULL,
 CONSTRAINT [PK_authority_account_role] PRIMARY KEY CLUSTERED 
(
	[authority_account_role_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[authority_account_role] ON
INSERT [dbo].[authority_account_role] ([authority_account_role_id], [authority_role_id], [authority_account_id]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[authority_account_role] OFF
/****** Object:  Table [dbo].[authority_account]    Script Date: 12/04/2016 20:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[authority_account](
	[authority_account_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](40) NOT NULL,
	[real_name] [varchar](40) NULL,
	[password] [varchar](40) NOT NULL,
	[create_time] [datetime] NOT NULL,
	[enable] [bit] NOT NULL,
 CONSTRAINT [PK_AUTHORITY_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[authority_account_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[authority_account] ON
INSERT [dbo].[authority_account] ([authority_account_id], [name], [real_name], [password], [create_time], [enable]) VALUES (1, N'admin', N'系统管理员', N'48D084B0ADB080DA', getdate(), 1)
SET IDENTITY_INSERT [dbo].[authority_account] OFF

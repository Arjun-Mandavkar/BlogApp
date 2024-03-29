USE [master]
GO
/****** Object:  Database [BlogApp]    Script Date: 05-02-2023 15:54:53 ******/
CREATE DATABASE [BlogApp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BlogApp', FILENAME = N'C:\Users\ArjunMandavkar\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\BlogApp.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BlogApp_log', FILENAME = N'C:\Users\ArjunMandavkar\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\BlogApp.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BlogApp] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BlogApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BlogApp] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [BlogApp] SET ANSI_NULLS ON 
GO
ALTER DATABASE [BlogApp] SET ANSI_PADDING ON 
GO
ALTER DATABASE [BlogApp] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [BlogApp] SET ARITHABORT ON 
GO
ALTER DATABASE [BlogApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BlogApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BlogApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BlogApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BlogApp] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [BlogApp] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [BlogApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BlogApp] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [BlogApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BlogApp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BlogApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BlogApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BlogApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BlogApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BlogApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BlogApp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BlogApp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BlogApp] SET RECOVERY FULL 
GO
ALTER DATABASE [BlogApp] SET  MULTI_USER 
GO
ALTER DATABASE [BlogApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BlogApp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BlogApp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BlogApp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BlogApp] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BlogApp] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BlogApp] SET QUERY_STORE = OFF
GO
USE [BlogApp]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;
GO
USE [BlogApp]
GO
/****** Object:  Table [dbo].[BlogComments]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogComments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BlogId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Text] [nvarchar](512) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[isUserExists] [bit] NOT NULL,
 CONSTRAINT [PK_BlogComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogEditors]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogEditors](
	[UserId] [int] NOT NULL,
	[BlogId] [int] NOT NULL,
 CONSTRAINT [PK_BlogEditors] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[BlogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogLikes]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogLikes](
	[UserId] [int] NOT NULL,
	[BlogId] [int] NOT NULL,
 CONSTRAINT [PK_BlogLikes] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[BlogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogOwners]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogOwners](
	[UserId] [int] NOT NULL,
	[BlogId] [int] NOT NULL,
	[OwnerName] [nvarchar](50) NOT NULL,
	[IsOwnerExists] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blogs]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Content] [nvarchar](2048) NOT NULL,
	[Likes] [int] NOT NULL,
 CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05-02-2023 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[RoleId] [int] NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [BlogOwnerIndex]    Script Date: 05-02-2023 15:54:53 ******/
CREATE NONCLUSTERED INDEX [BlogOwnerIndex] ON [dbo].[BlogOwners]
(
	[BlogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 05-02-2023 15:54:53 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[Roles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 05-02-2023 15:54:53 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] WITH NOCHECK ADD  CONSTRAINT [FK_Users_Roles_Id] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles]([Id])
GO
ALTER TABLE [dbo].[BlogComments]  WITH CHECK ADD  CONSTRAINT [FK_BlogComments_Blogs_BlogId] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogComments] CHECK CONSTRAINT [FK_BlogComments_Blogs_BlogId]
GO
ALTER TABLE [dbo].[BlogComments]  WITH NOCHECK ADD  CONSTRAINT [FK_BlogComments_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BlogComments] NOCHECK CONSTRAINT [FK_BlogComments_Users_UserId]
GO
ALTER TABLE [dbo].[BlogEditors]  WITH CHECK ADD  CONSTRAINT [FK_BlogEditors_Blogs_BlogId] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogEditors] CHECK CONSTRAINT [FK_BlogEditors_Blogs_BlogId]
GO
ALTER TABLE [dbo].[BlogEditors]  WITH CHECK ADD  CONSTRAINT [FK_BlogEditors_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogEditors] CHECK CONSTRAINT [FK_BlogEditors_Users_UserId]
GO
ALTER TABLE [dbo].[BlogLikes]  WITH CHECK ADD  CONSTRAINT [FK_BlogLikes_Blogs_BlogId] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogLikes] CHECK CONSTRAINT [FK_BlogLikes_Blogs_BlogId]
GO
ALTER TABLE [dbo].[BlogLikes]  WITH CHECK ADD  CONSTRAINT [FK_BlogLikes_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BlogLikes] CHECK CONSTRAINT [FK_BlogLikes_Users_UserId]
GO
ALTER TABLE [dbo].[BlogOwners]  WITH CHECK ADD  CONSTRAINT [FK_BlogOwners_Blogs_BlogId] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blogs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogOwners] CHECK CONSTRAINT [FK_BlogOwners_Blogs_BlogId]
GO
ALTER TABLE [dbo].[BlogOwners]  WITH NOCHECK ADD  CONSTRAINT [FK_BlogOwners_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[BlogOwners] NOCHECK CONSTRAINT [FK_BlogOwners_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [BlogApp] SET  READ_WRITE 
GO

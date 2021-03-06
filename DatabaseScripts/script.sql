USE [master]
GO
/****** Object:  Database [InnovationPortal]    Script Date: 1/2/2020 2:06:56 PM ******/
CREATE DATABASE [InnovationPortal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InnovationPortal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\InnovationPortal.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InnovationPortal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\InnovationPortal_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [InnovationPortal] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InnovationPortal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InnovationPortal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InnovationPortal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InnovationPortal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InnovationPortal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InnovationPortal] SET ARITHABORT OFF 
GO
ALTER DATABASE [InnovationPortal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [InnovationPortal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InnovationPortal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InnovationPortal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InnovationPortal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InnovationPortal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InnovationPortal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InnovationPortal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InnovationPortal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InnovationPortal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [InnovationPortal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InnovationPortal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InnovationPortal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InnovationPortal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InnovationPortal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InnovationPortal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InnovationPortal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InnovationPortal] SET RECOVERY FULL 
GO
ALTER DATABASE [InnovationPortal] SET  MULTI_USER 
GO
ALTER DATABASE [InnovationPortal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InnovationPortal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InnovationPortal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InnovationPortal] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InnovationPortal] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'InnovationPortal', N'ON'
GO
ALTER DATABASE [InnovationPortal] SET QUERY_STORE = OFF
GO
USE [InnovationPortal]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [InnovationPortal]
GO
/****** Object:  Table [dbo].[AdmSettings]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdmSettings](
	[ParamName] [varchar](50) NOT NULL,
	[ParamType] [varchar](50) NOT NULL,
	[StringValue] [varchar](512) NULL,
	[Version] [varchar](15) NOT NULL,
	[NumValue] [decimal](18, 4) NULL,
	[DateValue] [datetime] NULL,
	[UpdateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParamName] ASC,
	[ParamType] ASC,
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ADMSettingsbk]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADMSettingsbk](
	[ParamName] [varchar](50) NOT NULL,
	[ParamType] [varchar](50) NOT NULL,
	[StringValue] [varchar](512) NULL,
	[Version] [varchar](15) NOT NULL,
	[NumValue] [decimal](18, 4) NULL,
	[DateValue] [datetime] NULL,
	[UpdateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdeaAttachment]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdeaAttachment](
	[IdeaAttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[IdeaID] [int] NOT NULL,
	[AttachedFileName] [nvarchar](500) NOT NULL,
	[FileExtention] [char](100) NOT NULL,
	[FileSizeInByte] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdeaAttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdeaCategories]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdeaCategories](
	[IdeaCategorieID] [int] IDENTITY(1,1) NOT NULL,
	[CategoriesName] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[AddedByUserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdeaCategorieID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdeaComment]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdeaComment](
	[IdeaCommentId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaID] [int] NOT NULL,
	[CommentDescription] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CommentByUserid] [int] NOT NULL,
 CONSTRAINT [PK__IdeaComm__7C34277BF04E8688] PRIMARY KEY CLUSTERED 
(
	[IdeaCommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ideas]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ideas](
	[IdeaID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](800) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsAttachment] [bit] NOT NULL,
	[IsSensitive] [bit] NOT NULL,
	[AttachmentCount] [tinyint] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK__Ideas] PRIMARY KEY CLUSTERED 
(
	[IdeaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdeaState]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdeaState](
	[IdeastatusID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[AddedByUserId] [int] NOT NULL,
 CONSTRAINT [PK__IdeaState] PRIMARY KEY CLUSTERED 
(
	[IdeastatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdeaStatusHistoryLog]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdeaStatusHistoryLog](
	[IdeaStatusHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaID] [int] NOT NULL,
	[IdeastatusID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedByUserID] [int] NOT NULL,
 CONSTRAINT [PK__IdeaStatusHistoryLog] PRIMARY KEY CLUSTERED 
(
	[IdeaStatusHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMapping]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMapping](
	[RoleMappingID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK__RoleMapping] PRIMARY KEY CLUSTERED 
(
	[RoleMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](30) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[S3AttachmentContainer]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[S3AttachmentContainer](
	[S3AttachmentContainerId] [int] IDENTITY(1,1) NOT NULL,
	[IdeaID] [int] NOT NULL,
	[AttachedFileName] [nvarchar](500) NOT NULL,
	[FolderName] [char](100) NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__S3Attach__0F1824A47312FBCB] PRIMARY KEY CLUSTERED 
(
	[S3AttachmentContainerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAuthentication]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAuthentication](
	[UserAuthenticationID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TokenMD5] [char](32) NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[LanguageCode] [char](2) NULL,
	[CountryCode] [char](2) NOT NULL,
	[IsHPID] [bit] NULL,
	[ClientApplication] [varchar](32) NULL,
	[ClientVersion] [varchar](64) NULL,
	[ClientPlatform] [varchar](16) NULL,
	[CallerId] [varchar](50) NOT NULL,
	[ClientId] [varchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[UseCaseGroup] [varchar](50) NULL,
	[LogoutDate] [datetime] NULL,
 CONSTRAINT [PK__UserAuthentication] PRIMARY KEY CLUSTERED 
(
	[UserAuthenticationID] ASC,
	[TokenMD5] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/2/2020 2:06:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [varchar](100) NULL,
	[SessionToken] [nvarchar](max) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[ProfileId] [varchar](50) NULL,
	[IsacId] [int] NULL,
	[IsSynchronized] [bit] NULL,
	[ActiveHealth] [bit] NULL,
	[HPIDprofileId] [varchar](50) NULL,
	[PrimaryUse] [char](7) NULL,
	[CompanyName] [varchar](256) NULL,
	[EmailConsent] [bit] NULL,
	[RefreshTokenExpireIn] [datetime] NULL,
	[RefreshTokenType] [int] NULL,
	[SmartFriend] [bit] NULL,
	[OrigClientId] [varchar](max) NULL,
	[Locale] [nvarchar](50) NULL,
 CONSTRAINT [PK__Users__1788CC4C47697D3C] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AlertMessageBaseUrl', N'AppSetting', N'https://content.methone.hpcloud.hp.net/messages', N'', NULL, NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AllowCaseCreationWithoutContactInfo', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-12-11T12:32:03.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidApplicationARN', N'AppSetting', N'arn:aws:sns:us-west-2:638179059512:app/GCM/SanctuaryAndroidProd', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidApplicationARN', N'TPAMonitoring', N'arn:aws:sns:us-west-2:638179059512:app/GCM/SanctuaryAndroidProd', N'', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidLatestAppDownloadLocation', N'AppSetting', N'https://rink.hockeyapp.net/apps/80f2ef58a4d6434db1dc65d074f8781a/app_versions/6', N'', NULL, NULL, CAST(N'2018-09-10T10:22:19.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidLatestAppVersion', N'AppSettings', N'', N'', NULL, NULL, CAST(N'2018-12-05T13:02:53.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'APIActionsListFilePath', N'AppSetting', N'C:\Resources\APIActions\APIActions.txt', N'V3', NULL, NULL, CAST(N'2019-06-25T08:47:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'APIActionsListFilePath', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v4\APIActions.txt', N'V4', NULL, NULL, CAST(N'2019-06-17T19:56:24.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AppleApplicationARN', N'AppSetting', N'arn:aws:sns:us-west-2:638179059512:app/APNS/SanctuaryIOSProduction', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AppleApplicationARN', N'TPAMonitoring', N'arn:aws:sns:us-west-2:638179059512:app/APNS/SanctuaryIOSProduction', N'', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ApplicationFeatureRedirectorURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AuthCache', N'AppSetting', N'statedata-cache.yqbh5a.0001.usw2.cache.amazonaws.com:3306', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSBucketName', N'GNProcessor', N'global-newton-trns', N'', NULL, NULL, CAST(N'2019-02-26T14:32:44.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSDestinationFolderNameInsideBucket', N'GNProcessor', N'processed-pro/', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSKey', N'AppSetting', N'AKIAZJFTPU44KQEMKO3Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSSecret', N'AppSetting', N'xUU/KCBk+clnZHy8jcBoul8vnHehq0PLFvMQWdMf', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSSourceFolderNameInsideBucket', N'GNProcessor', N'data-pro/', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSTPAKey', N'AppSetting', N'AKIAJGFZ25BZQDCUJFBQ', N'', NULL, NULL, CAST(N'2018-09-18T12:59:13.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSTPASecret', N'AppSetting', N'Ik54FwO3a3snSfwftq6RW1chlovplOpI/ZNUgcne', N'', NULL, NULL, CAST(N'2018-09-18T12:59:13.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'BucketName', N'RedShiftWriter', N'redshiftwriter-pro', N'', NULL, NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaptchaEnabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaseBucketName', N'AppSetting', N'global-newton-trns', N'', NULL, NULL, CAST(N'2019-03-06T10:56:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaseKeyName', N'AppSetting', N'data-pro/NotifTest_', N'', NULL, NULL, CAST(N'2019-03-06T10:56:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSUrl', N'AppSetting', N'https://ccls.external.hp.com/helpandsupport/ContactCenters/ContactCenters.svc/GetWorkingHours', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSUseCase', N'AppSetting', N'HPSA8', N'', NULL, NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CertificateSubject', N'AppSetting', N'CN=api.methone.hpcloud.hp.net, OU=CS, O=HP Inc, L=Palo Alto, S=California, C=US', N'', NULL, NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CheckInterval', N'TPAMonitoring', N'', N'', CAST(1.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CheckSumKey', N'ResourceLoader', N'8A21D1746EDD550E653C526AABC7D7CF', N'', NULL, NULL, CAST(N'2019-10-08T08:41:36.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ClientSettingsProvider.ServiceUri', N'GNProcessor', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ClientSettingsProvider.ServiceUri', N'TPAMonitoring', N'', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMContextName', N'AppSetting', N'SvcCoverage', N'', NULL, NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMTimeout', N'AppSetting', N'', N'', CAST(15.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-04-23T13:03:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMWebServiceUrl', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/CarePacksEOL/CrestInfo.svc/GetCarePackInfoListForProduct', N'', NULL, NULL, CAST(N'2019-02-26T14:33:01.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionPwd', N'AppSetting', N'init$X1P', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionURL', N'AppSetting', N'https://pi-prd-01.sapnet.hpicorp.net:65101/RESTAdapter/v1/secondaryserialnumber', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionUsername', N'AppSetting', N'OTMHTTPUSER', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CPCRedirectorURL', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/Common/cpcRedirectorPage.asp', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CustomerB2BName', N'AppSetting', N'EventServicesWeb_UAT_C', N'', NULL, NULL, CAST(N'2019-02-26T14:33:19.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CustomerB2BNameExt', N'AppSetting', N'AMS_HPSA_C', N'', NULL, NULL, CAST(N'2019-02-26T14:33:20.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'RedShiftWriter', N'', N'RSW', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'TPAMonitoring', N'', N'TPA', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DebugLogging', N'TPAMonitoring', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DefaultClients', N'ResourceLoader', N'Mobile,PC,Web', N'', NULL, NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DefaultImageBaseURL', N'AppSetting', N'https://content-itg.methone.hpcloud.hp.net/productimages/', N'', NULL, NULL, CAST(N'2019-12-11T12:32:07.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\AddDeviceEmailTemplate.html', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate_EN', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\AddDeviceEmailTemplate.html', N'', NULL, NULL, CAST(N'2019-10-07T10:54:55.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate_ES', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v4\AddDeviceEmailTemplate_es-ES.html', N'', NULL, NULL, CAST(N'2019-10-07T17:58:40.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DisablePCBCache', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:36.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DoNotRemoveSANFilesFromS3', N'RedShiftWriter', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DowloadedResourceFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\configFromTFS.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DowloadedSKUListCreatorFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\SKU_list_Creator_9.0.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DownloadedFolderPath', N'GNProcessor', N'E:\Resources\MethoneServices-PRO\GNProcessor\GNProcessorFiles', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DROIDApplicationID', N'AppSetting', N'hpi-obligation-int-droid', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DROIDApplicationKey', N'AppSetting', N'K!x5&R2l$dF4', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASAPIKey', N'AppSetting', N'iIwWYCtKb7aRxipjcnRgX28SaKgqZ27X4LoFZp4Y', N'', NULL, NULL, CAST(N'2019-06-19T13:51:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASDeafaultPurchaseCountryCode', N'AppSetting', N'US', N'', NULL, NULL, CAST(N'2019-02-26T14:33:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASUrl', N'AppSetting', N'https://eas-api.hpcloud.hp.com/v1', N'', NULL, NULL, CAST(N'2019-06-21T08:38:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASUseCountryCodeFromProfile', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableBadToGoodStateNotification', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-12-11T12:32:07.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableEagerDatabaseAccess', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableExtServiceLog', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnablePushNotificationFromSetStorage', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-09-12T06:09:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableServiceOperationsCache', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableSNRValidationForCases', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-12-11T12:32:08.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableSNSFailover', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GenesisTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-11T12:32:08.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GetStateDataAllowedDomains', N'AppSetting', N'*.hp.com,localhost', N'', NULL, NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubAuthToken', N'ResourceLoader', N'69f0b652841f0b6d61c4adfc170683a6f2133167', N'', NULL, NULL, CAST(N'2018-09-24T10:51:05.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubSKUUrl', N'ResourceLoader', N'https://raw.github.azc.ext.hp.com/supportsolutions/tools/master/config_file/pro/SKU_list_Creator_9.0.xlsm?token={0}', N'', NULL, NULL, CAST(N'2019-02-26T14:33:39.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubUrl', N'ResourceLoader', N'https://raw.github.azc.ext.hp.com/supportsolutions/tools/master/config_file/pro/config%20file_9.0.xlsm?token={0}', N'', NULL, NULL, CAST(N'2019-02-26T14:33:39.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HasBackendSupportToHPSAAlerts', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-06-05T11:57:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesLcCcMapping_ES_MX', N'AppSetting', N'es-ES', N'', NULL, NULL, CAST(N'2019-10-07T10:51:46.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesUrl', N'AppSetting', N'https://hermesws.ext.hp.com/HermesWS/productcontent', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKey', N'AppSetting', N'UyN6Xmls0lo89sEyNTQCGC6KcmI7lA4W', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKeyAUTH', N'AppSetting', N'frH7HVcGAg4rAFXKfnr9VtgEirpKkdqw', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKeyAUTH_hpsa9', N'AppSetting', N'G9WZBnKGJQsuxZ1APjxLqFe5sy2acVB8', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecret', N'AppSetting', N'Y08lcDhNxVcN4kcK0Z6GmqclsftfEdGG', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecretAUTH', N'AppSetting', N'omVxk8gzNsLOAucRpR211maeSbuhW6dO', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecretAUTH_hpsa9', N'AppSetting', N'YA57Nrs0IVrWs38ZsJZJcyM2ZpawGpaq', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDCreateProfileTimeout', N'AppSetting', N'', N'', CAST(14.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:36.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDEnabled', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDGetTokenTimeout', N'AppSetting', N'', N'', CAST(14.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDNewUserUrl', N'AppSetting', N'https://directory.stg.cd.id.hp.com/directory/v1/scim/v2/Users', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDOAuthUrl', N'AppSetting', N'https://directory.stg.cd.id.hp.com/directory/v1/oauth/token', N'', NULL, NULL, CAST(N'2019-02-26T14:33:22.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDProfileUrl', N'AppSetting', N'https://directory.id.hp.com/directory/v1/scim/v2/Me', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDRedirectURI', N'AppSetting', N'https://content-itg.methone.hpcloud.hp.net/profile/index9.html', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPAppId', N'AppSetting', N'200760_HPSA_PRO', N'', NULL, NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPPassword', N'AppSetting', N'HPSA@2017', N'', NULL, NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPResetPasswordLink', N'AppSetting', N'https://hpp12.passport.hp.com/hppcf/resetpwdpublic.do?lang=en&cc=us&s_level=1&email=$email$&hpappid=hppcf&guid=$guid$&userid=$userId$', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPResetPasswordMailTo', N'AppSetting', N'hp.passport@hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPTimeout', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:55.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPUserName', N'AppSetting', N'200760_HPSA_PROD@HP.COM', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSAQueueURL', N'TPAMonitoring', N'https://sqs.us-west-2.amazonaws.com/638179059512/MethoneServicesQueue', N'', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSFApplicationID', N'AppSetting', N'hpi-obligation-int-hpsa', N'', NULL, NULL, CAST(N'2019-02-26T14:33:12.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSFApplicationKey', N'AppSetting', N'J6y7$Pw!3&nH', N'', NULL, NULL, CAST(N'2019-02-26T14:33:12.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSSTranslationTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:34.000' AS DateTime))
GO
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPWCApplicationID', N'AppSetting', N'hpi-obligation-int-hpwc', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPWCApplicationKey', N'AppSetting', N'D4t&3fQ$p%9N', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IncludeTPAChanges', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:11.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IosLatestAppDownloadLocation', N'AppSettings', N'https://rink.hockeyapp.net/apps/67e8d1e290dd4296891676385521e8de/app_versions/6', N'', NULL, NULL, CAST(N'2018-09-10T10:22:20.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IosLatestAppVersion', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2018-12-05T13:02:53.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacAppId', N'AppSetting', N'220', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacPassword', N'AppSetting', N'HPSA!@#$HUJIKO', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacUserId', N'AppSetting', N'HPSA', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacCertificateFile', N'AppSetting', N'C:\HP\certificate.pfx', N'', NULL, NULL, CAST(N'2019-02-26T14:33:38.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacCertificatePassword', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:38.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacTimeout', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:55.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacUrl', N'AppSetting', N'https://bw-prd1-llb24-a1-aa-443t4318.itcs.hpicorp.net/registrations/services/CustProdRegistrations.serviceagent/PortTypeEndpoint1', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsPairedDetectionMode', N'AppSetting', N'', N'', CAST(2.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsTruncateResource', N'ResourceLoader', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSClientID', N'AppSetting', N'zpWrvdveVx1rhrsVwyhj195QAm6W6GtG', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSClientSecret', N'AppSetting', N'pPGxTZngMogYuftR', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSTokenURL', N'AppSetting', N'https://css.api.hp.com/oauth/accesstoken?grant_type=client_credentials', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSURL', N'AppSetting', N'https://css.api.hp.com/knowledge/v1/solutions?cli=HELJ-CSR', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisId', N'AppSetting', N'AKIAZJFTPU44KQEMKO3Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisSecret', N'AppSetting', N'xUU/KCBk+clnZHy8jcBoul8vnHehq0PLFvMQWdMf', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisStreamName', N'AppSetting', N'sanctuary-stream', N'', NULL, NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisStreamSize', N'AppSetting', N'', N'', CAST(4.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LegacyAPI', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LocalProcessURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LogAPICalls', N'AppSetting', N'Faults', N'', NULL, NULL, CAST(N'2019-05-14T13:17:09.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LogFilename', N'TPAMonitoring', N'D:\Logs\MethoneServices-PRO\TPAMonitoring\TPAMonitoring.log', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LoggingMode', N'AppSetting', N'', N'', CAST(0.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MaxSessionTimeMinutes', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-11T12:32:09.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MessagesUseCaseGroupsFile', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\MessagesUseCaseGroup.csv', N'', NULL, NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MinMobileDeviceTokenLength', N'AppSetting', N'', N'', CAST(64.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NewRESTAPI', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NotificationIdKeysForEasFailureId', N'TPAMonitoring', N'BatFailureNoWarr, BatFailureWarr, HDDFailureWarr, HDDFailureNoWarr', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NPCRedirectorURL', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/Common/npcRedirectorPage.asp', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationURL', N'AppSetting', N'https://obligation-int.corp.hpicorp.net/obligation-1.1/SOAP/Obligation', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSFields', N'AppSetting', N'OBS', N'', NULL, NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSProxyURL', N'AppSetting', N'https://hpcsgen.hpcloud.hp.com/obsi/callback/interaction', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSXAPIKey', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPITimeOut', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPIToken', N'AppSetting', N'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcHAiOiJjcy1kY3NvIiwiYXV0Ijp7fX0.TEeT3NhqlT5mDUpyttG3FImLWu0iBCiBOqu0IBnt1_cwTwX4rRQPEdTSn_gz6RC8MrlbWyTjik8d-qRVy2i6dP8UXxT5zQzYN5N4EoUB76_mGsN_nHQiyiPEvPu0tOmtl4IOwY6-ILWl1josDZOposBhjqPUwMTtVJzhPqafzmhCrc52YUs0q...', N'', NULL, NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPIUrl', N'AppSetting', N'https://pcbapi.inc.hp.com', N'', NULL, NULL, CAST(N'2019-10-16T15:16:52.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBCatalogFallbackCount', N'AppSetting', N'', N'', CAST(3.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBDaysRepeatCall', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMaxHeight', N'AppSetting', N'', N'', CAST(2048.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMaxWidth', N'AppSetting', N'', N'', CAST(2048.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMinHeight', N'AppSetting', N'', N'', CAST(300.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMinWidth', N'AppSetting', N'', N'', CAST(300.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBMaxCallCounter', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-03-01T13:13:37.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PrinterInkErrorLevel', N'AppSetting', N'', N'', CAST(11.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-21T10:35:02.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PrinterInkWarningLevel', N'AppSetting', N'', N'', CAST(11.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-21T10:35:22.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProcessURLLength', N'AppSetting', N'', N'', CAST(2000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProductIdentity', N'AppSetting', N'Samsung', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProductTypeCallRefreshCount', N'AppSetting', N'', N'', CAST(50000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PushNoficationsMinTimeDifference', N'AppSetting', N'', N'V4', CAST(60.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-19T12:13:31.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'RedShiftDisabled', N'AppSetting', N'', N'', CAST(1.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-10-24T14:14:58.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourceFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\config.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'Resources9.0TranslationSet', N'AppSetting', N'Resources_9.0', N'', NULL, NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'Resources9.0TranslationSet', N'ResourceLoader', N'Resources_9.0', N'RL', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourcesDefaultIconURL', N'AppSetting', N'https://content.methone.hpcloud.hp.net/icons', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourcesReloadEnabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SANBucketName', N'RedShiftWriter', N'stanctuary-stats-itg', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SANFilesFromS3Prefix', N'RedShiftWriter', N'data', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ServiceOperationsCacheTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowOnlyOneServerIsBusyError', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowOnlyOneServerIsBusyError', N'TPAMonitoring', N'true', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'GNProcessor', N'true', N'GN', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'TPAMonitoring', N'false', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailFrom', N'AppSetting', N'HPSupportAssistantTeam@hp.com', N'', NULL, NULL, CAST(N'2018-11-09T17:02:04.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailHost', N'AppSetting', N'email-smtp.us-west-2.amazonaws.com', N'', NULL, NULL, CAST(N'2018-01-29T13:14:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailPort', N'AppSetting', N'', N'', CAST(587.0000 AS Decimal(18, 4)), NULL, CAST(N'2018-01-29T13:15:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailPswd', N'AppSetting', N'BDnQwf1JzNcTW+q70qSbZWEPX5LodhOIDYyqvO3VxVmA', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailSSL', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2018-01-29T13:15:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailUser', N'AppSetting', N'AKIAZJFTPU44I2DOIH7Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNREnableSNvsPNValidation', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-12-13T12:28:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalURL', N'AppSetting', N'https://snr-srv.glb.itcs.hp.com/wwsnr/webservices/ManufacturingInstalledBase/ManufacturingInstalledBaseService.svc', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalURL', N'AppSetting', N'https://snr-srv-int.glb.itcs.hpicorp.net/wwsnr/Webservices/ManufacturingInstalledBase/ManufacturingInstalledBaseService.svc', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:28:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRRetryCounter', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-13T12:29:01.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-13T12:29:01.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:28:58.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SoftpaqUrl', N'AppSetting', N'http://h19001.www1.hp.com/pub/softpaq', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'supportLocationsCallRefreshCount', N'AppSetting', N'', N'', CAST(50000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SynchronizeIsaacOnGetProfile', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TESTLoginEnabled', N'AppSetting', N'false', N'', NULL, CAST(N'2019-12-27T14:41:28.120' AS DateTime), CAST(N'2019-12-27T14:41:28.120' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSCacheDisabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSFallbackDisabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSProductLinkURL', N'AppSetting', N'https://h20180.www2.hp.com/apps/Nav?', N'', NULL, NULL, CAST(N'2019-09-04T21:05:57.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:34.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSURL', N'AppSetting', N'https://css.api.hp.com/taxonomy/v1.1/SupportDomain/', N'', NULL, NULL, CAST(N'2019-02-26T14:33:11.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ToEmail', N'ResourceLoader', N'supsolws@hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TPAEnabled', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TPAQueueURL', N'TPAMonitoring', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'AppSetting', N'Resources', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'GNProcessor', N'GlobalNewton', N'GN', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
GO
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'TPAMonitoring', N'Resources', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TroubleshootingDefaultIconURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:17.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ValidateSerialNumberInOS', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ValidateSerialNumberInSNR', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-03-26T08:08:47.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ViewerContentRedirectorURL', N'AppSetting', N'https://ehelpsvr.rgv.hp.com/CrestLookup/CarepackRedirector.aspx', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'VirtualAgentRedirectorURL', N'AppSetting', N'https://virtualagent.hpcloud.hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[AdmSettings] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'VirtualAgentResourceID', N'AppSetting', N'VirtualAgent', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AlertMessageBaseUrl', N'AppSetting', N'https://content.methone.hpcloud.hp.net/messages', N'', NULL, NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AllowCaseCreationWithoutContactInfo', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-12-11T12:32:03.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidApplicationARN', N'AppSetting', N'arn:aws:sns:us-west-2:638179059512:app/GCM/SanctuaryAndroidProd', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidApplicationARN', N'TPAMonitoring', N'arn:aws:sns:us-west-2:638179059512:app/GCM/SanctuaryAndroidProd', N'', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidLatestAppDownloadLocation', N'AppSetting', N'https://rink.hockeyapp.net/apps/80f2ef58a4d6434db1dc65d074f8781a/app_versions/6', N'', NULL, NULL, CAST(N'2018-09-10T10:22:19.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AndroidLatestAppVersion', N'AppSettings', N'', N'', NULL, NULL, CAST(N'2018-12-05T13:02:53.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'APIActionsListFilePath', N'AppSetting', N'C:\Resources\APIActions\APIActions.txt', N'V3', NULL, NULL, CAST(N'2019-06-25T08:47:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'APIActionsListFilePath', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v4\APIActions.txt', N'V4', NULL, NULL, CAST(N'2019-06-17T19:56:24.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AppleApplicationARN', N'AppSetting', N'arn:aws:sns:us-west-2:638179059512:app/APNS/SanctuaryIOSProduction', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AppleApplicationARN', N'TPAMonitoring', N'arn:aws:sns:us-west-2:638179059512:app/APNS/SanctuaryIOSProduction', N'', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ApplicationFeatureRedirectorURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AuthCache', N'AppSetting', N'statedata-cache.yqbh5a.0001.usw2.cache.amazonaws.com:3306', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSBucketName', N'GNProcessor', N'global-newton-trns', N'', NULL, NULL, CAST(N'2019-02-26T14:32:44.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSDestinationFolderNameInsideBucket', N'GNProcessor', N'processed-pro/', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSKey', N'AppSetting', N'AKIAZJFTPU44KQEMKO3Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSSecret', N'AppSetting', N'xUU/KCBk+clnZHy8jcBoul8vnHehq0PLFvMQWdMf', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSSourceFolderNameInsideBucket', N'GNProcessor', N'data-pro/', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSTPAKey', N'AppSetting', N'AKIAJGFZ25BZQDCUJFBQ', N'', NULL, NULL, CAST(N'2018-09-18T12:59:13.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'AWSTPASecret', N'AppSetting', N'Ik54FwO3a3snSfwftq6RW1chlovplOpI/ZNUgcne', N'', NULL, NULL, CAST(N'2018-09-18T12:59:13.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'BucketName', N'RedShiftWriter', N'redshiftwriter-pro', N'', NULL, NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaptchaEnabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaseBucketName', N'AppSetting', N'global-newton-trns', N'', NULL, NULL, CAST(N'2019-03-06T10:56:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CaseKeyName', N'AppSetting', N'data-pro/NotifTest_', N'', NULL, NULL, CAST(N'2019-03-06T10:56:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSUrl', N'AppSetting', N'https://ccls.external.hp.com/helpandsupport/ContactCenters/ContactCenters.svc/GetWorkingHours', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CCLSUseCase', N'AppSetting', N'HPSA8', N'', NULL, NULL, CAST(N'2019-02-26T14:33:16.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CertificateSubject', N'AppSetting', N'CN=api.methone.hpcloud.hp.net, OU=CS, O=HP Inc, L=Palo Alto, S=California, C=US', N'', NULL, NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CheckInterval', N'TPAMonitoring', N'', N'', CAST(1.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CheckSumKey', N'ResourceLoader', N'8A21D1746EDD550E653C526AABC7D7CF', N'', NULL, NULL, CAST(N'2019-10-08T08:41:36.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ClientSettingsProvider.ServiceUri', N'GNProcessor', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ClientSettingsProvider.ServiceUri', N'TPAMonitoring', N'', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMContextName', N'AppSetting', N'SvcCoverage', N'', NULL, NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMTimeout', N'AppSetting', N'', N'', CAST(15.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-04-23T13:03:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CLMWebServiceUrl', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/CarePacksEOL/CrestInfo.svc/GetCarePackInfoListForProduct', N'', NULL, NULL, CAST(N'2019-02-26T14:33:01.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionPwd', N'AppSetting', N'init$X1P', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionURL', N'AppSetting', N'https://pi-prd-01.sapnet.hpicorp.net:65101/RESTAdapter/v1/secondaryserialnumber', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ConversionUsername', N'AppSetting', N'OTMHTTPUSER', N'', NULL, NULL, CAST(N'2019-02-26T14:33:05.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CPCRedirectorURL', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/Common/cpcRedirectorPage.asp', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CustomerB2BName', N'AppSetting', N'EventServicesWeb_UAT_C', N'', NULL, NULL, CAST(N'2019-02-26T14:33:19.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'CustomerB2BNameExt', N'AppSetting', N'AMS_HPSA_C', N'', NULL, NULL, CAST(N'2019-02-26T14:33:20.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:02.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'RedShiftWriter', N'', N'RSW', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DatabaseRetryCounter', N'TPAMonitoring', N'', N'TPA', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DebugLogging', N'TPAMonitoring', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DefaultClients', N'ResourceLoader', N'Mobile,PC,Web', N'', NULL, NULL, CAST(N'2019-02-26T14:32:48.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DefaultImageBaseURL', N'AppSetting', N'https://content-itg.methone.hpcloud.hp.net/productimages/', N'', NULL, NULL, CAST(N'2019-12-11T12:32:07.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\AddDeviceEmailTemplate.html', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate_EN', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\AddDeviceEmailTemplate.html', N'', NULL, NULL, CAST(N'2019-10-07T10:54:55.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DeviceEmailTemplate_ES', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v4\AddDeviceEmailTemplate_es-ES.html', N'', NULL, NULL, CAST(N'2019-10-07T17:58:40.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DisablePCBCache', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:36.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DoNotRemoveSANFilesFromS3', N'RedShiftWriter', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DowloadedResourceFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\configFromTFS.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DowloadedSKUListCreatorFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\SKU_list_Creator_9.0.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DownloadedFolderPath', N'GNProcessor', N'E:\Resources\MethoneServices-PRO\GNProcessor\GNProcessorFiles', N'', NULL, NULL, CAST(N'2019-02-26T14:32:45.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DROIDApplicationID', N'AppSetting', N'hpi-obligation-int-droid', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'DROIDApplicationKey', N'AppSetting', N'K!x5&R2l$dF4', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASAPIKey', N'AppSetting', N'iIwWYCtKb7aRxipjcnRgX28SaKgqZ27X4LoFZp4Y', N'', NULL, NULL, CAST(N'2019-06-19T13:51:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASDeafaultPurchaseCountryCode', N'AppSetting', N'US', N'', NULL, NULL, CAST(N'2019-02-26T14:33:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASUrl', N'AppSetting', N'https://eas-api.hpcloud.hp.com/v1', N'', NULL, NULL, CAST(N'2019-06-21T08:38:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EASUseCountryCodeFromProfile', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableBadToGoodStateNotification', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-12-11T12:32:07.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableEagerDatabaseAccess', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableExtServiceLog', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnablePushNotificationFromSetStorage', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-09-12T06:09:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableServiceOperationsCache', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableSNRValidationForCases', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-12-11T12:32:08.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'EnableSNSFailover', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:09.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GenesisTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-11T12:32:08.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GetStateDataAllowedDomains', N'AppSetting', N'*.hp.com,localhost', N'', NULL, NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubAuthToken', N'ResourceLoader', N'69f0b652841f0b6d61c4adfc170683a6f2133167', N'', NULL, NULL, CAST(N'2018-09-24T10:51:05.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubSKUUrl', N'ResourceLoader', N'https://raw.github.azc.ext.hp.com/supportsolutions/tools/master/config_file/pro/SKU_list_Creator_9.0.xlsm?token={0}', N'', NULL, NULL, CAST(N'2019-02-26T14:33:39.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'GitHubUrl', N'ResourceLoader', N'https://raw.github.azc.ext.hp.com/supportsolutions/tools/master/config_file/pro/config%20file_9.0.xlsm?token={0}', N'', NULL, NULL, CAST(N'2019-02-26T14:33:39.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HasBackendSupportToHPSAAlerts', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-06-05T11:57:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesLcCcMapping_ES_MX', N'AppSetting', N'es-ES', N'', NULL, NULL, CAST(N'2019-10-07T10:51:46.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HermesUrl', N'AppSetting', N'https://hermesws.ext.hp.com/HermesWS/productcontent', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKey', N'AppSetting', N'UrGyLwEKBXOiEbpSzyMiMofdi06gnXVZ', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKeyAUTH', N'AppSetting', N'I4x8qjkVQczvNFKGXYAwhgxgwJXdrgSL', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiKeyAUTH_hpsa9', N'AppSetting', N'50hAjha6HrK7lQCgOOI0JjyRcqyQ15Uf', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecret', N'AppSetting', N'cT5bC8GO1RfEycZGXoBYMmQ2x6Vx8hXC', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecretAUTH', N'AppSetting', N'NKsN5IfumCSxLAux7EQb2WMWfEnPAzs9', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDApiSecretAUTH_hpsa9', N'AppSetting', N'QfvCYbr97GxL7CtXMkM5RwvSuA4IAq6E', N'', NULL, NULL, CAST(N'2019-02-26T14:33:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDCreateProfileTimeout', N'AppSetting', N'', N'', CAST(14.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:36.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDEnabled', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDGetTokenTimeout', N'AppSetting', N'', N'', CAST(14.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDNewUserUrl', N'AppSetting', N'https://directory.id.hp.com/directory/v1/scim/v2/Users', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDOAuthUrl', N'AppSetting', N'https://directory.id.hp.com/directory/v1/oauth/token', N'', NULL, NULL, CAST(N'2019-02-26T14:33:22.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDProfileUrl', N'AppSetting', N'https://directory.id.hp.com/directory/v1/scim/v2/Me', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPIDRedirectURI', N'AppSetting', N'https://content.methone.hpcloud.hp.net/profile/index9.html', N'', NULL, NULL, CAST(N'2019-02-26T14:33:24.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPAppId', N'AppSetting', N'200760_HPSA_PRO', N'', NULL, NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPPassword', N'AppSetting', N'HPSA@2017', N'', NULL, NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPResetPasswordLink', N'AppSetting', N'https://hpp12.passport.hp.com/hppcf/resetpwdpublic.do?lang=en&cc=us&s_level=1&email=$email$&hpappid=hppcf&guid=$guid$&userid=$userId$', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPResetPasswordMailTo', N'AppSetting', N'hp.passport@hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPTimeout', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:55.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPPUserName', N'AppSetting', N'200760_HPSA_PROD@HP.COM', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSAQueueURL', N'TPAMonitoring', N'https://sqs.us-west-2.amazonaws.com/638179059512/MethoneServicesQueue', N'', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSFApplicationID', N'AppSetting', N'hpi-obligation-int-hpsa', N'', NULL, NULL, CAST(N'2019-02-26T14:33:12.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSFApplicationKey', N'AppSetting', N'J6y7$Pw!3&nH', N'', NULL, NULL, CAST(N'2019-02-26T14:33:12.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPSSTranslationTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:34.000' AS DateTime))
GO
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPWCApplicationID', N'AppSetting', N'hpi-obligation-int-hpwc', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'HPWCApplicationKey', N'AppSetting', N'D4t&3fQ$p%9N', N'', NULL, NULL, CAST(N'2019-02-26T14:33:13.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IncludeTPAChanges', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:11.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IosLatestAppDownloadLocation', N'AppSettings', N'https://rink.hockeyapp.net/apps/67e8d1e290dd4296891676385521e8de/app_versions/6', N'', NULL, NULL, CAST(N'2018-09-10T10:22:20.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IosLatestAppVersion', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2018-12-05T13:02:53.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacAppId', N'AppSetting', N'220', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacPassword', N'AppSetting', N'HPSA!@#$HUJIKO', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsaacUserId', N'AppSetting', N'HPSA', N'', NULL, NULL, CAST(N'2019-02-26T14:32:56.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacCertificateFile', N'AppSetting', N'C:\HP\certificate.pfx', N'', NULL, NULL, CAST(N'2019-02-26T14:33:38.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacCertificatePassword', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:38.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacTimeout', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:55.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsacUrl', N'AppSetting', N'https://bw-prd1-llb24-a1-aa-443t4318.itcs.hpicorp.net/registrations/services/CustProdRegistrations.serviceagent/PortTypeEndpoint1', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsPairedDetectionMode', N'AppSetting', N'', N'', CAST(2.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'IsTruncateResource', N'ResourceLoader', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:32:49.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSClientID', N'AppSetting', N'zpWrvdveVx1rhrsVwyhj195QAm6W6GtG', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSClientSecret', N'AppSetting', N'pPGxTZngMogYuftR', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSTokenURL', N'AppSetting', N'https://css.api.hp.com/oauth/accesstoken?grant_type=client_credentials', N'', NULL, NULL, CAST(N'2019-02-26T14:33:07.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KaaSURL', N'AppSetting', N'https://css.api.hp.com/knowledge/v1/solutions?cli=HELJ-CSR', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisId', N'AppSetting', N'AKIAZJFTPU44KQEMKO3Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisSecret', N'AppSetting', N'xUU/KCBk+clnZHy8jcBoul8vnHehq0PLFvMQWdMf', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisStreamName', N'AppSetting', N'sanctuary-stream', N'', NULL, NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'KinesisStreamSize', N'AppSetting', N'', N'', CAST(4.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:08.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LegacyAPI', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LocalProcessURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LogAPICalls', N'AppSetting', N'Faults', N'', NULL, NULL, CAST(N'2019-05-14T13:17:09.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LogFilename', N'TPAMonitoring', N'D:\Logs\MethoneServices-PRO\TPAMonitoring\TPAMonitoring.log', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'LoggingMode', N'AppSetting', N'', N'', CAST(0.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MaxSessionTimeMinutes', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-11T12:32:09.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MessagesUseCaseGroupsFile', N'AppSetting', N'E:\Resources\MethoneServices-PRO\HPSAProfileService-v3\MessagesUseCaseGroup.csv', N'', NULL, NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'MinMobileDeviceTokenLength', N'AppSetting', N'', N'', CAST(64.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NewRESTAPI', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:06.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NotificationIdKeysForEasFailureId', N'TPAMonitoring', N'BatFailureNoWarr, BatFailureWarr, HDDFailureWarr, HDDFailureNoWarr', N'', NULL, NULL, CAST(N'2019-02-26T14:32:54.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'NPCRedirectorURL', N'AppSetting', N'https://hpsa-redirectors.hpcloud.hp.com/Common/npcRedirectorPage.asp', N'', NULL, NULL, CAST(N'2019-02-26T14:33:25.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationRetryCounter', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ObligationURL', N'AppSetting', N'https://obligation-int.corp.hpicorp.net/obligation-1.1/SOAP/Obligation', N'', NULL, NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSFields', N'AppSetting', N'OBS', N'', NULL, NULL, CAST(N'2019-02-26T14:33:33.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSProxyURL', N'AppSetting', N'https://hpcsgen.hpcloud.hp.com/obsi/callback/interaction', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'OBSXAPIKey', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:32.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPITimeOut', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPIToken', N'AppSetting', N'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcHAiOiJjcy1kY3NvIiwiYXV0Ijp7fX0.TEeT3NhqlT5mDUpyttG3FImLWu0iBCiBOqu0IBnt1_cwTwX4rRQPEdTSn_gz6RC8MrlbWyTjik8d-qRVy2i6dP8UXxT5zQzYN5N4EoUB76_mGsN_nHQiyiPEvPu0tOmtl4IOwY6-ILWl1josDZOposBhjqPUwMTtVJzhPqafzmhCrc52YUs0q...', N'', NULL, NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBAPIUrl', N'AppSetting', N'https://pcbapi.inc.hp.com', N'', NULL, NULL, CAST(N'2019-10-16T15:16:52.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBCatalogFallbackCount', N'AppSetting', N'', N'', CAST(3.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBDaysRepeatCall', N'AppSetting', N'', N'', CAST(30.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMaxHeight', N'AppSetting', N'', N'', CAST(2048.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMaxWidth', N'AppSetting', N'', N'', CAST(2048.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMinHeight', N'AppSetting', N'', N'', CAST(300.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:30.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBImageMinWidth', N'AppSetting', N'', N'', CAST(300.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:29.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PCBMaxCallCounter', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-03-01T13:13:37.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PrinterInkErrorLevel', N'AppSetting', N'', N'', CAST(11.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-21T10:35:02.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PrinterInkWarningLevel', N'AppSetting', N'', N'', CAST(11.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-21T10:35:22.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProcessURLLength', N'AppSetting', N'', N'', CAST(2000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProductIdentity', N'AppSetting', N'Samsung', N'', NULL, NULL, CAST(N'2019-02-26T14:32:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ProductTypeCallRefreshCount', N'AppSetting', N'', N'', CAST(50000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:32:58.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'PushNoficationsMinTimeDifference', N'AppSetting', N'', N'V4', CAST(60.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-06-19T12:13:31.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'RedShiftDisabled', N'AppSetting', N'', N'', CAST(1.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-10-24T14:14:58.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourceFile', N'ResourceLoader', N'E:\Resources\MethoneServices-PRO\ResourceLoader\config.xlsm', N'', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'Resources9.0TranslationSet', N'AppSetting', N'Resources_9.0', N'', NULL, NULL, CAST(N'2019-02-26T14:33:27.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'Resources9.0TranslationSet', N'ResourceLoader', N'Resources_9.0', N'RL', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourcesDefaultIconURL', N'AppSetting', N'https://content.methone.hpcloud.hp.net/icons', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ResourcesReloadEnabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SANBucketName', N'RedShiftWriter', N'stanctuary-stats-itg', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SANFilesFromS3Prefix', N'RedShiftWriter', N'data', N'', NULL, NULL, CAST(N'2019-02-26T14:32:47.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ServiceOperationsCacheTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:14.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowOnlyOneServerIsBusyError', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:03.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowOnlyOneServerIsBusyError', N'TPAMonitoring', N'true', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:52.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'GNProcessor', N'true', N'GN', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ShowStackTrace', N'TPAMonitoring', N'false', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailFrom', N'AppSetting', N'HPSupportAssistantTeam@hp.com', N'', NULL, NULL, CAST(N'2018-11-09T17:02:04.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailHost', N'AppSetting', N'email-smtp.us-west-2.amazonaws.com', N'', NULL, NULL, CAST(N'2018-01-29T13:14:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailPort', N'AppSetting', N'', N'', CAST(587.0000 AS Decimal(18, 4)), NULL, CAST(N'2018-01-29T13:15:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailPswd', N'AppSetting', N'BDnQwf1JzNcTW+q70qSbZWEPX5LodhOIDYyqvO3VxVmA', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailSSL', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2018-01-29T13:15:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SmtpMailUser', N'AppSetting', N'AKIAZJFTPU44I2DOIH7Q', N'', NULL, NULL, CAST(N'2019-12-02T08:54:23.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNREnableSNvsPNValidation', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-12-13T12:28:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalURL', N'AppSetting', N'https://snr-srv.glb.itcs.hp.com/wwsnr/webservices/ManufacturingInstalledBase/ManufacturingInstalledBaseService.svc', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRExternalUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:29:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalURL', N'AppSetting', N'https://snr-srv-int.glb.itcs.hpicorp.net/wwsnr/Webservices/ManufacturingInstalledBase/ManufacturingInstalledBaseService.svc', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRInternalUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:28:59.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRPassword', N'AppSetting', N'8q-MN+7j4Cf*', N'', NULL, NULL, CAST(N'2019-12-13T12:28:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRRetryCounter', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-13T12:29:01.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRTimeout', N'AppSetting', N'', N'', CAST(10.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-12-13T12:29:01.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SNRUserName', N'AppSetting', N'205762-HPSABackend', N'', NULL, NULL, CAST(N'2019-12-13T12:28:58.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SoftpaqUrl', N'AppSetting', N'http://h19001.www1.hp.com/pub/softpaq', N'', NULL, NULL, CAST(N'2019-02-26T14:33:00.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'supportLocationsCallRefreshCount', N'AppSetting', N'', N'', CAST(50000.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:04.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'SynchronizeIsaacOnGetProfile', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TESTLoginEnabled', N'AppSetting', N'false', N'', NULL, CAST(N'2019-12-27T14:41:28.120' AS DateTime), CAST(N'2019-12-27T14:41:28.120' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSCacheDisabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSFallbackDisabled', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:35.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSProductLinkURL', N'AppSetting', N'https://h20180.www2.hp.com/apps/Nav?', N'', NULL, NULL, CAST(N'2019-09-04T21:05:57.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSTimeout', N'AppSetting', N'', N'', CAST(5.0000 AS Decimal(18, 4)), NULL, CAST(N'2019-02-26T14:33:34.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TMSURL', N'AppSetting', N'https://css.api.hp.com/taxonomy/v1.1/SupportDomain/', N'', NULL, NULL, CAST(N'2019-02-26T14:33:11.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ToEmail', N'ResourceLoader', N'supsolws@hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:32:50.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TPAEnabled', N'AppSetting', N'true', N'', NULL, NULL, CAST(N'2019-02-26T14:33:10.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TPAQueueURL', N'TPAMonitoring', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:32:51.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'AppSetting', N'Resources', N'', NULL, NULL, CAST(N'2019-02-26T14:33:28.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'GNProcessor', N'GlobalNewton', N'GN', NULL, NULL, CAST(N'2019-02-26T14:32:46.000' AS DateTime))
GO
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TranslationDefaultResourceSet', N'TPAMonitoring', N'Resources', N'TPA', NULL, NULL, CAST(N'2019-02-26T14:32:53.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'TroubleshootingDefaultIconURL', N'AppSetting', N'', N'', NULL, NULL, CAST(N'2019-02-26T14:33:17.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ValidateSerialNumberInOS', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-02-26T14:33:37.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ValidateSerialNumberInSNR', N'AppSetting', N'false', N'', NULL, NULL, CAST(N'2019-03-26T08:08:47.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'ViewerContentRedirectorURL', N'AppSetting', N'https://ehelpsvr.rgv.hp.com/CrestLookup/CarepackRedirector.aspx', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'VirtualAgentRedirectorURL', N'AppSetting', N'https://virtualagent.hpcloud.hp.com', N'', NULL, NULL, CAST(N'2019-02-26T14:33:26.000' AS DateTime))
INSERT [dbo].[ADMSettingsbk] ([ParamName], [ParamType], [StringValue], [Version], [NumValue], [DateValue], [UpdateDate]) VALUES (N'VirtualAgentResourceID', N'AppSetting', N'VirtualAgent', N'', NULL, NULL, CAST(N'2019-02-26T14:33:15.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[RoleMapping] ON 

INSERT [dbo].[RoleMapping] ([RoleMappingID], [UserId], [RoleID], [CreatedDate]) VALUES (11, 6, 2, CAST(N'2019-12-31T11:11:05.080' AS DateTime))
SET IDENTITY_INSERT [dbo].[RoleMapping] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName], [CreatedDate], [IsActive]) VALUES (1, N'Admin', CAST(N'2019-12-23T15:29:00.690' AS DateTime), 1)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [CreatedDate], [IsActive]) VALUES (2, N'Submitter', CAST(N'2019-12-23T15:29:00.693' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[UserAuthentication] ON 

INSERT [dbo].[UserAuthentication] ([UserAuthenticationID], [UserId], [TokenMD5], [Token], [LanguageCode], [CountryCode], [IsHPID], [ClientApplication], [ClientVersion], [ClientPlatform], [CallerId], [ClientId], [CreatedDate], [ModifiedDate], [UseCaseGroup], [LogoutDate]) VALUES (33, 6, N'294918409ed53a85299fb798dabe3be9', N'eyJraWQiOiJBY2Nlc3MgVG9rZW4gU2lnbmluZyBLZXkgUGFpciBKdWx5IDIwMTkiLCJhbGciOiJSUzUxMiJ9.eyJzdWIiOiJVc2Vyc1wvYXVxYnVpZ2h4ZnJlaXNkc2FrbXo4Mzg1c3FvN2l1NGkiLCJuYmYiOjE1Nzc3OTA2MDEsInNjb3BlIjoib3BlbmlkIHVzZXIucHJvZmlsZS5yZWFkIG9mZmxpbmVfYWNjZXNzIHByb2ZpbGUgZW1haWwgdXNlci5wcm9maWxlLndyaXRlIiwiaXNzIjoiaHR0cHM6XC9cL2RpcmVjdG9yeS5pZC5ocC5jb20iLCJleHAiOjE1Nzc3OTE1NjEsImlhdCI6MTU3Nzc5MDY2MSwiY2xpZW50X2lkIjoiVXJHeUx3RUtCWE9pRWJwU3p5TWlNb2ZkaTA2Z25YVloiLCJqdGkiOiJhLkhzZ0t2dyJ9.LyWHVceI_8C_JbYD2X0tladZsBV3hb9LNeAHd5yf5bYfRqa3M5D5MSa7_b9rw_QnBm25xRi9GjomU12hWhrAjHiZGx3oRHJkEa94UNQlgPSddm5kZoh0fEzI3E5TS72xnN1eJTPJsg2TTzaByDCAVWcWEQNV5DhLweus0Kj2t1fqUXiq9vR1kPPwRFLHCGj2GZ7xW_mDP9_9FMJY2twIc1pbMD7bQr_ivgKXKxHvEKH6z0oj_1W-GAB69QA4m3ZGz8VtkbECP0MdhNelgxSVz6YuwRYxvjqhs50yah2ioyG9X6Bk0JCslDdvmyB0WHDSJv5jAPO098oouoOO9VhPHQ', N'en', N'US', 1, N'HP.InnovationPortal', NULL, N'web', N'123string', NULL, CAST(N'2019-12-31T11:11:05.090' AS DateTime), CAST(N'2019-12-31T11:11:05.090' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserAuthentication] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [EmailAddress], [SessionToken], [RefreshToken], [CreatedDate], [ModifiedDate], [IsActive], [ProfileId], [IsacId], [IsSynchronized], [ActiveHealth], [HPIDprofileId], [PrimaryUse], [CompanyName], [EmailConsent], [RefreshTokenExpireIn], [RefreshTokenType], [SmartFriend], [OrigClientId], [Locale]) VALUES (6, N'ramdeo.angh@hp.com', NULL, N'AWGh19gjTkfe4BkNCgWKH1qxUBTzAAAAAAAAAAABnqFEWB9naIdD4n-3WtmW6mmwujbmdz5KpWg-wyZmw3uJqnwY1BDnvhDRZAMMfQEpUscTfERjCgX9ZNGorr49a56xAmy8vfN6pBuzJ8xkPQ', CAST(N'2019-12-31T11:11:05.007' AS DateTime), CAST(N'2019-12-31T11:11:05.007' AS DateTime), 1, N'', 0, 0, NULL, N'auqbuighxfreisdsakmz8385sqo7iu4i', N'Item002', NULL, 0, CAST(N'2019-12-31T16:56:05.003' AS DateTime), 0, NULL, NULL, N'en_US')
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[AdmSettings] ADD  DEFAULT (NULL) FOR [StringValue]
GO
ALTER TABLE [dbo].[AdmSettings] ADD  DEFAULT ('') FOR [Version]
GO
ALTER TABLE [dbo].[AdmSettings] ADD  DEFAULT (NULL) FOR [NumValue]
GO
ALTER TABLE [dbo].[AdmSettings] ADD  DEFAULT (NULL) FOR [DateValue]
GO
ALTER TABLE [dbo].[AdmSettings] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[IdeaAttachment]  WITH CHECK ADD  CONSTRAINT [FK_IdeaAttachment_Ideas_IdeaID] FOREIGN KEY([IdeaID])
REFERENCES [dbo].[Ideas] ([IdeaID])
GO
ALTER TABLE [dbo].[IdeaAttachment] CHECK CONSTRAINT [FK_IdeaAttachment_Ideas_IdeaID]
GO
ALTER TABLE [dbo].[IdeaCategories]  WITH CHECK ADD  CONSTRAINT [FK_IdeaCategories_Users_UserId] FOREIGN KEY([AddedByUserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[IdeaCategories] CHECK CONSTRAINT [FK_IdeaCategories_Users_UserId]
GO
ALTER TABLE [dbo].[IdeaComment]  WITH CHECK ADD  CONSTRAINT [FK_IdeaComment_Ideas_IdeaID] FOREIGN KEY([IdeaID])
REFERENCES [dbo].[Ideas] ([IdeaID])
GO
ALTER TABLE [dbo].[IdeaComment] CHECK CONSTRAINT [FK_IdeaComment_Ideas_IdeaID]
GO
ALTER TABLE [dbo].[IdeaComment]  WITH CHECK ADD  CONSTRAINT [FK_IdeaComment_Users_CommentByUserid] FOREIGN KEY([CommentByUserid])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[IdeaComment] CHECK CONSTRAINT [FK_IdeaComment_Users_CommentByUserid]
GO
ALTER TABLE [dbo].[Ideas]  WITH CHECK ADD  CONSTRAINT [FK_Ideas_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Ideas] CHECK CONSTRAINT [FK_Ideas_Users_UserId]
GO
ALTER TABLE [dbo].[IdeaState]  WITH CHECK ADD  CONSTRAINT [FK_IdeaState_Users_UserId] FOREIGN KEY([AddedByUserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[IdeaState] CHECK CONSTRAINT [FK_IdeaState_Users_UserId]
GO
ALTER TABLE [dbo].[IdeaStatusHistoryLog]  WITH CHECK ADD  CONSTRAINT [FK_IdeaStatusHistoryLog_Ideas_IdeaID] FOREIGN KEY([IdeaID])
REFERENCES [dbo].[Ideas] ([IdeaID])
GO
ALTER TABLE [dbo].[IdeaStatusHistoryLog] CHECK CONSTRAINT [FK_IdeaStatusHistoryLog_Ideas_IdeaID]
GO
ALTER TABLE [dbo].[IdeaStatusHistoryLog]  WITH CHECK ADD  CONSTRAINT [FK_IdeaStatusHistoryLog_Users_ModifiedByUserID] FOREIGN KEY([ModifiedByUserID])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[IdeaStatusHistoryLog] CHECK CONSTRAINT [FK_IdeaStatusHistoryLog_Users_ModifiedByUserID]
GO
ALTER TABLE [dbo].[RoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_RoleMapping_Roles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[RoleMapping] CHECK CONSTRAINT [FK_RoleMapping_Roles_RoleID]
GO
ALTER TABLE [dbo].[RoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_RoleMapping_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[RoleMapping] CHECK CONSTRAINT [FK_RoleMapping_Users_UserId]
GO
ALTER TABLE [dbo].[S3AttachmentContainer]  WITH CHECK ADD  CONSTRAINT [FK_S3AttachmentContainer_Ideas_IdeaID] FOREIGN KEY([IdeaID])
REFERENCES [dbo].[Ideas] ([IdeaID])
GO
ALTER TABLE [dbo].[S3AttachmentContainer] CHECK CONSTRAINT [FK_S3AttachmentContainer_Ideas_IdeaID]
GO
ALTER TABLE [dbo].[UserAuthentication]  WITH CHECK ADD  CONSTRAINT [FK_UserAuthentication_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserAuthentication] CHECK CONSTRAINT [FK_UserAuthentication_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [InnovationPortal] SET  READ_WRITE 
GO

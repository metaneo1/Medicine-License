USE [master]
GO
/****** Object:  Database [Medlicense]    Script Date: 23.05.2020 22:04:17 ******/
CREATE DATABASE [Medlicense]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Medlicense', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SERVER2014\MSSQL\DATA\Medlicense.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Medlicense_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SERVER2014\MSSQL\DATA\Medlicense_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Medlicense] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Medlicense].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Medlicense] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Medlicense] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Medlicense] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Medlicense] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Medlicense] SET ARITHABORT OFF 
GO
ALTER DATABASE [Medlicense] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Medlicense] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Medlicense] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Medlicense] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Medlicense] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Medlicense] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Medlicense] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Medlicense] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Medlicense] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Medlicense] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Medlicense] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Medlicense] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Medlicense] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Medlicense] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Medlicense] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Medlicense] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Medlicense] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Medlicense] SET RECOVERY FULL 
GO
ALTER DATABASE [Medlicense] SET  MULTI_USER 
GO
ALTER DATABASE [Medlicense] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Medlicense] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Medlicense] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Medlicense] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Medlicense] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Medlicense', N'ON'
GO
USE [Medlicense]
GO
/****** Object:  UserDefinedDataType [dbo].[Identical]    Script Date: 23.05.2020 22:04:18 ******/
CREATE TYPE [dbo].[Identical] FROM [int] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[IdenticalChild]    Script Date: 23.05.2020 22:04:18 ******/
CREATE TYPE [dbo].[IdenticalChild] FROM [int] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[IdenticalParent]    Script Date: 23.05.2020 22:04:18 ******/
CREATE TYPE [dbo].[IdenticalParent] FROM [int] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Logical]    Script Date: 23.05.2020 22:04:18 ******/
CREATE TYPE [dbo].[Logical] FROM [bit] NULL
GO
/****** Object:  UserDefinedFunction [dbo].[GetDocumentAndRequestElements]    Script Date: 23.05.2020 22:04:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[GetDocumentAndRequestElements](@Id_RequestType int, @Id_LicenseRequest int)
returns @T table(DocumentStateName nvarchar(max)
      ,RequestElementTypeId int
      ,RequestElementTypeName nvarchar(max) 
      ,[Id] int
      ,[Id_Request] int
      ,[Id_Document] int
      ,[Id_DocumentState] int
      ,[Id_RequestElement] nvarchar(max)
	  ,[Id_ParentRequestElement] nvarchar(max)
	  ,[IsActive] bit
      ,[Description]nvarchar(max)
      ,[HasDocument] bit
      ,[RowNum] int ,primary key (RowNum)) 
as
begin

 with tree as (
    SELECT        
        Id, 
        Id_Parent,
		IsActive,
        0 AS LEVEL
    FROM RequestElement
    WHERE Id_Parent is null
  
    UNION ALL
  
    SELECT        
        rn.Id, 
        rn.Id_Parent, 
		rn.IsActive,
        LEVEL + 1
    FROM            RequestElement rn 
    INNER JOIN    tree ON tree.Id = rn.Id_Parent
  )

insert into @T


  select 
  ds.Name_ru AS DocumentStateName, ret.Id AS RequestElementTypeId, ret.Name_ru AS RequestElementTypeName, dir.Id,
dir.Id_Request as Id_Request, dir.Id_Document, 
                         dir.Id_DocumentState, case 
      when dir.Id is null then cast (re.Id as nvarchar(max))
      when dir.Id is not null then cast (re.Id as nvarchar(max)) + '_' + cast (dir.Id as nvarchar(max))
    end as Id_RequestElement, re.Id_Parent as Id_ParentRequestElement,
					re.IsActive,	 dir.Description, cast(CASE WHEN dir.Id IS NULL THEN 0 ELSE 1 END as bit)AS HasDocument, RowNum = ROW_NUMBER() OVER (ORDER BY dir.Id)
  from tree t
  inner join RequestElement re on re.Id = t.Id
  left join DocumentInRequest dir on dir.Id_RequestElement = t.Id  and dir.Id_Request = @Id_LicenseRequest
  LEFT OUTER JOIN
                         dbo.RequestElementType AS ret ON ret.Id = re.Id_RequestElemType LEFT OUTER JOIN
                         dbo.DocumentState AS ds ON ds.Id = dir.Id_DocumentState
WHERE        (re.Id_RequestType = @Id_RequestType) 


  return
end

GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 23.05.2020 22:04:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ActivityType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÂèä_äåÿòåëüíîñòè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdminUnits]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUnits](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[ParentId] [dbo].[Identical] NULL,
	[IdTypeadm] [dbo].[Identical] NOT NULL,
	[Latitude] [decimal](12, 10) NULL,
	[Longitude] [decimal](12, 10) NULL,
	[Comment] [nvarchar](max) NULL,
	[IsRayonCenter] [dbo].[Logical] NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÒåððèòîðèàëüíàÿ_Åäèíèöà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUser]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUser](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[phone1] [nvarchar](max) NULL,
	[phone2] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[DOB] [datetime] NULL,
	[Id_Sex] [dbo].[IdenticalChild] NOT NULL,
	[IsActive] [dbo].[Logical] NULL,
 CONSTRAINT [XPKÏîëüçîâàòåëü] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[User_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[Id_ApplicationUser] [int] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocTemplForReqElemType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocTemplForReqElemType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
	[Id_Document] [dbo].[IdenticalChild] NOT NULL,
	[Id_RequestElementType] [dbo].[IdenticalParent] NOT NULL,
 CONSTRAINT [XPKØàáëîíû_äîêóìåíòîâ_çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Document]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[Id] [dbo].[IdenticalChild] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Filename] [nvarchar](max) NULL,
	[PathToFile] [nvarchar](max) NULL,
	[Id_DocumentFormat] [dbo].[IdenticalChild] NOT NULL,
 CONSTRAINT [XPKÄîêóìåíò] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Document_Format]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document_Format](
	[Id] [dbo].[IdenticalChild] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÔîðìàò_Äîêóìåíòà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocumentInRequest]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentInRequest](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_Request] [dbo].[IdenticalParent] NOT NULL,
	[Id_Document] [dbo].[IdenticalChild] NOT NULL,
	[Id_DocumentState] [dbo].[IdenticalParent] NOT NULL,
	[Id_RequestElement] [dbo].[Identical] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [XPKÏîäàâàåìûé_äîêóìåíò] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocumentState]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentState](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÑòàòóñ_Äîêóìåíòà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LicenseRequest]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseRequest](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[DateCreate] [datetime] NULL,
	[ClinicName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Phone1] [nvarchar](max) NULL,
	[Phone2] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Id_Company] [dbo].[IdenticalParent] NULL,
	[Id_AdminUnit] [dbo].[Identical] NULL,
	[Id_RequestType] [dbo].[IdenticalParent] NULL,
	[Id_User] [dbo].[IdenticalParent] NOT NULL,
	[IsDraft] [bit] NOT NULL,
 CONSTRAINT [XPKÇàÿâêà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LicenseRequestActivityType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseRequestActivityType](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_Type] [dbo].[IdenticalParent] NOT NULL,
	[Id_Request] [dbo].[IdenticalParent] NOT NULL,
 CONSTRAINT [XPKÂèä_äåÿòåëüíîñòè_ïî_çàÿâêå] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocalGoverment2]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalGoverment2](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_Parent] [dbo].[Identical] NULL,
	[Name] [nvarchar](max) NULL,
	[SOATE] [nvarchar](max) NULL,
	[Id_SettlementType] [dbo].[Identical] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
 CONSTRAINT [PK_LocalGoverment2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Message]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [dbo].[IdenticalChild] IDENTITY(1,1) NOT NULL,
	[Id_Parent] [dbo].[IdenticalChild] NULL,
	[MessageText] [nvarchar](max) NULL,
	[MessageDate] [datetime] NULL,
	[Id_Request] [dbo].[IdenticalParent] NOT NULL,
	[Id_QuestionType] [dbo].[IdenticalParent] NULL,
	[Id_AnswerWriter] [dbo].[IdenticalParent] NOT NULL,
 CONSTRAINT [XPKÑîîáùåíèå] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MessageForSentDocument]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageForSentDocument](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_SentDocument] [dbo].[Identical] NOT NULL,
	[Id_Message] [dbo].[IdenticalChild] NOT NULL,
 CONSTRAINT [XPKÏåðåïèñêà_ïî_äîêóìåíòó] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organization]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[TIN] [nvarchar](max) NULL,
	[NSCCode] [nvarchar](max) NULL,
	[RegistrationNumber] [nvarchar](max) NULL,
	[Id_Type] [dbo].[IdenticalParent] NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Id_AdminUnit] [dbo].[Identical] NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÎðãàíèçàöèÿ] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrganizationType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÒèï_Êîìïàíèè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QuestionType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Id_Parent] [dbo].[IdenticalParent] NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÊàòåãîðèÿ_âîïðîñà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestElement]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestElement](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_Parent] [dbo].[Identical] NULL,
	[IsActive] [dbo].[Logical] NULL,
	[OrderNumber] [int] NULL,
	[Id_RequestElemType] [dbo].[IdenticalParent] NOT NULL,
	[Id_TemplateDocument] [dbo].[IdenticalParent] NULL,
	[Id_RequestType] [dbo].[IdenticalParent] NOT NULL,
	[Id_StructureType] [dbo].[IdenticalParent] NOT NULL,
	[IsRequired] [dbo].[Logical] NULL,
 CONSTRAINT [XPKÝëåìåíò_çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestElementType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestElementType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
	[IsNeedTemplate] [dbo].[Logical] NULL,
 CONSTRAINT [XPKÒèï_ýëåìåíòà_çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestElemStructureType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestElemStructureType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÂèä_ñòðóêòóðû_ýëåìåíòà] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestState]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestState](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÑòàòóñ_Çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestStateHistory]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStateHistory](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Id_Request] [dbo].[IdenticalParent] NOT NULL,
	[Id_State] [dbo].[IdenticalParent] NOT NULL,
	[DateStatusChange] [datetime] NULL,
	[Id_User] [int] NULL,
 CONSTRAINT [XPKÈñòîðèÿ_ñòàòóñîâ_çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestStep]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStep](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Id_Parent] [dbo].[IdenticalParent] NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[ActiveImageUrl] [nvarchar](max) NULL,
	[DiabledImageUrl] [nvarchar](max) NULL,
 CONSTRAINT [XPKØàã_çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestType](
	[Id] [dbo].[IdenticalParent] IDENTITY(1,1) NOT NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
	[PrintTemplate_ru] [nvarchar](max) NULL,
	[PrintTemplate_kg] [nvarchar](max) NULL,
 CONSTRAINT [XPKÒèï_Çàÿâêè] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettlementType]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettlementType](
	[Id] [dbo].[IdenticalChild] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Level] [int] NULL,
 CONSTRAINT [XPKТип_населенного_пункта] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sex]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sex](
	[Id] [dbo].[IdenticalChild] IDENTITY(1,1) NOT NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [XPKÏîë] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeAdminUnits]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeAdminUnits](
	[Id] [dbo].[Identical] IDENTITY(1,1) NOT NULL,
	[Name_kg] [nvarchar](max) NULL,
	[Description_kg] [nvarchar](max) NULL,
	[Name_ru] [nvarchar](max) NULL,
	[Description_ru] [nvarchar](max) NULL,
	[CODE] [nvarchar](max) NULL,
 CONSTRAINT [XPKÒèï_Àäìèíèñòðàòèâíîãî_äåëåíèÿ] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[vw_DocumentInRequest]    Script Date: 23.05.2020 22:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_DocumentInRequest]
AS
SELECT        dbo.[Document].Description AS DocumentDescription, dbo.[Document].Name AS DocumentName, dbo.[Document].Filename, dbo.Document_Format.Name_ru AS DocumentFormatName_ru, 
                         dbo.Document_Format.Name_kg AS DocumentFormatName_kg, dbo.Document_Format.Description_ru AS DocumentFormatDecsription_ru, dbo.Document_Format.Description_kg AS DocumentFormatDecsription_kg, 
                         dbo.Document_Format.CODE AS DocumentCode, dbo.DocumentInRequest.Id, dbo.DocumentInRequest.Description, dbo.DocumentInRequest.Id_DocumentState, dbo.DocumentInRequest.Id_Request, 
                         dbo.DocumentInRequest.Id_RequestElement, dbo.DocumentInRequest.Id_Document
FROM            dbo.[Document] INNER JOIN
                         dbo.Document_Format ON dbo.[Document].Id_DocumentFormat = dbo.Document_Format.Id RIGHT OUTER JOIN
                         dbo.DocumentInRequest ON dbo.[Document].Id = dbo.DocumentInRequest.Id_Document

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_User_Id]    Script Date: 23.05.2020 22:04:19 ******/
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[AspNetUserClaims]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 23.05.2020 22:04:19 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 23.05.2020 22:04:19 ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 23.05.2020 22:04:19 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LicenseRequest] ADD  CONSTRAINT [DF_LicenseRequest_IsDraft]  DEFAULT ((1)) FOR [IsDraft]
GO
ALTER TABLE [dbo].[AdminUnits]  WITH CHECK ADD  CONSTRAINT [R_11] FOREIGN KEY([ParentId])
REFERENCES [dbo].[AdminUnits] ([Id])
GO
ALTER TABLE [dbo].[AdminUnits] CHECK CONSTRAINT [R_11]
GO
ALTER TABLE [dbo].[AdminUnits]  WITH CHECK ADD  CONSTRAINT [R_12] FOREIGN KEY([IdTypeadm])
REFERENCES [dbo].[TypeAdminUnits] ([Id])
GO
ALTER TABLE [dbo].[AdminUnits] CHECK CONSTRAINT [R_12]
GO
ALTER TABLE [dbo].[ApplicationUser]  WITH CHECK ADD  CONSTRAINT [R_179] FOREIGN KEY([Id_Sex])
REFERENCES [dbo].[Sex] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUser] CHECK CONSTRAINT [R_179]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_ApplicationUser] FOREIGN KEY([Id_ApplicationUser])
REFERENCES [dbo].[ApplicationUser] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_ApplicationUser]
GO
ALTER TABLE [dbo].[DocTemplForReqElemType]  WITH CHECK ADD  CONSTRAINT [R_173] FOREIGN KEY([Id_Document])
REFERENCES [dbo].[Document] ([Id])
GO
ALTER TABLE [dbo].[DocTemplForReqElemType] CHECK CONSTRAINT [R_173]
GO
ALTER TABLE [dbo].[DocTemplForReqElemType]  WITH CHECK ADD  CONSTRAINT [R_212] FOREIGN KEY([Id_RequestElementType])
REFERENCES [dbo].[RequestElementType] ([Id])
GO
ALTER TABLE [dbo].[DocTemplForReqElemType] CHECK CONSTRAINT [R_212]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [R_211] FOREIGN KEY([Id_DocumentFormat])
REFERENCES [dbo].[Document_Format] ([Id])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [R_211]
GO
ALTER TABLE [dbo].[DocumentInRequest]  WITH CHECK ADD  CONSTRAINT [R_193] FOREIGN KEY([Id_Request])
REFERENCES [dbo].[LicenseRequest] ([Id])
GO
ALTER TABLE [dbo].[DocumentInRequest] CHECK CONSTRAINT [R_193]
GO
ALTER TABLE [dbo].[DocumentInRequest]  WITH CHECK ADD  CONSTRAINT [R_195] FOREIGN KEY([Id_Document])
REFERENCES [dbo].[Document] ([Id])
GO
ALTER TABLE [dbo].[DocumentInRequest] CHECK CONSTRAINT [R_195]
GO
ALTER TABLE [dbo].[DocumentInRequest]  WITH CHECK ADD  CONSTRAINT [R_199] FOREIGN KEY([Id_DocumentState])
REFERENCES [dbo].[DocumentState] ([Id])
GO
ALTER TABLE [dbo].[DocumentInRequest] CHECK CONSTRAINT [R_199]
GO
ALTER TABLE [dbo].[DocumentInRequest]  WITH CHECK ADD  CONSTRAINT [R_204] FOREIGN KEY([Id_RequestElement])
REFERENCES [dbo].[RequestElement] ([Id])
GO
ALTER TABLE [dbo].[DocumentInRequest] CHECK CONSTRAINT [R_204]
GO
ALTER TABLE [dbo].[LicenseRequest]  WITH CHECK ADD  CONSTRAINT [R_185] FOREIGN KEY([Id_Company])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequest] CHECK CONSTRAINT [R_185]
GO
ALTER TABLE [dbo].[LicenseRequest]  WITH CHECK ADD  CONSTRAINT [R_187] FOREIGN KEY([Id_AdminUnit])
REFERENCES [dbo].[AdminUnits] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequest] CHECK CONSTRAINT [R_187]
GO
ALTER TABLE [dbo].[LicenseRequest]  WITH CHECK ADD  CONSTRAINT [R_188] FOREIGN KEY([Id_RequestType])
REFERENCES [dbo].[RequestType] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequest] CHECK CONSTRAINT [R_188]
GO
ALTER TABLE [dbo].[LicenseRequest]  WITH CHECK ADD  CONSTRAINT [R_192] FOREIGN KEY([Id_User])
REFERENCES [dbo].[ApplicationUser] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequest] CHECK CONSTRAINT [R_192]
GO
ALTER TABLE [dbo].[LicenseRequestActivityType]  WITH CHECK ADD  CONSTRAINT [R_180] FOREIGN KEY([Id_Type])
REFERENCES [dbo].[ActivityType] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequestActivityType] CHECK CONSTRAINT [R_180]
GO
ALTER TABLE [dbo].[LicenseRequestActivityType]  WITH CHECK ADD  CONSTRAINT [R_181] FOREIGN KEY([Id_Request])
REFERENCES [dbo].[LicenseRequest] ([Id])
GO
ALTER TABLE [dbo].[LicenseRequestActivityType] CHECK CONSTRAINT [R_181]
GO
ALTER TABLE [dbo].[LocalGoverment2]  WITH CHECK ADD  CONSTRAINT [FK_LocalGoverment2_SettlementType] FOREIGN KEY([Id_SettlementType])
REFERENCES [dbo].[SettlementType] ([Id])
GO
ALTER TABLE [dbo].[LocalGoverment2] CHECK CONSTRAINT [FK_LocalGoverment2_SettlementType]
GO
ALTER TABLE [dbo].[LocalGoverment2]  WITH CHECK ADD  CONSTRAINT [FK_Parent_Child] FOREIGN KEY([Id_Parent])
REFERENCES [dbo].[LocalGoverment2] ([Id])
GO
ALTER TABLE [dbo].[LocalGoverment2] CHECK CONSTRAINT [FK_Parent_Child]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [R_189] FOREIGN KEY([Id_Request])
REFERENCES [dbo].[LicenseRequest] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [R_189]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [R_191] FOREIGN KEY([Id_QuestionType])
REFERENCES [dbo].[QuestionType] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [R_191]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [R_206] FOREIGN KEY([Id_AnswerWriter])
REFERENCES [dbo].[ApplicationUser] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [R_206]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [R_207] FOREIGN KEY([Id_Parent])
REFERENCES [dbo].[Message] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [R_207]
GO
ALTER TABLE [dbo].[MessageForSentDocument]  WITH CHECK ADD  CONSTRAINT [R_197] FOREIGN KEY([Id_SentDocument])
REFERENCES [dbo].[DocumentInRequest] ([Id])
GO
ALTER TABLE [dbo].[MessageForSentDocument] CHECK CONSTRAINT [R_197]
GO
ALTER TABLE [dbo].[MessageForSentDocument]  WITH CHECK ADD  CONSTRAINT [R_198] FOREIGN KEY([Id_Message])
REFERENCES [dbo].[Message] ([Id])
GO
ALTER TABLE [dbo].[MessageForSentDocument] CHECK CONSTRAINT [R_198]
GO
ALTER TABLE [dbo].[Organization]  WITH CHECK ADD  CONSTRAINT [R_184] FOREIGN KEY([Id_Type])
REFERENCES [dbo].[OrganizationType] ([Id])
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [R_184]
GO
ALTER TABLE [dbo].[Organization]  WITH CHECK ADD  CONSTRAINT [R_186] FOREIGN KEY([Id_AdminUnit])
REFERENCES [dbo].[AdminUnits] ([Id])
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [R_186]
GO
ALTER TABLE [dbo].[QuestionType]  WITH CHECK ADD  CONSTRAINT [R_190] FOREIGN KEY([Id_Parent])
REFERENCES [dbo].[QuestionType] ([Id])
GO
ALTER TABLE [dbo].[QuestionType] CHECK CONSTRAINT [R_190]
GO
ALTER TABLE [dbo].[RequestElement]  WITH CHECK ADD  CONSTRAINT [R_200] FOREIGN KEY([Id_Parent])
REFERENCES [dbo].[RequestElement] ([Id])
GO
ALTER TABLE [dbo].[RequestElement] CHECK CONSTRAINT [R_200]
GO
ALTER TABLE [dbo].[RequestElement]  WITH CHECK ADD  CONSTRAINT [R_201] FOREIGN KEY([Id_RequestElemType])
REFERENCES [dbo].[RequestElementType] ([Id])
GO
ALTER TABLE [dbo].[RequestElement] CHECK CONSTRAINT [R_201]
GO
ALTER TABLE [dbo].[RequestElement]  WITH CHECK ADD  CONSTRAINT [R_202] FOREIGN KEY([Id_TemplateDocument])
REFERENCES [dbo].[DocTemplForReqElemType] ([Id])
GO
ALTER TABLE [dbo].[RequestElement] CHECK CONSTRAINT [R_202]
GO
ALTER TABLE [dbo].[RequestElement]  WITH CHECK ADD  CONSTRAINT [R_203] FOREIGN KEY([Id_RequestType])
REFERENCES [dbo].[RequestType] ([Id])
GO
ALTER TABLE [dbo].[RequestElement] CHECK CONSTRAINT [R_203]
GO
ALTER TABLE [dbo].[RequestElement]  WITH CHECK ADD  CONSTRAINT [R_210] FOREIGN KEY([Id_StructureType])
REFERENCES [dbo].[RequestElemStructureType] ([Id])
GO
ALTER TABLE [dbo].[RequestElement] CHECK CONSTRAINT [R_210]
GO
ALTER TABLE [dbo].[RequestStateHistory]  WITH CHECK ADD  CONSTRAINT [FK_RequestStateHistory_ApplicationUser] FOREIGN KEY([Id_User])
REFERENCES [dbo].[ApplicationUser] ([Id])
GO
ALTER TABLE [dbo].[RequestStateHistory] CHECK CONSTRAINT [FK_RequestStateHistory_ApplicationUser]
GO
ALTER TABLE [dbo].[RequestStateHistory]  WITH CHECK ADD  CONSTRAINT [R_182] FOREIGN KEY([Id_Request])
REFERENCES [dbo].[LicenseRequest] ([Id])
GO
ALTER TABLE [dbo].[RequestStateHistory] CHECK CONSTRAINT [R_182]
GO
ALTER TABLE [dbo].[RequestStateHistory]  WITH CHECK ADD  CONSTRAINT [R_183] FOREIGN KEY([Id_State])
REFERENCES [dbo].[RequestState] ([Id])
GO
ALTER TABLE [dbo].[RequestStateHistory] CHECK CONSTRAINT [R_183]
GO
ALTER TABLE [dbo].[RequestStep]  WITH CHECK ADD  CONSTRAINT [R_213] FOREIGN KEY([Id_Parent])
REFERENCES [dbo].[RequestStep] ([Id])
GO
ALTER TABLE [dbo].[RequestStep] CHECK CONSTRAINT [R_213]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Document"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Document_Format"
            Begin Extent = 
               Top = 6
               Left = 274
               Bottom = 136
               Right = 444
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DocumentInRequest"
            Begin Extent = 
               Top = 6
               Left = 482
               Bottom = 136
               Right = 671
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DocumentInRequest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_DocumentInRequest'
GO
USE [master]
GO
ALTER DATABASE [Medlicense] SET  READ_WRITE 
GO

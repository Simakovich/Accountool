USE [master]
GO
/****** Object:  Database [Accountool]    Script Date: 30.04.2024 0:44:53 ******/
CREATE DATABASE [Accountool]
GO
ALTER DATABASE [Accountool] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Accountool].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Accountool] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Accountool] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Accountool] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Accountool] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Accountool] SET ARITHABORT OFF 
GO
ALTER DATABASE [Accountool] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Accountool] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Accountool] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Accountool] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Accountool] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Accountool] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Accountool] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Accountool] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Accountool] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Accountool] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Accountool] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Accountool] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Accountool] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Accountool] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Accountool] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Accountool] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Accountool] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Accountool] SET RECOVERY FULL 
GO
ALTER DATABASE [Accountool] SET  MULTI_USER 
GO
ALTER DATABASE [Accountool] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Accountool] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Accountool] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Accountool] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Accountool] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Accountool] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Accountool', N'ON'
GO
ALTER DATABASE [Accountool] SET QUERY_STORE = ON
GO
ALTER DATABASE [Accountool] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Accountool]
GO
ALTER LOGIN sa WITH PASSWORD='Password1!' 
/****** Object:  Table [dbo].[AspNetClaims]    Script Date: 30.04.2024 0:44:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
USE [Accountool]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 03.05.2024 23:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
CREATE TABLE ClientProfile
(
    Id INT PRIMARY KEY IDENTITY,
    Address NVARCHAR(200) NOT NULL,
    UserId NVARCHAR(450),
    CONSTRAINT FK_ClientProfile_AspNetUsers FOREIGN KEY (UserId)
        REFERENCES AspNetUsers (Id) 
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Town
(
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL
);

CREATE TABLE KioskSection
(
    Id INT PRIMARY KEY IDENTITY,
    NomerKioska NVARCHAR(200) NOT NULL,
    AdresSection NVARCHAR(200) NOT NULL,
    AreaSection FLOAT NOT NULL,
    Kadastr NVARCHAR(200) NOT NULL,
    DataResh DATETIME NOT NULL,
    TypeArenda NVARCHAR(200) NOT NULL,
    Certificate NVARCHAR(200) NOT NULL,
    DateArenda DATETIME NOT NULL
);

CREATE TABLE OrgName
(
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL
);

CREATE TABLE Organization
(
    Id INT PRIMARY KEY IDENTITY,
    OrgNameId INT NOT NULL,
    Dogovor INT NOT NULL,
    Telefon INT NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    [Limit] INT NOT NULL
    CONSTRAINT FK_Kiosk_OrgName FOREIGN KEY (OrgNameId)
        REFERENCES OrgName (Id)
);

CREATE TABLE Kiosk
(
    Id INT PRIMARY KEY IDENTITY,
    Nomer NVARCHAR(200) NOT NULL,
    ModelKiosk NVARCHAR(200) NULL,
    Arenda DATETIME NULL,
    TownId INT NULL,
    Adress NVARCHAR(200) NULL,
    Area FLOAT NOT NULL,
    OrganizationId INT NULL,
    KioskSectionId INT NULL,

    CONSTRAINT FK_Kiosk_Town FOREIGN KEY (TownId)
        REFERENCES Town (Id),

    CONSTRAINT FK_Kiosk_Organization FOREIGN KEY (OrganizationId)
        REFERENCES Organization (Id),

    CONSTRAINT FK_Kiosk_KioskSection FOREIGN KEY (KioskSectionId)
        REFERENCES KioskSection (Id)
);

CREATE TABLE Equipment
(
    Id INT PRIMARY KEY IDENTITY,
    ModelEq NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    PowerEq INT NOT NULL,
    KioskId INT NOT NULL
    CONSTRAINT FK_Equipment_Kiosk FOREIGN KEY (KioskId)
        REFERENCES Kiosk (Id)
);

CREATE TABLE Schetchik
(
    Id INT PRIMARY KEY IDENTITY,
    NomerSchetchika NVARCHAR(200) NOT NULL,
    ModelSchetchika NVARCHAR(200) NOT NULL,
    TexUchet BIT NOT NULL,
    TwoTarif BIT NOT NULL,
    Poverka DATETIME NULL,
    Poteri INT NOT NULL,
    KioskId INT NULL,
    CONSTRAINT FK_Schetchik_Kiosk FOREIGN KEY (KioskId)
        REFERENCES Kiosk(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Indication
(
    Id INT PRIMARY KEY IDENTITY,
    Month DATETIME NOT NULL,
    Tarif1 FLOAT NOT NULL,
    Tarif2 FLOAT NOT NULL,
    TarifSumm FLOAT NOT NULL,
    Archive BIT NOT NULL,
    SchetchikId INT NOT NULL,
    CONSTRAINT FK_Indication_Schetchik FOREIGN KEY (SchetchikId)
        REFERENCES Schetchik (Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE UserKiosk
(
    UserId NVARCHAR(450),
    KioskId INT,
    PRIMARY KEY (UserId, KioskId),
    CONSTRAINT FK_UserKiosks_AspNetUsers FOREIGN KEY (UserId)
        REFERENCES AspNetUsers (Id) 
        ON DELETE CASCADE, 
    CONSTRAINT FK_UserKiosks_Kiosks FOREIGN KEY (KioskId)
        REFERENCES Kiosk (Id)
        ON DELETE CASCADE
);

INSERT INTO Town (Name)
VALUES 
('Gomel'), 
('Vetka'), 
('Buda-Koshelevo'), 
('Dobrush'), 
('Rechitsa'), 
('Khoyniki'), 
('Zhlobin'), 
('Rogachev'), 
('Korma'), 
('Bragin'), 
('Loev'), 
('Chechersk'), 
('Svetlogorsk'), 
('Oktyabrsky'), 
('Mozyr'), 
('Kalinkovichi'), 
('Petrikov'), 
('Zhitkovichi'), 
('Turov'), 
('Narovlya'), 
('Yelsk'), 
('Lelchitsy');

INSERT INTO OrgName (Name)
VALUES 
('Energosbyt'), 
('BelGUT'),
('Distanciya_Gomel'), 
('Distanciya_Zhlobin'), 
('KZHREUP'),
('Hospital'),
('GorElectroTransport');
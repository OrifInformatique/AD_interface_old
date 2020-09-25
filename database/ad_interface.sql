USE [master]
GO
/****** Object:  Database [ad_interface]    Script Date: 25.09.2020 11:08:43 ******/
CREATE DATABASE [ad_interface]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ad_interface', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ad_interface.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ad_interface_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ad_interface_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ad_interface] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ad_interface].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ad_interface] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ad_interface] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ad_interface] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ad_interface] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ad_interface] SET ARITHABORT OFF 
GO
ALTER DATABASE [ad_interface] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ad_interface] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ad_interface] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ad_interface] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ad_interface] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ad_interface] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ad_interface] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ad_interface] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ad_interface] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ad_interface] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ad_interface] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ad_interface] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ad_interface] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ad_interface] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ad_interface] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ad_interface] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ad_interface] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ad_interface] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ad_interface] SET  MULTI_USER 
GO
ALTER DATABASE [ad_interface] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ad_interface] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ad_interface] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ad_interface] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ad_interface] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ad_interface] SET QUERY_STORE = OFF
GO
USE [ad_interface]
GO
/****** Object:  Table [dbo].[t_errors]    Script Date: 25.09.2020 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_errors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[samaccountname_a] [nvarchar](50) NOT NULL,
	[samaccountname_b] [nvarchar](50) NOT NULL,
	[field_a] [nvarchar](50) NOT NULL,
	[field_b] [nvarchar](50) NOT NULL,
	[value_a] [nvarchar](50) NOT NULL,
	[expected_value_a] [nvarchar](50) NOT NULL,
	[value_b] [nvarchar](50) NOT NULL,
	[save_date] [datetime] NOT NULL,
	[validate] [bit] NOT NULL,
	[validation_date] [datetime] NULL,
	[validator] [nvarchar](50) NULL,
	[fixed] [bit] NOT NULL,
	[fix_date] [datetime] NULL,
	[corrector] [nvarchar](50) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[t_errors] ADD  CONSTRAINT [DF_t_errors_save_date]  DEFAULT (getdate()) FOR [save_date]
GO
ALTER TABLE [dbo].[t_errors] ADD  CONSTRAINT [DF_t_errors_validate]  DEFAULT ((0)) FOR [validate]
GO
ALTER TABLE [dbo].[t_errors] ADD  CONSTRAINT [DF_t_errors_fixed]  DEFAULT ((0)) FOR [fixed]
GO
USE [master]
GO
ALTER DATABASE [ad_interface] SET  READ_WRITE 
GO

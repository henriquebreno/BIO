USE [BIO-data]
GO

/****** Object:  Table [dbo].[GasMeteringPoint]    Script Date: 13/03/2024 19:22:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GasMeteringPoint]') AND type in (N'U'))
DROP TABLE [dbo].[GasMeteringPoint]
GO

/****** Object:  Table [dbo].[GasMeteringPoint]    Script Date: 13/03/2024 19:22:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GasMeteringPoint](
	[Id] [bigint] NOT NULL,
	[MeterId] [bigint] NOT NULL,
	[EffectiveStartTimeUtc] [datetime] NULL,
	[EffectiveEndTimeUtc] [datetime] NULL,
	[InDelivery] [bit] NULL,
	[PriceAreaCode] [nvarchar](255) NULL,
	[InstallationCountry] [nvarchar](255) NULL,
	[InstallationMunicipalityCode] [nvarchar](255) NULL,
	[InstalationPostalCode] [nvarchar](255) NULL,
	[InstallationCity] [nvarchar](255) NULL,
	[InstallationStreetName] [nvarchar](255) NULL,
	[InstallationBuildingNumber] [nvarchar](255) NULL,
	[InstallationBuildingFloor] [nvarchar](255) NULL,
	[InstallationRoomIdentification] [nvarchar](255) NULL,
	[CalorificValueAreaId] [bigint] NULL,
	[Source] [nvarchar](255) NULL,
 CONSTRAINT [PK_GasMeteringPoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


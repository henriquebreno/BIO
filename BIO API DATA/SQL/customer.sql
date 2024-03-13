USE [BIO-data]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 13/03/2024 19:21:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
DROP TABLE [dbo].[Customer]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 13/03/2024 19:21:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Id] [bigint] NOT NULL,
	[CustomerNumber] [nvarchar](255) NULL,
	[EffectiveStartTimeUtc] [datetime] NULL,
	[EffectiveEndTimeUtc] [datetime] NULL,
	[VatIdentification] [nvarchar](255) NULL,
	[CustomerName] [nvarchar](255) NULL,
	[Country] [nvarchar](255) NULL,
	[MunicipalityCode] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[StreetName] [nvarchar](255) NULL,
	[BuildingNumber] [nvarchar](255) NULL,
	[BuildingFloor] [nvarchar](255) NULL,
	[RoomIdentification] [nvarchar](255) NULL,
	[Source] [nvarchar](255) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


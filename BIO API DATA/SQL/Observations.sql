USE [BIO-data]
GO

ALTER TABLE [dbo].[Observations] DROP CONSTRAINT [FK_Observations_GasMeterMeasurements]
GO

/****** Object:  Table [dbo].[Observations]    Script Date: 13/03/2024 19:23:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Observations]') AND type in (N'U'))
DROP TABLE [dbo].[Observations]
GO

/****** Object:  Table [dbo].[Observations]    Script Date: 13/03/2024 19:23:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Observations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GasMeterMeasurementId] [bigint] NULL,
	[Quality] [nvarchar](255) NULL,
	[Value] [decimal](18, 2) NULL,
	[Position] [int] NULL,
	[Correction] [bit] NULL,
 CONSTRAINT [PK_Observations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Observations]  WITH CHECK ADD  CONSTRAINT [FK_Observations_GasMeterMeasurements] FOREIGN KEY([GasMeterMeasurementId])
REFERENCES [dbo].[GasMeterMeasurements] ([Id])
GO

ALTER TABLE [dbo].[Observations] CHECK CONSTRAINT [FK_Observations_GasMeterMeasurements]
GO


USE [BIO-data]
GO

ALTER TABLE [dbo].[GasMeterMeasurements] DROP CONSTRAINT [FK_GasMeterMeasurements_GasMeteringPoint]
GO

/****** Object:  Table [dbo].[GasMeterMeasurements]    Script Date: 13/03/2024 19:23:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GasMeterMeasurements]') AND type in (N'U'))
DROP TABLE [dbo].[GasMeterMeasurements]
GO

/****** Object:  Table [dbo].[GasMeterMeasurements]    Script Date: 13/03/2024 19:23:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GasMeterMeasurements](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Start] [datetime] NULL,
	[End] [datetime] NULL,
	[Resolution] [nvarchar](255) NULL,
	[Unit] [nvarchar](255) NULL,
	[MeteringPointIdentification] [bigint] NULL,
 CONSTRAINT [PK_GasMeterMeasurements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_GasMeterMeasurements] UNIQUE NONCLUSTERED 
(
	[MeteringPointIdentification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GasMeterMeasurements]  WITH CHECK ADD  CONSTRAINT [FK_GasMeterMeasurements_GasMeteringPoint] FOREIGN KEY([MeteringPointIdentification])
REFERENCES [dbo].[GasMeteringPoint] ([Id])
GO

ALTER TABLE [dbo].[GasMeterMeasurements] CHECK CONSTRAINT [FK_GasMeterMeasurements_GasMeteringPoint]
GO


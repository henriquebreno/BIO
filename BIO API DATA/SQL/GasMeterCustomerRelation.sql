USE [BIO-data]
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation] DROP CONSTRAINT [FK_GasMeterCustomerRelation_GasMeteringPoint]
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation] DROP CONSTRAINT [FK_GasMeterCustomerRelation_Customer]
GO

/****** Object:  Table [dbo].[GasMeterCustomerRelation]    Script Date: 13/03/2024 19:22:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GasMeterCustomerRelation]') AND type in (N'U'))
DROP TABLE [dbo].[GasMeterCustomerRelation]
GO

/****** Object:  Table [dbo].[GasMeterCustomerRelation]    Script Date: 13/03/2024 19:22:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GasMeterCustomerRelation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveStartTimeUtc] [datetime] NULL,
	[EffectiveEndTimeUtc] [datetime] NULL,
	[CustomerId] [bigint] NULL,
	[GasMeteringPointId] [bigint] NULL,
	[Source] [nvarchar](255) NULL,
 CONSTRAINT [PK_GasMeterCustomerRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation]  WITH CHECK ADD  CONSTRAINT [FK_GasMeterCustomerRelation_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation] CHECK CONSTRAINT [FK_GasMeterCustomerRelation_Customer]
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation]  WITH CHECK ADD  CONSTRAINT [FK_GasMeterCustomerRelation_GasMeteringPoint] FOREIGN KEY([GasMeteringPointId])
REFERENCES [dbo].[GasMeteringPoint] ([Id])
GO

ALTER TABLE [dbo].[GasMeterCustomerRelation] CHECK CONSTRAINT [FK_GasMeterCustomerRelation_GasMeteringPoint]
GO


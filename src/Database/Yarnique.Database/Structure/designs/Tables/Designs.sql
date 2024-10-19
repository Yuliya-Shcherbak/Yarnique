IF OBJECT_ID(N'[designs].[Designs]', N'U') IS NOT NULL
   DROP TABLE [designs].[Designs];
GO

CREATE TABLE [designs].[Designs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[Price] FLOAT NULL,
	[Published] BIT NOT NULL,
	CONSTRAINT [PK_designs_Designs_Id] PRIMARY KEY ([Id] ASC)
)
GO
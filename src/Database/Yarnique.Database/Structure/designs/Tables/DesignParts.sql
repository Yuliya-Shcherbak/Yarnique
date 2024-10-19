IF OBJECT_ID(N'[designs].[DesignParts]', N'U') IS NOT NULL
   DROP TABLE [designs].[DesignParts];
GO

CREATE TABLE [designs].[DesignParts]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	CONSTRAINT [PK_consumables_DesignParts_Id] PRIMARY KEY ([Id] ASC)
)
GO
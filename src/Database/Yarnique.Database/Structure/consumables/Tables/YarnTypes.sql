CREATE TABLE [consumables].[YarnTypes]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	CONSTRAINT [PK_consumables_YarnTypes_Id] PRIMARY KEY ([Id] ASC)
)
GO
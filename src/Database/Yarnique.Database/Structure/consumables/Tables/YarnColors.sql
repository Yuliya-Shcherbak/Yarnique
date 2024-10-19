IF OBJECT_ID(N'[consumables].[YarnColors]', N'U') IS NOT NULL
	DROP TABLE [consumables].[YarnColors];
GO

CREATE TABLE [consumables].[YarnColors]
(
	[YarnId] UNIQUEIDENTIFIER NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[Available] BIT NOT NULL,
	CONSTRAINT [PK_consumables_YarnColors_Id] PRIMARY KEY ([YarnId] ASC, [Code] ASC),
	CONSTRAINT [FK_consumables_YarnColors_Yarns] FOREIGN KEY ([YarnId]) REFERENCES [consumables].Yarns([Id])
)
GO
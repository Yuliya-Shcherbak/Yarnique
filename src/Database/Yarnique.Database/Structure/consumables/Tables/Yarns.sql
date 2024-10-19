CREATE TABLE [consumables].Yarns
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[ProducerId] UNIQUEIDENTIFIER NOT NULL,
	[TypeId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [PK_consumables_Yarns_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_consumables_Yarns_Producers] FOREIGN KEY (ProducerId) REFERENCES [consumables].Producers(Id),
	CONSTRAINT [FK_consumables_Yarns_YarnTypes] FOREIGN KEY (TypeId) REFERENCES [consumables].YarnTypes(Id)
)
GO
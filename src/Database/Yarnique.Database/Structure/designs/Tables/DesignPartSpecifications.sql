IF OBJECT_ID(N'[designs].[DesignPartSpecifications]', N'U') IS NOT NULL
   DROP TABLE [designs].[DesignPartSpecifications];
GO

CREATE TABLE [designs].[DesignPartSpecifications]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[DesignId] UNIQUEIDENTIFIER NOT NULL,
	[DesignPartId] UNIQUEIDENTIFIER NOT NULL,
	--[YarnId] UNIQUEIDENTIFIER NOT NULL,
	--[YarnCode] VARCHAR(20) NOT NULL,
	[YarnAmount] INT NOT NULL,
	CONSTRAINT [PK_designs_DesignPartSpecification_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_designs_DesignPartSpecification_designs_Designs] FOREIGN KEY ([DesignId]) REFERENCES [designs].[Designs]([Id]),
	CONSTRAINT [FK_designs_DesignPartSpecification_designs_DesignParts] FOREIGN KEY ([DesignPartId]) REFERENCES [designs].[DesignParts]([Id]),
	--CONSTRAINT [FK_designs_DesignPartSpecification_consumables_Yarns] FOREIGN KEY ([YarnId]) REFERENCES [consumables].Yarns([Id]),
	--CONSTRAINT [FK_designs_DesignPartSpecification_consumables_YarnColors] FOREIGN KEY ([YarnId], [YarnCode]) REFERENCES [consumables].YarnColors([YarnId], [Code])
)
GO
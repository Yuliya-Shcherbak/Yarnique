IF EXISTS ( SELECT *
            FROM   sysobjects
            WHERE  id = object_id(N'[orders].[CopyPublishedDesign]')
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [orders].[CopyPublishedDesign]
END
GO

CREATE PROCEDURE [orders].[CopyPublishedDesign] @DesignId UNIQUEIDENTIFIER
AS
	DECLARE @NewDesignId UNIQUEIDENTIFIER
	DECLARE @TempDesigns TABLE
	(
		[Id] UNIQUEIDENTIFIER NOT NULL,
		[Name] VARCHAR(255) NOT NULL,
		[Price] FLOAT NULL,
		[Discontinued] BIT NOT NULL,
		[SellerId] UNIQUEIDENTIFIER NOT NULL
	)

	DECLARE @TempDesignSpecifications TABLE
	(
		[Id] UNIQUEIDENTIFIER NOT NULL,
		[DesignId] UNIQUEIDENTIFIER NOT NULL,
		[DesignPartId] UNIQUEIDENTIFIER NOT NULL,
		[YarnAmount] INT NOT NULL,
		[Term] VARCHAR(255) NOT NULL,
		[ExecutionOrder] INT NOT NULL
	)

	DECLARE @TempDesignSpecificationsWithNewParts TABLE
	(
		[Id] UNIQUEIDENTIFIER NOT NULL,
		[DesignId] UNIQUEIDENTIFIER NOT NULL,
		[DesignPartId] UNIQUEIDENTIFIER NOT NULL,
		[Name] VARCHAR(255) NOT NULL,
		[BlobName] VARCHAR(255) NULL,
		[YarnAmount] INT NOT NULL,
		[Term] VARCHAR(255) NOT NULL,
		[ExecutionOrder] INT NOT NULL
	)

	INSERT INTO @TempDesigns
	SELECT
		d.Id
		, d.Name
		, d.Price
		, 0
		, d.SellerId
	FROM [designs].[Designs] AS d
	WHERE d.Id = @DesignId

	SELECT @NewDesignId = Id FROM @TempDesigns

	INSERT INTO @TempDesignSpecifications
	SELECT
		dps.Id
		, @NewDesignId
		, odp.Id
		, dps.YarnAmount
		, dps.Term
		, dps.ExecutionOrder
	FROM [designs].[DesignPartSpecifications] AS dps
	JOIN [designs].[DesignParts] AS dp ON dps.DesignPartId = dp.Id
	JOIN [orders].[DesignParts] AS odp ON dp.Name = odp.Name
	WHERE dps.DesignId = @DesignId

	INSERT INTO @TempDesignSpecificationsWithNewParts
	SELECT
		dps.Id
		, @NewDesignId
		, dps.DesignPartId
		, dp.Name
		, dp.BlobName
		, dps.YarnAmount
		, dps.Term
		, dps.ExecutionOrder
	FROM [designs].[DesignPartSpecifications] AS dps
	JOIN [designs].[DesignParts] AS dp ON dps.DesignPartId = dp.Id
	WHERE dps.DesignId = @DesignId AND dp.Name NOT IN (SELECT Name FROM [orders].[DesignParts])

	BEGIN TRANSACTION [CopyPublishedDesignTransaction]
	BEGIN TRY
		INSERT INTO [orders].[Designs] SELECT [Id], [Name], [Price], [Discontinued], [SellerId] FROM @TempDesigns
		INSERT INTO [orders].[DesignParts] SELECT [DesignPartId], [Name], [BlobName] FROM @TempDesignSpecificationsWithNewParts
		INSERT INTO [orders].[DesignPartSpecifications] SELECT [Id], [DesignId], [DesignPartId], [YarnAmount], [Term], [ExecutionOrder] FROM @TempDesignSpecifications
		INSERT INTO [orders].[DesignPartSpecifications] SELECT [Id], [DesignId], [DesignPartId], [YarnAmount], [Term], [ExecutionOrder] FROM @TempDesignSpecificationsWithNewParts
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION [CopyPublishedDesignTransaction]
	END CATCH
GO
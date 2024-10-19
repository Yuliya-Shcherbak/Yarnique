DECLARE @woolYarnTypeId uniqueidentifier = NEWID();
DECLARE @cottonYarnTypeId uniqueidentifier = NEWID();
DECLARE @acrylicYarnTypeId uniqueidentifier = NEWID();

DECLARE @YarnArtProducerId uniqueidentifier = NEWID();
DECLARE @AlizeProducerId uniqueidentifier = NEWID();
DECLARE @LanossoProducerId uniqueidentifier = NEWID();

DECLARE @YarnArt_BabyCotton_Id uniqueidentifier = NEWID();
DECLARE @YarnArt_AdoreDream_Id uniqueidentifier = NEWID();
DECLARE @Lanoso_Cottonax_Id uniqueidentifier = NEWID();
DECLARE @Lanoso_Woolrich_Id uniqueidentifier = NEWID();

PRINT N'Seeding YarnTypes...';

INSERT INTO [consumables].[YarnTypes] VALUES (@woolYarnTypeId, 'Wool')
INSERT INTO [consumables].[YarnTypes] VALUES (@cottonYarnTypeId, 'Cotton')
INSERT INTO [consumables].[YarnTypes] VALUES (@acrylicYarnTypeId, 'Acrylic')

PRINT N'Seeding Producers...';

INSERT INTO [consumables].[Producers] VALUES (@YarnArtProducerId, 'YarnArt')
INSERT INTO [consumables].[Producers] VALUES (@AlizeProducerId, 'Alize')
INSERT INTO [consumables].[Producers] VALUES (@LanossoProducerId, 'Lanoso')

PRINT N'Seeding Yarns...';

INSERT INTO [consumables].[Yarns] VALUES (@YarnArt_BabyCotton_Id, 'Baby Cotton', @YarnArtProducerId, @cottonYarnTypeId)
INSERT INTO [consumables].[Yarns] VALUES (@Lanoso_Woolrich_Id, 'Woolrich', @LanossoProducerId, @woolYarnTypeId)
INSERT INTO [consumables].[Yarns] VALUES (@Lanoso_Cottonax_Id, 'Cottonax', @LanossoProducerId, @cottonYarnTypeId)

PRINT N'Seeding YarnColors...';
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '400', 'White', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '423', 'Red', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '460', 'Black', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '452', 'Grey', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '430', 'Lemon', 0)
INSERT INTO [consumables].[YarnColors] VALUES (@YarnArt_BabyCotton_Id, '414', 'Rose', 1)

INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '901', 'Ecru', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '958', 'Navy Blue', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '930', 'Dark Green', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '954', 'Saxe Blue', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '955', 'White', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Cottonax_Id, '952', 'Anthracite', 1)

INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2001', 'Ecru', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2025', 'Orange', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2031', 'Light Fuchsia', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2003', 'Beige', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2023', 'Eggplant Purple', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2027', 'Red', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2029', 'Emerald', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2059', 'Purple', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2032', 'Shocking Pink', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2065', 'Lilac', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '13215', 'Henna Green', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2005', 'Brown', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2018', 'Ice Blue', 1)
INSERT INTO [consumables].[YarnColors] VALUES (@Lanoso_Woolrich_Id, '2026', 'Salmon Pink', 1)
PRINT N'Creating database...';

CREATE DATABASE [Yarnique]
    CONTAINMENT = NONE
GO

PRINT N'Creating [consumables]...';

GO
CREATE SCHEMA [consumables]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [designs]...';


GO
CREATE SCHEMA [designs]
    AUTHORIZATION [dbo];

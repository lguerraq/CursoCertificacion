CREATE TABLE [dbo].[Pais] (
    [IdPais]     INT           IDENTITY (1, 1) NOT NULL,
    [NombrePais] VARCHAR (200) NULL,
    CONSTRAINT [PK_Pais] PRIMARY KEY CLUSTERED ([IdPais] ASC)
);


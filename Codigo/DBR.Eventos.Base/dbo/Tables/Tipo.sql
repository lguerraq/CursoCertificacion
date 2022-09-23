CREATE TABLE [dbo].[Tipo] (
    [IdTipo]      INT          IDENTITY (1, 1) NOT NULL,
    [NombreTipo]  VARCHAR (50) NULL,
    [Grupo]       VARCHAR (50) NULL,
    [Valor]       INT          NULL,
    [Abreviatura] VARCHAR (10) NULL,
    CONSTRAINT [PK_Tipo] PRIMARY KEY CLUSTERED ([IdTipo] ASC)
);


CREATE TABLE [dbo].[Profesion] (
    [IdProfesion] INT           IDENTITY (1, 1) NOT NULL,
    [Descripcion] VARCHAR (250) NULL,
    CONSTRAINT [PK_Profesion] PRIMARY KEY CLUSTERED ([IdProfesion] ASC)
);

